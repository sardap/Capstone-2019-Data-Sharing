from flask import Flask, Blueprint, request, Response, abort, jsonify
import requests
import json
import os
import re

#docker build -t docker-flask:latest .; docker stop flaskapp; docker rm flaskapp; docker run --name flaskapp -d -e DEPLOYER_IP=140.140.140.30:6000 -p 5000:5000 docker-flask:latest; docker logs flaskapp -f

DEPLOYER_IP = os.environ['DEPLOYER_IP']

app = Flask(__name__)

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

    url = "http://" + DEPLOYER_IP + "/blockchain_policy_deployer/deploy"

    json_policy = re.sub(r'\"', '\\\"', json_policy)
    
    payload = "{\n\"json_policy\":\"" + json_policy + "\",\n \"wallet_id\": \"" + wallet_id + "\"\n}"
    
    headers = {
        'Content-Type': "application/json",
        'User-Agent': "PostmanRuntime/7.15.2",
        'Accept': "*/*",
        'Cache-Control': "no-cache",
        'Host': DEPLOYER_IP,
        'Accept-Encoding': "gzip, deflate",
        'Connection': "keep-alive",
        'cache-control': "no-cache"
        }
    
    response = requests.request("POST", url, data=payload, headers=headers)
           
    app.logger.info(response.text)

    return response.text
    
def handle_invalid_usage(error):
    response = jsonify(error.to_dict())
    response.status_code = error.status_code
    return response

@app.route('/addpolicy', methods=['POST'])
def add_policy():
    body = request.get_json()
    #Check if the body is valid
    missing_keys = check_add_policy_request(body)
    if(len(missing_keys) > 0):
        raise InvalidUsage("Missing Keys: " + str(missing_keys), status_code = 400)

    result = deploy_policy(body['json_policy'], body['wallet_id'])

    return result

if __name__ == "__main__":
    app.run(host = '0.0.0.0', port = 5000, debug = True)
