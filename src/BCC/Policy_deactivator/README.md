# Policy Deactivator 

The policy deactivator is a small cli utility so that a data subject can change a policies active state to false.   

The deactivator assumes the user is running a read-only node of the chain

it's usage is:
`$deactivator.js <address:port/path> <username> <password> <wallet_id> <streamid>`

For example: `index.js 127.0.0.1:7090 multichainrpc mypass 1XDM9zZz6eviBstLxi47n1Sy25iZZfYjfRwc1M WXvrKVce9gMYjTBV8Q7CVgtwzMfuXx3o`


## Testing information:

In order to test deactivation it requires deploying a policy to deactivate. 

Pre-requisites:
- Start all the other components with docker compose.  
  - NOTE: The environment variables DOCKER_HOST_IP, GOOGLE_API_CLIENT_ID and GOOGLE_API_CLIENT_SECRET must be set
- A Google API key

1. Run `RPC Get address` to get an address.  Take the `result` address and set your postman `address` variable to this.
2. Run `New Policy Creation Token` to get a policy creation token.  Set your postman `create_token` to this value
3. Deploy a policy.  You'll need to copy and paste a working Google API key.  Go to the body of the request and replace `GOOGLE API KEY HERE` with your API key.  Notice also the variables `create_token` and `address` this should be populated in the request with your postman environment variables.  
Run the request and the return should say success.  Check the CLI logs for a line like this:  
`policy_gateway         | [2019-10-08 03:33:13,482] INFO in startup: {"trans_id":"316147d5d47ccba14eba3b8ad9a80493948642c2004a41412e5f75671ed8accc", "key" : "WXvrKVce9gMYjTBV8Q7CVgtwzMfuXx3o"}`
Copy the `key` into the postman variable `streamid`
4. Run `Subscribe` to the `streamid`.  It should return everything as `null`.  This is required so that we can use another RPC call to get the current status of a policy
5. Run `Pull policy` this will pull the current state of the policy off the blockchain.  There might be multiple copies, look at the most bottom `active` field.  It should be set to true.
6. Run the policy decativator command as instructed above
7. Re-run `Pull policy` look at the bottom active field, it should now be false.  
8. [OPTIONAL] Run `Set Policy True`.  This will change the active field back to true.  Now re-run steps 5 --> 7 

Postman JSON
````
{
	"info": {
		"_postman_id": "2ac48821-80e5-4c1b-ad99-ec9b4e2e0f6e",
		"name": "deactivator",
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
					"raw": "{\"method\":\"getaddresses\",\"params\":[],\"id\":\"98580176-1569033970\",\"chain_name\":\"chain1\"}"
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
				"body": {
					"mode": "raw",
					"raw": ""
				},
				"url": {
					"raw": "http://localhost:5010/bcc_policy_token_gateway/newtoken/broker0",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5010",
					"path": [
						"bcc_policy_token_gateway",
						"newtoken",
						"broker0"
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
			"name": "Subscribe",
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
					"raw": "{\"method\":\"subscribe\",\"params\":[\"WXvrKVce9gMYjTBV8Q7CVgtwzMfuXx3o\"],\"chain_name\":\"chain1\"}"
				},
				"url": {
					"raw": "localhost:7090",
					"host": [
						"localhost"
					],
					"port": "7090"
				},
				"description": "Request that the node subscribe to a streamid and cache its information.  This makes pulling that information with RPC work"
			},
			"response": []
		},
		{
			"name": "Pull policy",
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
						"value": "application/json",
						"type": "text"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\"method\":\"liststreamitems\",\"params\":[\"{{streamid}}\"],\"chain_name\":\"chain1\"}"
				},
				"url": {
					"raw": "localhost:7090",
					"host": [
						"localhost"
					],
					"port": "7090"
				},
				"description": "A request to get a policy"
			},
			"response": []
		},
		{
			"name": "Set Policy True",
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
					"raw": "{\"method\":\"publishfrom\",\"params\":[\"{{address}}\", \"{{streamid}}\",\"policy\",{\"json\":{\"active\":[true]}}],\"chain_name\":\"chain1\"}"
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
````

