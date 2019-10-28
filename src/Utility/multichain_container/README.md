# Multichain con

## license
This com is under GPL-3.0 refer to the license file in this dir

## Docker Info

### Building Image
No special build instructions

### Startup Info
the following environment variables **can** be set 
* `CHAIN_NAME`
* `RPC_PASSWORD`
* `RPC_USERNAME`
* `STREAM_NAME` If set will create this stream


## Accessing

You can access via rpc

## Testing

1. Start by moving into this dir and running ```docker-compose up```
2. Then enter the docker con by running ```docker exec -it multichain bash```
3. You can then access the multichain cli by running ```multichain-cli $CHAIN_NAME -rpcuser=$RPC_USERNAME -rpcport=25565 -rpcpassword=$RPC_PASSWORD (ANY COMMAND YOU WANT)```