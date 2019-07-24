# Fetcher Com

## Docker Info

### Building Image
`docker build --rm -f "Fetcher\Dockerfile" -t fetcher:latest Fetcher`

### Startup Info
Requires the following environment variables be set 
* `GOOGLE_API_CLIENT_ID`
* `GOOGLE_API_CLIENT_SECRET`

Example run `docker run --rm -d --name fetcher -p 80:80/tcp -p 443:443/tcp  -e GOOGLE_API_CLIENT_ID=xxxx -e GOOGLE_API_CLIENT_SECRET=xxxx fetcher:latest`

## Accessing
Refer to testfetch command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

### Specify Cust type
There 3 cust types
| Name      | number |
|-----------|---|
| Fake      | 0 |
| GoogleFit | 1 |
| Fitbit    | 2 |

Fake is purely for testing.

### Specify Data type
There 4 data types each maps to a number 
| Name      | Number | Supported By |
|-----------|--------|--------------|
| HeartRate | 0      |              |
| Height    | 1      | GoogleFit    |
| Foo       | 2      |              |
| Bar       | 3      |              |

### Google Fit
You must HTTP encode the refresh token before using it in the rest call.
