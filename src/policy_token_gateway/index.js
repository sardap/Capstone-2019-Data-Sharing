/* Import packages */
const EXPRESS = require("express");
const NEDB = require("nedb");
const UUIDv4 = require('uuid/v4');

/* Get environment variables */
let port = process.env.PORT;
if(port == undefined){
    port = 8080;
    console.log("No $PORT specified - starting on default" + port);
}

/* setup database / load existing database */
const database = new NEDB("storage/nedb.db");
database.loadDatabase();

/* setup http server */
const SERVER = EXPRESS();
SERVER.listen(port, ()=> console.log("server listening on port: " + port));


/* default route - use to check status of server*/
SERVER.get("/", (request, response) =>{    
    response.json({
        status: "Success",
        msg: "policy token creator gateway online",
    });
});


SERVER.get("/bcc_policy_token_gateway/newtoken/:brokerapikey", (request, response)=>{
    let broker_key = request.param("brokerapikey");

    if(!is_valid_broker_api_key(broker_key)){
        response.status(400).send("Broker API key invalid");
        return;
    }
    
    let data = {};
    data.time = Date.now();
    data.token = UUIDv4();
    data.broker_api_key = broker_key;
    database.insert(data);

    response.json({
        status: "success",
        policy_creation_token: data.token
    });
});

/* 
 * Route takes token as input 
 * If the token is in the database then it is valid
 * return validity 
 */
SERVER.get("/bcc_policy_token_gateway/checktoken/:token", (request, response) =>{
    let token = request.param("token");
    database.find({ token: token }, (err, docs) => {
        /* if the length of the returned data is 1
         * Then the token must be in the database
         * then return a success */
        if(docs.length == 1){
            response.json({
                status: "success",
                msg: "token valid"
            });
        } else {
            response.status(401).send("Token is not valid");
        }
      });
});




/* 
 * Becoming a broker is bureaucratic process.  
 * For the prototype this component these brokers 
 * Final product would query a database of brokers stored within the BCC
 */
const BROKERS = ["broker0", "broker1", "broker2"];
function is_valid_broker_api_key(key){
    return BROKERS.includes(key);
}

