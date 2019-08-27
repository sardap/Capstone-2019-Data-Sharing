# Deployer Com

## Docker Info

### Building Image
`docker build --rm -f "BlockchainPolicyDeployer\Dockerfile" -t blockchain_policy_deployer:dev BlockchainPolicyDeployer`

### Startup Info
Requires the following environment variables be set 
* `VALIDATOR_IP` the IP Or domain name of the policy validator 
* `VALIDATOR_PORT` 
* `CHAIN_NAME` The multichain chain name
* `STREAM_NAME` The multichain stream name to deploy policies onto 
* `RPC_IP` multichain rpc ip
* `RPC_PORT`
* `RPC_USERNAME` multichain rpc username
* `RPC_PASSWORD` multichain rpc password

Must open a port to 80

Example run `docker run -d -it -p 6000:80 -e VALIDATOR_IP=X.X.X.X -e VALIDATOR_PORT=5005 -e STREAM_NAME=stream1 -e CHAIN_NAME=chain1 -e RPC_PORT=25565 -e RPC_IP=X.X.X.X -e RPC_USERNAME=multichainrpc -e RPC_PASSWORD=workersunite --name dep blockchain_policy_deployer:dev bash`

Please Note: you also must be running the validator and a multichain instance which can be connected to with rpc

## Accessing
Refer to polciy deploy command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

### Example Body 

```json
{
	"json_policy" : "{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[true, false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
	"wallet_id" : "xxxxxxxxxxxxxxxxxx"
}
```

## Testing component

 Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

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
					"raw": "{\n    \"json_policy\": \"{\\\"excluded_categories\\\":[0],\\\"min_price\\\":10,\\\"time_period\\\":{\\\"start\\\":-4785955200,\\\"end\\\":693705600},\\\"data_type\\\":\\\"heart rate\\\",\\\"wallet_ID\\\":\\\"xxxxxxxxxxxxxxxxxx\\\",\\\"active\\\":[true, false],\\\"report_log\\\":[{\\\"data\\\":\\\"123\\\",\\\"hash\\\":\\\"321\\\"}]}\",\n    \"wallet_id\": \"xxxxxxxxxxxxxxxxxx\"\n}"
				},
				"url": {
					"raw": "http://localhost:6000/blockchain_policy_deployer/deploy",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "6000",
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
		}
	]
}```

 ### Check Deploy
 Run the deploy function and verify the output matches what is specified in the components reference page.