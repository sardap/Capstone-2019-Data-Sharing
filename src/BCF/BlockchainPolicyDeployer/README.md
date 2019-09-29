# Deployer Com

## Docker Info

### Building Image
`docker build . --rm -t blockchain_policy_deployer:dev BlockchainPolicyDeployer` or `docker-compose build`

### Startup Info
Requires the following environment variables be set 
* `VALIDATOR_IP` the IP Or domain name of the policy validator 
* `VALIDATOR_PORT` 
* `CHAIN_NAME` The multichain chain name
* `RPC_IP` multichain rpc ip
* `RPC_PORT`
* `RPC_USERNAME` multichain rpc username
* `RPC_PASSWORD` multichain rpc password

Example run check the docker compose file

## Accessing
Refer to polciy deploy command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

## Testing component

 Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

 1. Run Get address and enter that as the walled ID in the policy
 2. Run Test Post The Key is the Stream which the policy was deployed to
 3. Run RPC test Modify Change the RPC call to make sure the smart filter has been applied correctly. Replace the streamid with the key 

 Import the following json into postman for the collection.
```json
{
	"info": {
		"_postman_id": "9373f35d-1e76-40fb-ae72-404f0c525a03",
		"name": "Deployer",
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
			"name": "TestPost",
			"request": {
				"method": "POST",
				"header": [
					{
						"key": "",
						"value": "",
						"type": "text",
						"disabled": true
					},
					{
						"key": "wallet_id",
						"value": "",
						"type": "text",
						"disabled": true
					},
					{
						"key": "Content-Type",
						"name": "Content-Type",
						"value": "application/json",
						"type": "text"
					}
				],
				"body": {
					"mode": "raw",
					"raw": "{\n    \"json_policy\": \"{\\\"excluded_categories\\\":[0],\\\"min_price\\\":10,\\\"time_period\\\":{\\\"start\\\":-4785955200,\\\"end\\\":693705600},\\\"data_type\\\":\\\"heart rate\\\",\\\"wallet_ID\\\":\\\"{{address}}\\\",\\\"active\\\":[true, false],\\\"report_log\\\":[{\\\"data\\\":\\\"123\\\",\\\"hash\\\":\\\"321\\\"}]}\",\n    \"wallet_id\": \"{{address}}\"\n,\"broker_wallet_id\":\"{{broker_wallet_address}}}\"",
					"options": {
						"raw": {
							"language": "json"
						}
					}
				},
				"url": {
					"raw": "http://localhost:6080/blockchain_policy_deployer/deploy",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "6080",
					"path": [
						"blockchain_policy_deployer",
						"deploy"
					],
					"query": [
						{
							"key": "",
							"value": "test",
							"disabled": true
						},
						{
							"key": "wallet_id",
							"value": "test",
							"disabled": true
						}
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
}```

 ### Check Deploy
 Run the deploy function and verify the output matches what is specified in the components reference page.