from flask import Flask, Blueprint, request, Response, abort, jsonify
from werkzeug.exceptions import BadRequest
import requests
import json
import os

_app = Flask(__name__)

@_app.route('/policy_drop_off_point/receivepolicy', methods=['POST'])
def add_policy():
	body = request.get_json()

	needed_keys = ["policy_creation_token", "policy_blockchain_location"]
	missing_keys = []

	for key in needed_keys:
		if(not (key in body)):
			missing_keys.append(key)

	if(len(missing_keys) > 0):
		raise BadRequest("Missing Keys: " + str(missing_keys))

	_app.logger.info("Mock Drop off revived")

	return {"result" : "success"}

if __name__ == "__main__":
	_app.run(host = '0.0.0.0', port = 5000, debug = True)
