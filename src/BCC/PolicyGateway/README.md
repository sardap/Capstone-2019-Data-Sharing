# Policy Gateway Com

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

#### Port
You will need to expose port **5000**

## Accessing
Refer to `addpolicy` command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0


### Example Body
``` json
{
	"json_policy" : "{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[true, false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
	"policy_creation_token" : "8e397dc9-695e-4e7c-b6ad-9002695618ab",
    "wallet_id" : "xxxxxxxxxxxxxxxxxx",
    "cust_type" : "1",
    "data_type" : "1",
	"api_key" : "APIKEYHERE",
	"broker_id" : 1
}
```
## Testing component

Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

Import the following json into Postman for the collection.

```json
{
{
	"info": {
		"name": "PolicyGateway",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
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
					"raw": "{\n\t\"json_policy\" : \"{\\\"excluded_categories\\\":[0],\\\"min_price\\\":10,\\\"time_period\\\":{\\\"start\\\":-4785955200,\\\"end\\\":693705600},\\\"data_type\\\":\\\"heart rate\\\",\\\"wallet_ID\\\":\\\"xxxxxxxxxxxxxxxxxx\\\",\\\"active\\\":[true, false],\\\"report_log\\\":[{\\\"data\\\":\\\"123\\\",\\\"hash\\\":\\\"321\\\"}]}\",\n\t\"policy_creation_token\" : \"XXXXXX\",\n\t\"wallet_id\" : \"xxxxxxxxxxxxxxxxxx\",\n    \"cust_type\" : \"1\",\n    \"data_type\" : \"1\",\n\t\"api_key\" : \"Dota2\",\n\t\"broker_id\" : 1\n}"
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
	]
}
}
```

