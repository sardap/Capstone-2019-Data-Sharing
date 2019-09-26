BCC Policy Token Creation Gateway

Written in NodeJS using libraries Express, UUID, and nedb
    - Express is used to provide HTTP support for our HTTP routes
    - NEDB is a small object database, using a subset of MongoDB APIs.  Making it suitable for a prototype which can then be moved to Mongo if it outgrows NEDB
    - UUID is being used to generate a UUID which is used as a token

System dependencies:
    - nodejs 
    - npm 
Libraries (will be pulled in with npm):
	- Express
	- nedb
	- uuidv4
	- mysql connector

To run locally(without a contaienr):
    - run `$npm install` to pull down the dependencies
    - run `$node index.js` to start the server

Environment variables: 
    - `PORT` environment variable.  If no `PORT` variable exists it will default to 8080
      - The dockerfile exposes port 8080
    - MYSQL variables to connect to the broker database:
      - `MYSQL_HOST` IP address or a hostname
      - `MYSQL_USER` the username used to access the database
      - `MYSQL_PASSWORD` password to access the database with the MYSQL_USER
      - `MYSQL_DATABASE` the database to use
    - `DEBUG` The debug variable is a boolean 0 or 1.  When 1 additional routes are made available for debugging purposes.  See Debugging under the additional information section of this README.

Running in Docker:
    - Build with `$docker build -t name/app-name .`
    - For persistent storage mount a directory in the container with /usr/src/app/storage
        - eg `docker run -p 8080:8080 -v $(pwd)/storage:/usr/src/app/storage andrew/token-app` will mount ./storage into the container

Broker keys:
	- The token gateway connects to a remote mysql database containing the broker keys.  Required variables are listed above.  In the testing information there are instructions on how to set up a default database to test this component.

Additional information:
    - The database will be stored in storage/nedb.db 
    - Routes:
        - Generate a new token:     /bcc_policy_token_gateway/newtoken/:brokerapikey
        - Validate existing token:  /bcc_policy_token_gateway/checktoken/:token

	- Debugging:
    	- Set the environment variable DEBUG=1  then go to the route http://IP:PORT/debug/ to see a list of available debug routes
        	- eg /debug/brokers will dump a list of broker API keys


Testing information:
Functionality to test: 
1. Can create token
   1. Doesn't create with an unverified broker id
   2. Generates with verified broker id
2. Can verify token
   1. Returns success with a token
   2. Returns failure with an invalid token
3. State persistance 

All testing procedures documented below assume testing is done locally using the Docker Compose yml file in this directory.  
This component depends on a remote database to query broker APIs.  
The Docker Compose yml will automatically pull in this container, and set it with some default values in the broker table.  The compose file will also set `DEBUG=1` providing access to the debug routes.  See debugging under the additional information section of this README.

Start with:
`docker-compose up`

1. Create a token  
GET request to 127.0.0.1:8080/bcc_policy_token_gateway/newtoken/:brokerapikey  
   1. Replace `:brokerapikey` with a verified broker key.  Currently broker0 --> broker2.  System should return a success and an API key.  Save the API key somewhere to test token verification
   2. Replace `:brokerapikey` with any random string.  System should return a failure.  
2. Can verify tokens  
GET request to 127.0.0.1:8080/bcc_policy_token_gateway/checktoken/:token  

    1. Replace `:token` with a token generated in testing the token creation.  System should return the token exists
    2. Replace `:token` with a random string.  The system should return the token 

3. Stop the container and check the storage/nedb.db file for entries after the previously run steps.  Restart the container and verify test 2.1 works with the previously generated token.

Postman Tests

```json
{
	"info": {
		"name": "Policy_token_creator",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "New Policy Creation Token",
			"event": [
				{
					"listen": "test",
					"script": {
						"id": "f21fb2ff-3fae-47f8-aef4-06cbfc06aede",
						"exec": [
							"pm.test(\"Status Test\", function () {",
							"    var jsonData = pm.response.json();",
							"    pm.expect(jsonData.status).to.eql(\"success\");",
							"});",
							"",
							"pm.test(\"Policy token Created Test\", function () {",
							"    var jsonData = pm.response.json();",
							"    pm.expect(jsonData.policy_creation_token).not.eql(null);",
							"});"
						],
						"type": "text/javascript"
					}
				}
			],
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "localhost:8080/bcc_policy_token_gateway/newtoken/broker1",
					"host": [
						"localhost"
					],
					"port": "8080",
					"path": [
						"bcc_policy_token_gateway",
						"newtoken",
						"broker1"
					]
				}
			},
			"response": []
		},
		{
			"name": "New Policy Creation Token Fail",
			"event": [
				{
					"listen": "test",
					"script": {
						"id": "f21fb2ff-3fae-47f8-aef4-06cbfc06aede",
						"exec": [
							"pm.test(\"Body matches string\", function () {",
							"    pm.expect(pm.response.text()).to.include(\"Broker API key invalid\");",
							"});"
						],
						"type": "text/javascript"
					}
				}
			],
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "localhost:8080/bcc_policy_token_gateway/newtoken/SpamThisFLOWERToGIVEN0tailPOWER",
					"host": [
						"localhost"
					],
					"port": "8080",
					"path": [
						"bcc_policy_token_gateway",
						"newtoken",
						"SpamThisFLOWERToGIVEN0tailPOWER"
					]
				}
			},
			"response": []
		},
		{
			"name": "Check policy Creation Token",
			"event": [
				{
					"listen": "test",
					"script": {
						"id": "b9f8beee-7915-41a0-8e41-e9b9e6888da1",
						"exec": [
							"pm.test(\"Status Test\", function () {",
							"    var jsonData = pm.response.json();",
							"    pm.expect(jsonData.status).to.eql(\"success\");",
							"});"
						],
						"type": "text/javascript"
					}
				}
			],
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "localhost:8080/bcc_policy_token_gateway/checktoken/8e397dc9-695e-4e7c-b6ad-9002695618ab",
					"host": [
						"localhost"
					],
					"port": "8080",
					"path": [
						"bcc_policy_token_gateway",
						"checktoken",
						"8e397dc9-695e-4e7c-b6ad-9002695618ab"
					]
				}
			},
			"response": []
		},
		{
			"name": "Check policy Creation Token Fail",
			"event": [
				{
					"listen": "test",
					"script": {
						"id": "b9f8beee-7915-41a0-8e41-e9b9e6888da1",
						"exec": [
							"pm.test(\"Response\", function () {",
							"    pm.expect(pm.response.text()).to.include(\"Token is not valid\");",
							"});"
						],
						"type": "text/javascript"
					}
				}
			],
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "localhost:8080/bcc_policy_token_gateway/checktoken/Dota2",
					"host": [
						"localhost"
					],
					"port": "8080",
					"path": [
						"bcc_policy_token_gateway",
						"checktoken",
						"Dota2"
					]
				}
			},
			"response": []
		}
	]
}
```



        