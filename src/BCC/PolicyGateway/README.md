# Policy Gateway Com

## license
This com is under GPLv2 refer to the license file in this dir

## Docker Info

### Building Image
Change into this dir
`docker build --rm . -t policygateway:latest;`

### Startup Info
Requires the following environment variables be set 
* `DEPLOYER_IP` The full address for the policy deployer
* `FETCHER_IP` The full address for the fetcher
* `POLICY_TOKEN_IP` The full address for the policy token creator
* `MYSQL_USERNAME`
* `MYSQL_USER_PASSWORD`
* `MYSQL_PORT`
* `MYSQL_IP` 

Refer to the docker compose file for an example startup


#### Port
You will need to expose port **5000**

## Accessing
Refer to `addpolicy` command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

## Testing component

Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

Run in the following steps
1. RPC Get address copy the First entry in the result array and set your postmans env var address to it
2. Run New Policy Creation Token Get the token and set the postmans env var create_token to it
3. Run Add Policy With the Google API Key Replaced and wait and pray. You should see ```{ result : "success" } ``` then check the log and find the Policy_gateway message which has the id : "SOMTHING" and key "SOMTHING" copy the key this is the streamid set the postman env var of streamid to this value
4. RPC test Modify Run this should have no errors then change the active word to say something else this should fail  

Import the following json into Postman for the collection.

```json
{
	"info": {
		"_postman_id": "bb437a01-b8e2-4bfb-aed1-6aacba6df254",
		"name": "PolicyGateway",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "RPC Get Address",
			"request": {
				"auth": {
					"type": "basic",
					"basic": [
						{
							"key": "password",
							"value": "mypass",
							"type": "string"
						},
						{
							"key": "username",
							"value": "multichainrpc",
							"type": "string"
						}
					]
				},
				"method": "POST",
				"header": [
					{
						"key": "Content-Type",
						"name": "Content-Type",
						"type": "text",
						"value": "application/json"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\"method\":\"getaddresses\",\"params\":[],\"id\":\"98580176-1569033970\",\"chain_name\":\"chain1\"}",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "localhost:7090",
					"host": [
						"localhost"
					],
					"port": "7090"
				}
			},
			"response": []
		},
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
					"raw": "http://localhost:5010/bcc_policy_token_gateway/newtoken/broker1",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5010",
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
			"name": "Add Policy",
			"event": [
				{
					"listen": "test",
					"script": {
						"id": "2f6c872f-39a1-4be8-a140-7eca95c587be",
						"exec": [
							"pm.test(\"Your test name\", function () {",
							"    var jsonData = pm.response.json();",
							"    pm.expect(jsonData.result).to.eql(\"success\");",
							"});",
							""
						],
						"type": "text/javascript"
					}
				}
			],
			"request": {
				"method": "POST",
				"header": [
					{
						"key": "Content-Type",
						"name": "Content-Type",
						"value": "application/json",
						"type": "text"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\n\t\"json_policy\" : \"{\\\"excluded_categories\\\":[0],\\\"min_price\\\":10,\\\"time_period\\\":{\\\"start\\\":-4785955200,\\\"end\\\":693705600},\\\"data_type\\\":\\\"heart rate\\\",\\\"wallet_ID\\\":\\\"{{address}}\\\",\\\"active\\\":[true, false],\\\"report_log\\\":[{\\\"data\\\":\\\"123\\\",\\\"hash\\\":\\\"321\\\"}]}\",\n\t\"policy_creation_token\" : \"{{create_token}}\",\n\t\"wallet_id\" : \"{{address}}\",\n    \"cust_type\" : \"1\",\n    \"data_type\" : \"1\",\n\t\"api_key\" : \"GOOGLE API KEY HERE\",\n\t\"broker_id\" : 1\n} "
				},
				"url": {
					"raw": "http://localhost:6010/addpolicy",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "6010",
					"path": [
						"addpolicy"
					]
				}
			},
			"response": []
		},
		{
			"name": "RPC Test Modify",
			"request": {
				"auth": {
					"type": "basic",
					"basic": [
						{
							"key": "password",
							"value": "mypass",
							"type": "string"
						},
						{
							"key": "username",
							"value": "multichainrpc",
							"type": "string"
						}
					]
				},
				"method": "POST",
				"header": [
					{
						"key": "Content-Type",
						"name": "Content-Type",
						"type": "text",
						"value": "application/json"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\"method\":\"publish\",\"params\":[\"{{streamid}}\",\"policy\",{\"json\":{\"active\":[true]}}],\"chain_name\":\"chain1\"}",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "localhost:7090",
					"host": [
						"localhost"
					],
					"port": "7090"
				}
			},
			"response": []
		}
	]
}
```

