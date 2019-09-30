#This file is licensed under the GPLv2 - To get a copy visit https://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html

from flask import Flask, Blueprint, request, Response, abort, jsonify
from werkzeug.exceptions import BadRequest
from werkzeug.exceptions import InternalServerError
import requests
import json
import os
import re
import mysql.connector

_deployer_ip = os.environ['DEPLOYER_IP']
_fetcher_ip = os.environ['FETCHER_IP']
_policy_token_ip = os.environ['POLICY_TOKEN_IP']
_mysql_username = os.environ['MYSQL_USERNAME']
_mysql_user_password = os.environ['MYSQL_USER_PASSWORD']
_mysql_port = os.environ['MYSQL_PORT']
_mysql_ip = os.environ['MYSQL_IP']

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
    needed_keys = ["json_policy", "policy_creation_token", "wallet_id", "api_key", "broker_id", "cust_type", "data_type"]
    missing_keys = []

    for key in needed_keys:
        if(not (key in request_json)):
            missing_keys.append(key)

    return missing_keys

def check_broker_id(broker_id):
    mydb = mysql.connector.connect(
        host = _mysql_ip,
        user = _mysql_username,
        passwd = _mysql_user_password,
        port = _mysql_port
    )
    
    cur = mydb.cursor(buffered=True)
    cur.execute("USE main;")
    cur.execute("SELECT * FROM Broker WHERE ID = " + str(broker_id) + ";")
    
    result = cur.rowcount > 0

    cur.close()
    mydb.close()

    return result

def test_fetch(api_key, cust_type, data_type):
    url = "http://" + _fetcher_ip + "/fetcher/testfetch/" + api_key + "/" + cust_type + "/" + data_type

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

def get_broker_info(broker_id):
    mydb = mysql.connector.connect(
        host = _mysql_ip,
        user = _mysql_username,
        passwd = _mysql_user_password,
        port = _mysql_port
    )
    
    cur = mydb.cursor(buffered=True)
    cur.execute("USE main;")
    cur.execute("SELECT * FROM Broker WHERE ID = " + str(broker_id) + " limit 1;")
    row = cur.fetchone()

    # Fetches the Drop off location col named values aren't working
    drop_off_location = row[3]
    broker_wallet_id = row[4] 

    cur.close()
    mydb.close()

    return broker_wallet_id, drop_off_location

def deploy_policy(json_policy, wallet_id, broker_wallet_id):
    import requests

    url = "http://" + _deployer_ip + "/blockchain_policy_deployer/deploy"

    json_policy = re.sub(r'\"', '\\\"', json_policy)
   
    payload = "{\n\"json_policy\":\"" + json_policy + "\",\n \"wallet_id\": \"" + wallet_id + "\"\n, \"broker_wallet_id\":\"" + broker_wallet_id + "\"}"
    
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

def push_to_db(off_chain_policy_id, api_address, data_cust, data_type, on_chain_address, data_broker_id):
    mydb = mysql.connector.connect(
        host = _mysql_ip,
        user = _mysql_username,
        passwd = _mysql_user_password,
        port = _mysql_port
    )
    cur = mydb.cursor()

    cur.execute("USE main;")
    cur.execute("INSERT INTO Policy(OffChainPolicyID, APIAddress, DataCust, DataType, OnchainAddress, DataBrokerID) \
        VALUES('" + off_chain_policy_id + "', '" + api_address + "', " + data_cust + ", " + data_type + ", '" + on_chain_address + "', '" + str(data_broker_id) + "') \
    ;")

    mydb.commit()
    cur.close()
    mydb.close()

def send_to_drop_off(creation_token, policy_blockchain_location, drop_off_location):
    url = "http://" + drop_off_location + "/policy_drop_off_point/receivepolicy"

    payload = "{\n\t\"policy_creation_token\" : \"" + creation_token + "\",\n\t\"policy_blockchain_location\" : \"" + policy_blockchain_location + "\"\n}"
    headers = {
        'Content-Type': "application/json",
        }

    response = requests.request("POST", url, data=payload, headers=headers)

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
    
    if(not check_broker_id(body['broker_id'])):
        raise BadRequest("Invalid Broker ID")
    else:
        _app.logger.info("Broker ID is valid")

    if(not test_fetch(body['api_key'], body['cust_type'], body['data_type'])):
        raise BadRequest("Failed test fetch")   
    else:
        _app.logger.info("Test Fetch successful")

    broker_wallet_id, drop_off_location = get_broker_info(body['broker_id'])

    dep_response_text = deploy_policy(body['json_policy'], body['wallet_id'], broker_wallet_id)
    _app.logger.info("Policy Deployed on blockchain")

    dep_response = json.loads(dep_response_text)

    push_to_db(dep_response['key'], body['api_key'], body['cust_type'], body['data_type'], dep_response['trans_id'], body['broker_id'])
    _app.logger.info("Policy Pushed to DB")
    
    send_to_drop_off(body['policy_creation_token'], dep_response['trans_id'], drop_off_location)
    _app.logger.info("Policy Sent to drop off")

    return {"result" : "success"}

if __name__ == "__main__":
    _app.run(host = '0.0.0.0', port = 5000, debug = True)

