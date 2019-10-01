//what needs to be included to deactivate a policy
//send command to publish new state to the stream
//we need
    //address of the blockchain
    //need a wallet address
        //each policy has its own stream
    //need a location
    //json representing the new state
    //{active: "false"}
    //assumption is the append will change the policy to only {active: "false"}

    //Assuming we need wallet ID, where is it in the RPC Post?
    //The user is running a node, how do they get the streamid/location

//example data
//{"method":"publishfrom","params":["<from address / wallet id>","<stream1/location">,"policy", <data>],"chain_name":"Mainchain"}
    //all calls are POSTS, the method is the method called on the chain
const axios = require('axios');


function post_data()
{
    axios.post('http://localhost:3000/post', {
        "method":"publishfrom",
        "params":[
            "<from address / wallet id>",
            "<stream1/location>","policy", 
            "<data>"]
        ,"chain_name":"Mainchain"}
        ,{
    auth: {
        username: "some_username",
        password: "some_password"
      }
  }).then(function (response) {
    console.log(response.config.data);
  }).catch(function (error) {
    console.log(error);
  });
}

function main(){
    post_data();
}

main();



