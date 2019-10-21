# Policy Dropoff Point

The policy dropoff point serves as an endpoint operated by a data broker so once a policy has been validated, then deployed on the blockchain by the blockchain consortium the broker can then be notified of the location on the chain of the data subjects policy.  

## Pre-requisites and testing overview:  
Testing requires a database.  Thus far we've been using the Broker database in AWS.  
TESTING REQUIRES these environment variables set for docker-compose to use.  
      - DB_HOST=${DB_HOST}
      - DB_USER=${DB_USER}
      - DB_PASS=${DB_PASS}
      - DB_NAME=${DB_NAME}

In the root directory of the Google drive folder is a secrets/ directory.  There is a file AWS Database credentials.
Alternatively, set up a local database to test.  

Earlier in the policy creation process the broker should have received a policy creation token and mapped this to a registered user.  If wishing to test this component in isolation, the tester must edit the `UserTokenLinkings` table that maps a policy creation token to an internal id.  Generate a UUID a put in in a policy creation token column.  Then when testing the component use that same UUID.  The end result should be the table is updated with the location in the same row as the UUID.  

1. Generate a UUID, for example c24a3bdd-a3a3-40be-ab43-beec26e842a9 (but it must be unique and not already in the table)
2. Connect to the database and edit a row placing that uuid in the PolicyCreationToken column
3. Run the postman query using that UUID as the policy_creation_token variable.  The policy_blockchain_location can be anything for testing purposes
4. View the table and verify the location has been added to the row


Postman JSON  
````
{
	"info": {
		"_postman_id": "fd3ae1bb-19d9-4e18-b858-fbba900e183d",
		"name": "dropoff",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "Dropoff",
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
					"raw": "{\"policy_creation_token\": \"{{policy_creation_token}}\", \"policy_blockchain_location\": \"some location\"}"
				},
				"url": {
					"raw": "http://localhost:7080/policy_drop_off_point/receivepolicy",
					"protocol": "http",
					"host": [
						"localhost"
					],
					"port": "7080",
					"path": [
						"policy_drop_off_point",
						"receivepolicy"
					],
					"query": [
						{
							"key": "policy_creation_token",
							"value": "c24a3bdd-a3a3-40be-ab43-beec26e842a9",
							"disabled": true
						}
					]
				}
			},
			"response": []
		}
	]
}
````