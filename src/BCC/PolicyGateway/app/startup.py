from flask import Flask, Blueprint, request, Response, abort, jsonify
from werkzeug.exceptions import BadRequest
import requests
import json
import os
import re

#docker build -t docker-flask:latest .; docker stop flaskapp; docker rm flaskapp; docker run --name flaskapp -d -e DEPLOYER_IP=140.140.140.30:6000 -p 5000:5000 docker-flask:latest; docker logs flaskapp -f

_deployer_ip = os.environ['DEPLOYER_IP'] if os.environ.get('DEPLOYER_IP') is not None else '140.140.140.30:6000'
_fetcher_ip = os.environ['FETCHER_IP'] if os.environ.get('FETCHER_IP') is not None else '140.140.140.30:80'

_app = Flask(__name__)

class InvalidUsage(Exception):
    status_code = 400

    def __init__(self, message, status_code=None, payload=None):
        Exception.__init__(self)
        self.message = message
        if status_code is not None:
            self.status_code = status_code
        self.payload = payload

    def to_dict(self):
        rv = dict(self.payload or ())
        rv['message'] = self.message
        return rv

def check_add_policy_request(request_json):
    needed_keys = ["json_policy", "policy_creation_token", "wallet_id", "api_key"]
    missing_keys = []

    for key in needed_keys:
        if(not (key in request_json)):
            missing_keys.append(key)

    return missing_keys

def deploy_policy(json_policy, wallet_id):
    import requests

    url = "http://" + _deployer_ip + "/blockchain_policy_deployer/deploy"

    json_policy = re.sub(r'\"', '\\\"', json_policy)
    
    payload = "{\n\"json_policy\":\"" + json_policy + "\",\n \"wallet_id\": \"" + wallet_id + "\"\n}"
    
    headers = {
        'Content-Type': "application/json",
        'User-Agent': "PostmanRuntime/7.15.2",
        'Accept': "*/*",
        'Cache-Control': "no-cache",
        'Host': _deployer_ip,
        'Accept-Encoding': "gzip, deflate",
        'Connection': "keep-alive",
        'cache-control': "no-cache"
        }
    
    response = requests.request("POST", url, data=payload, headers=headers)
           
    _app.logger.info(response.text)

    return response.text
    
def test_fetch(api_key):
    import requests

    url = "http://" + _fetcher_ip + "/fetcher/testfetch/" + api_key +"/1/1"

    headers = {
        'User-Agent': "PostmanRuntime/7.15.2",
        'Accept': "*/*",
        'Host': _fetcher_ip,
        'Accept-Encoding': "gzip, deflate",
        'Connection': "keep-alive",
        'cache-control': "no-cache"
        }

    response = requests.request("GET", url, headers=headers)

    return json.loads(response.text)['Result'] == True

def handle_invalid_usage(error):
    response = jsonify(error.to_dict())
    response.status_code = error.status_code
    return response

@_app.route('/addpolicy', methods=['POST'])
def add_policy():
    body = request.get_json()
    #Check if the body is valid
    missing_keys = check_add_policy_request(body)
    if(len(missing_keys) > 0):
        raise BadRequest("Missing Keys: " + str(missing_keys))

    if(not test_fetch(body['api_key'])):
        raise BadRequest("Failed test fetch")

    result = deploy_policy(body['json_policy'], body['wallet_id'])

    return result

if __name__ == "__main__":
    _app.run(host = '0.0.0.0', port = 5000, debug = True)

