#/usr/bin/python3

import sqlite3
import requests
from os import urandom
from flask import Flask, request
from flask_restful import Api, Resource, reqparse
from binascii import hexlify
from enum import Enum
from werkzeug.exceptions import BadRequest

API_KEY_LENGTH = 32
app = Flask(__name__)

class CreateTokenEnum(Enum):
	INITIAL = 1
	API_SELECTION = 2
	POLICY_DEPLOYED = 3
	DATA_BROKER_INFORMED = 4
	TOKEN_EXPIRED = 5

def static_vars(**kwargs):
    def decorate(func):
        for k in kwargs:
            setattr(func, k, kwargs[k])
        return func
    return decorate

@static_vars(db = sqlite3.connect(':memory:', check_same_thread=False))
def get_db():
	return get_db.db

def update_create_token_state(token, newstate):
	#TODO ADD ENFORCEMENT OF STATE FLOW
	cursor = db.cursor()
	cursor.execute('''UPDATE users SET state = :state WHERE key = :key ''', {'state':newstate,'key':token})

def create_error(errors, code):
	e = BadRequest({"errors" : errors})
	raise e

#TODO Add culling for old unused API KEYS
@app.route("/bcc_policy_gateway/newtoken/<string:apikey>", methods=['GET'])
def create_token(apikey):
	db = get_db()
	cursor = db.cursor()
	cursor.execute('''SELECT apikey FROM DATA_BROKER_INFO WHERE apikey=:apikey''', {'apikey':apikey})
	if(cursor.fetchone() == None):
		return  create_error(["invalid api key"], 400)
	
	create_token = str(hexlify(urandom(API_KEY_LENGTH)), 'utf-8')

	cursor.execute('''INSERT INTO CREATE_TOKENS(
		key, databrokerAPIkey, state) 
		VALUES(:key, :databrokerAPIkey, :state) ''', 
		{
			'key':create_token,
			'databrokerAPIkey':apikey,
			'state':CreateTokenEnum.INITIAL.value
		}
	)

	#TODO add check to ensure token was added

	return create_token, 201

def send_to_policy_dropoff(apikey, policyCreationToken):
	dataBrokerAPIKey
	#TODO Add sending to dropoff
	return {"", "errors":[]}

def test_data_fetch(apikey):
	#TODO CALL FETCHER TEST FETCH
	return True

def send_to_deployer(jsonPolicy, walletID):
	url = 'http://blockchain_policy_deployer/deploy'
	data = {
		"json_policy" : args['json_policy'],
		"wallet_id" : walletID
	}
	response = requests.post(url, data=data)

	#TODO make it contact the real deployer
	return {"value":"xxxxxxxx", "errors":[]}

def insert_policy_apikey_into_db(cursor, dataBrokerAPIKey, dataBrokerPolicyKey, policyBlockchainLocation, apiKey):



	return {"errors" : []}

#TODO Add culling for old unused API KEYS
@app.route("/bcc_policy_gateway/addpolicy", methods=['POST'])
def add_policy():
	args = request.get_json()
	
	print(args['policy_creation_token'])

	db = get_db()
	cursor = db.cursor()
	cursor.execute('''SELECT state FROM CREATE_TOKENS WHERE key=:key''', {'key':args['policy_creation_token']})
	row = cursor.fetchone()
	
	
	if(row == None):
		return create_error(["invalid policy creation token"], 400)
	
	print(row[0])

	state = CreateTokenEnum(row[0])

	#enforces Order of create token
	if(state != CreateTokenEnum.API_SELECTION):
		#TODO add messages for everything other than api selection
		return create_error(["Skiped selecting the API!"], 400)

	if(not test_data_fetch(args['api_key'])):
		return create_error(["api key invalid"], 400)

	blockchainLocation = send_to_deployer(args['json_policy'], args['wallet_id'])
	if(len(blockchainLocation.errors) > 0):
		return create_error(["deployer could not deploy policy"] + blockchainLocation.errors, 400)

	update_create_token_state(args['policy_creation_token'], CreateTokenEnum.POLICY_DEPLOYED)
	
	policyDropoffResult = send_to_policy_dropoff()
	if(len(policyDropoffResult.errors) > 0):
		return create_error(["failed to send to policy dropoff"] + policyDropoffResult.errors, 400)

	policyInsertResult = insert_policy_apikey_into_db(cursor, None, None, blockchainLocation.value, args['api_key'])
	if(len(policyInsertResult.errors) > 0):
		return create_error(["Insert into api policy db error"] + policyInsertResult.errors, 400)

	return create_token, 201

def insert_dummy_data(db):
	cursor = db.cursor()
	cursor.execute('''INSERT INTO DATA_BROKER_INFO(apikey) VALUES(:apikey) ''', {'apikey':"pauliscool"})
	db.commit()
 
def setup_DB(db):
	cursor = db.cursor()
	cursor.execute('''
		CREATE TABLE DATA_BROKER_INFO(
			id INTEGER PRIMARY KEY AUTOINCREMENT, 
			apikey TEXT UNIQUE
		)
	''')

	cursor.execute('''
		CREATE TABLE CREATE_TOKENS(
			key TEXT PRIMARY KEY, 
			databrokerAPIkey TEXT,
			state INTEGER,
			FOREIGN KEY(databrokerapikey) REFERENCES DATA_BROKER_INFO(apikey)
		)
	''')
	db.commit()


def main():
	db = get_db()
	setup_DB(db)
	insert_dummy_data(db)

	app.run(debug=True)

main()
