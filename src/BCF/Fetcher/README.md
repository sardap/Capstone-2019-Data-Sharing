# Fetcher Com

## Docker Info

### Building Image
`docker build --rm -f "Fetcher\Dockerfile" -t fetcher:latest Fetcher`

### Startup Info
Requires the following environment variables be set 
* `GOOGLE_API_CLIENT_ID`
* `GOOGLE_API_CLIENT_SECRET`
* `FITBIT_API_CLIENT_ID`
* `FITBIT_API_CLIENT_SECRET`

You will need to get these values from the google api console

Example run `docker run --rm -d --name fetcher -p 80:80/tcp -p 443:443/tcp  -e GOOGLE_API_CLIENT_ID=xxxx -e GOOGLE_API_CLIENT_SECRET=xxxx fetcher:latest`

## Accessing
Refer to `testfetch` command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

### Specify Custodian type

There are 3 custodian types.

| Name      | Number |
| --------- | ------ |
| Fake      | 0      |
| GoogleFit | 1      |
| Fitbit    | 2      |

**Note:** Fake is purely for testing.

### Specify Data type

There are 4 data types; each maps to a number.

| Name      | Number | Supported By |
| --------- | ------ | ------------ |
| HeartRate | 0      | Fitbit       |
| Height    | 1      | GoogleFit    |
| Foo       | 2      |              |
| Bar       | 3      |              |

### Google Fit
You must HTTP encode the refresh token before using it in the rest call.

## Testing component

Refer to https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

Import the following json into Postman for the collection.

```json
{
	"info": {
		"_postman_id": "38e68a76-a705-4973-b495-6d8da5645c0e",
		"name": "Fetcher",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "Test Fetch Google",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "localhost:80/fetcher/testfetch/GOOGLE_REFRESH_TOKEN/1/1",
					"host": [
						"localhost"
					],
					"port": "80",
					"path": [
						"fetcher",
						"testfetch",
						"GOOGLE_REFRESH_TOKEN",
						"1",
						"1"
					]
				}
			},
			"response": []
		},
		{
			"name": "Test Fetch Fitbit",
			"event": [
				{
					"listen": "test",
					"script": {
						"id": "3c8b8972-72e6-402a-946e-b69172d6f19b",
						"exec": [
							"pm.test(\"Valid heart rate data returned\", function () {",
							"    var jsonData = pm.response.json();",
							"    pm.expect(jsonData.Result).to.eql(true);",
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
					"raw": "localhost:80/fetcher/testfetch/FITBIT_REFRESH_TOKEN/2/0",
					"host": [
						"localhost"
					],
					"port": "80",
					"path": [
						"fetcher",
						"testfetch",
						"FITBIT_REFRESH_TOKEN",
						"2",
						"0"
					]
				}
			},
			"response": []
		}
	]
}
```

### Test Fetch
Run the test fetch function and verify the output matches what is specified in the components reference page.