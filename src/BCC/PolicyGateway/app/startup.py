from flask import Flask, Blueprint, request, Response, abort, jsonify
from werkzeug.exceptions import BadRequest
import requests
import json
import os
import re

_deployer_ip = os.environ['DEPLOYER_IP']
_fetcher_ip = os.environ['FETCHER_IP']
_policy_token_ip = os.environ['POLICY_TOKEN_IP']

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

def check_policy_create_token(token):
    url = "http://" + _policy_token_ip + "/bcc_policy_token_gateway/checktoken/" + token

    headers = {
        'Accept': "*/*",
        'Cache-Control': "no-cache",
        'Host': _policy_token_ip,
        'Accept-Encoding': "gzip, deflate",
        'Connection': "keep-alive",
        'cache-control': "no-cache"
        }

    response = requests.request("GET", url, headers=headers)

    return response.status_code == 200 and json.loads(response.text)['status'] == "success"

@_app.route('/addpolicy', methods=['POST'])
def add_policy():
    body = request.get_json()
    #Check if the body is valid
    missing_keys = check_add_policy_request(body)
    if(len(missing_keys) > 0):
        raise BadRequest("Missing Keys: " + str(missing_keys))

    if(not check_policy_create_token(body['policy_creation_token'])):
        raise BadRequest("Invalid policy creation token")
    else:
        _app.logger.info("Policy Creation Token Valid")

    if(not test_fetch(body['api_key'])):
        raise BadRequest("Failed test fetch")
    else:
        _app.logger.info("Test Fetch successful")

    result = deploy_policy(body['json_policy'], body['wallet_id'])

    return result

if __name__ == "__main__":
    _app.run(host = '0.0.0.0', port = 5000, debug = True)

