#!/usr/bin/nodejs



//example data
//{"method":"publishfrom","params":["<from address / wallet id>","<stream1/stream_id">,"policy", <data>],"chain_name":"Mainchain"}
    //all calls are POSTS, the method is the method called on the chain


//import filesystem object
//import axios a http client package
const axios = require('axios');
const fs = require('fs');


//Make the HTTP post to the multichain RPC
function post_data(address, username, password, wallet_id, stream_id)
{
    axios.post("http://" + address, {
        "method":"publishfrom",
        "params":[
            wallet_id,
            stream_id,
            "policy", //policy is a key in a key value pair 
            {"json":{"active":[false]}}] //the new policy
        ,"chain_name":"chain1"}
        ,{
    auth: {
        username: username,
        password: password
      }
  }).then(function (response) {
    console.log(response.data);
  }).catch(function (error) {
    console.error(error);
  });
}

function help(message, code)
{
    if(!code){
        code = 1;
    }

    if(code == 0){
        out = console.log;
    } else {
        out = console.error;
    }
    
    if(message){
        out(message);
    }

    out("Usage:");
    out("\t$deactivator <address:port/path> <username> <password> <wallet_id> <streamid>");
    out("example:")
    out("\t$deactivator 127.0.0.1:3000/post some_username abc123 ABcDXyZ ffg222323323232");

    process.exit(code);
}

function main(){
    let args = process.argv.slice(1);
    
    //check for 
    if (!args[1]) {
        help("incorrect number of arguments", 1);
    } else if (args[1] == '-h' || args[1] == '-?'|| args[1] == "--help") {
        help(null, 0);
    }

    if(args.length != 6){
        help("incorrect number of arguments", 1);
    }

    //get the cli arguments
    //ideally some validation would be good
    let address = args[1];
    let username = args[2];
    let password = args[3];
    let wallet_id = args[4];
    let stream_id = args[5];

    post_data(address, username, password, wallet_id, stream_id);

}

main();



