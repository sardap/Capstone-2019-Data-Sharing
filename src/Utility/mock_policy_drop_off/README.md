# BCCDB Com

This component is a mock of the policy dropoff point.  
The policy dropoff is an endpoint for the BCC to send the location of a policy on the blockchain.  
This dropoff is indended to act as an end point that will take in data and return a success.  
This allows testing of components without the need to instantiate a real drop off point and its database.  

## Docker Info

### Building Image
No special build instructions

### Startup Info
Requires No env vars

## Testing component

It's a mock none needed
```json
{
	"info": {
		"_postman_id": "2e774b09-0552-42a0-ab0f-22710ab69fb5",
		"name": "Mock Drop Off",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "receivepolicy",
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
					"raw": "{\n\t\"policy_creation_token\" : \"das\",\n\t\"policy_blockchain_location\" : \"kaptal\"\n}"
				},
				"url": {
					"raw": "http://localhost:6010/policy_drop_off_point/receivepolicy",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "6010",
					"path": [
						"policy_drop_off_point",
						"receivepolicy"
					]
				}
			},
			"response": []
		}
	]
}
```