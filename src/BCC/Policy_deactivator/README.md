# Policy Deactivator 

The policy deactivator is a small cli utility so that a data subject can change a policies active state to false.   

The deactivator assumes the user is running a read-only node of the chain

it's usage is:
`$deactivator.js <address:port/path> <username> <password> <wallet_id> <streamid>`

For example: `index.js 127.0.0.1:7090 multichainrpc mypass 1XDM9zZz6eviBstLxi47n1Sy25iZZfYjfRwc1M WXvrKVce9gMYjTBV8Q7CVgtwzMfuXx3o`


## Testing information:

In order to test deactivation it requires deploying a policy to deactivate. 

Pre-requisites:
- Start all the other components with docker compose.  
  - NOTE: The environment variables DOCKER_HOST_IP, GOOGLE_API_CLIENT_ID and GOOGLE_API_CLIENT_SECRET must be set
- A Google API key

1. Run `RPC Get address` to get an address.  Take the `result` address and set your postman `address` variable to this.
2. Run `New Policy Creation Token` to get a policy creation token.  Set your postman `create_token` to this value
3. Deploy a policy.  You'll need to copy and paste a working Google API key.  Go to the body of the request and replace `GOOGLE API KEY HERE` with your API key.  Notice also the variables `create_token` and `address` this should be populated in the request with your postman environment variables.  
Run the request and the return should say success.  Check the CLI logs for a line like this:  
`policy_gateway         | [2019-10-08 03:33:13,482] INFO in startup: {"trans_id":"316147d5d47ccba14eba3b8ad9a80493948642c2004a41412e5f75671ed8accc", "key" : "WXvrKVce9gMYjTBV8Q7CVgtwzMfuXx3o"}`
Copy the `key` into the postman variable `streamid`
4. Run `Subscribe` to the `streamid`.  It should return everything as `null`.  This is required so that we can use another RPC call to get the current status of a policy
5. Run `Pull policy` this will pull the current state of the policy off the blockchain.  There might be multiple copies, look at the most bottom `active` field.  It should be set to true.
6. Run the policy decativator command as instructed above
7. Re-run `Pull policy` look at the bottom active field, it should now be false.  
8. [OPTIONAL] Run `Set Policy True`.  This will change the active field back to true.  Now re-run steps 5 --> 7 

