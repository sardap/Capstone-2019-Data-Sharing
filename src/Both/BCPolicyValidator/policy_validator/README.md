#Policy Validator Com

## Docker Info

### Building Image
`docker build --rm -f "src\BCF\BCPolicyValidator\policy_validator\Dockerfile" -t policy_validator:latest src\BCF\BCPolicyValidator\policy_validator`

### Startup Info
Requires **no** environment variables to be set 

Example run `docker run --name valid -p 5001:80 policy_validator:latest`

## Accessing
Refer to checkjson and checkjsonpart commands here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

### Specify JsonPolicy
Below is the example policy 
```json
 {"excluded_categories":[0],"min_price":10,"time_period":{"start":-4785955200,"end":693705600},"data_type":"heart rate","wallet_ID":"xxxxxxxxxxxxxxxxxx","active":[false],"report_log":[{"data":"123","hash":"321"}]}
 ```

## Testing component

 Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

 Import the following json into postman for the collection.
```json
{
	"info": {
		"_postman_id": "b7991cca-e803-4a9f-bfb8-e242e052d302",
		"name": "PolicyVaildator",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "TestCheckJson_Valid",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "http://localhost:5005/checkjson/{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5005",
					"path": [
						"checkjson",
						"{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}"
					]
				},
				"description": "asd"
			},
			"response": []
		},
		{
			"name": "TestCheckJson_invalid",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "http://localhost:5005/checkjson/{\"excluded_categories\":\"poop\",\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5005",
					"path": [
						"checkjson",
						"{\"excluded_categories\":\"poop\",\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}"
					]
				},
				"description": "asd"
			},
			"response": []
		},
		{
			"name": "TestCheckJsonPart_Valid",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "http://localhost:5005/checkjsonpart/{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "5005",
					"path": [
						"checkjsonpart",
						"{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}"
					]
				},
				"description": "asd"
			},
			"response": []
		},
		{
			"name": "TestCheckJsonPart_invalid",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": ""
				},
				"description": "asd"
			},
			"response": []
		}
	]
} 
```

 ### Check Json Part
 Run the Check Json Part function and verify the output matches what is specified in the components reference page.

 ### Check Json
 Run the Check Json function and verify the output matches what is specified in the components reference page.