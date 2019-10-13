# BCCDataCustodianSelection

# Docker Info

## license
This com is under GPLv2 refer to the license file in this dir

### Building Image
Change into this dir
`docker build --rm . -t BCCDataCustodianSelection:dev;`

### Startup Info
Refer to Docker compose file in this dir

#### Port
You will need to expose port **80** and **443**

## Accessing
Refer here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0
Refer to Postman tests as well

## Testing component

Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

Run in the following steps
1. Get Policy Creation Token
2. Update the Starting Request page request with this new token
3. Copy the request to google chrome
4. Go though each of the pages and select google fit if running locally (fitbit cannot work localy at the moment)
5. Select any of the data types then press submit
6. You should be taken to the google OAuth Page complete this
7. You should Pray and wait and see a page with the access token and a success message

**!!!NOTE!!!** Because the BCCDB is not persieneint locally if you restart the system the **YOU MUST** Remove the permissions our system from google here https://myaccount.google.com/u/0/permissions

Import the following json into Postman for the collection.

```json
{
	"info": {
		"_postman_id": "6c53a0bb-e67d-48b7-a8e3-07f59a462f72",
		"name": "BCCSelection",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "New Policy Creation Token Local",
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
			"name": "New Policy Creation Token Rancher",
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
			"name": "Starting Request to Page",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "lvh.me/{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"\",\"wallet_ID\":\"\",\"active\":[false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}/UPDATE WITH NEW TOKEN",
					"host": [
						"lvh",
						"me"
					],
					"path": [
						"{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"\",\"wallet_ID\":\"\",\"active\":[false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
						"UPDATE WITH NEW TOKEN"
					]
				}
			},
			"response": []
		},
		{
			"name": "Add Policy For testing",
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
					"raw": "{\"json_policy\":\"{\\\"excluded_categories\\\":[0],\\\"min_price\\\":10,\\\"time_period\\\":{\\\"start\\\":-4785955200,\\\"end\\\":693705600},\\\"data_type\\\":\\\"1\\\",\\\"wallet_ID\\\":\\\"asdasd\\\",\\\"active\\\":[false],\\\"report_log\\\":[{\\\"data\\\":\\\"123\\\",\\\"hash\\\":\\\"321\\\"}]}\",\"policy_creation_token\":\"37a33182-ca85-4c45-9d29-32883e531ac5\",\"wallet_id\":\"asdasd\",\"cust_type\":\"1\",\"data_type\":\"1\",\"api_key\":\"1%2FUnX53ZaYazmYvLYTinDLtW55r4TxAzQq3MaMy3O58pyPtkwDpB71VD2lobV3UYbL\"}\r\n"
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
		}
	],
	"protocolProfileBehavior": {}
}
```

