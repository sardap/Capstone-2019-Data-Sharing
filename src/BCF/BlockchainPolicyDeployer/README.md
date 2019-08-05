# Fetcher Com

## Docker Info

### Building Image
`docker build --rm -f "BlockchainPolicyDeployer\Dockerfile" -t blockchain_policy_deployer:dev BlockchainPolicyDeployer`

docker stop dep; docker rm dep; docker run  -d -it -p 6000:80 -e RPC_USERNAME=multichainrpc -e RPC_PASSWORD=7FkityQ9fVURtZHzSMCPDXfrj8hXTKApC2yCNDbxYSHC --name dep blockchain_policy_deployer:dev bash

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

Example run `docker run  -d -it -p 6000:80 -e VALIDATOR_IP=X.X.X.X -e VALIDATOR_PORT=5005 -e STREAM_NAME=stream1 -e CHAIN_NAME=chain1 -e RPC_PORT=25565 -e RPC_IP=X.X.X.X -e RPC_USERNAME=multichainrpc -e RPC_PASSWORD=workersunite --name dep blockchain_policy_deployer:dev bash`

## Accessing
Refer to polciy deploy command here https://docs.google.com/spreadsheets/d/1tx5qSRbAhjFloYm4dX-Mxn17BmzcYBLhHZqnTUNTeOo/edit#gid=0

#Example Body 

`
{
	"json_policy" : "{\"excluded_categories\":[0],\"min_price\":10,\"time_period\":{\"start\":-4785955200,\"end\":693705600},\"data_type\":\"heart rate\",\"wallet_ID\":\"xxxxxxxxxxxxxxxxxx\",\"active\":[true, false],\"report_log\":[{\"data\":\"123\",\"hash\":\"321\"}]}",
	"wallet_id" : "xxxxxxxxxxxxxxxxxx"
}
`