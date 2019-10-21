/* Import packages */
const EXPRESS = require("express");
const NEDB = require("nedb");
const UUIDv4 = require('uuid/v4');
const MYSQL = require('mysql');
/* Get environment variables */

//get port
let port = process.env.PORT;

if(port == undefined){
    port = 8080;
    console.log("No $PORT specified - starting on default" + port);
}

let mysql_details = {
    MYSQL_PASSWORD: process.env.MYSQL_PASSWORD,
    USER: process.env.MYSQL_USER,
    HOST: process.env.MYSQL_HOST,
    DB: process.env.MYSQL_DATABASE
}; 

let broker_db_connection = MYSQL.createConnection({
    host     : mysql_details.HOST,
    user     : mysql_details.USER,
    password : mysql_details.MYSQL_PASSWORD,
    database : mysql_details.DB
  });
   


//To turn on debug information:
let debug = process.env.DEBUG;

if(debug == "1")
{
    debug = true;
    console.log("running in debug mode:  Available routes include http://IP:PORT/debug/mysql");
}


/* setup database / load existing database */
const database = new NEDB("storage/nedb.db");
database.loadDatabase();

/* setup http server */
const SERVER = EXPRESS();
SERVER.listen(port, ()=> console.log("server listening on port: " + port));


/* default route - use to check status of server*/
SERVER.get("/", (request, response) =>{    
    let data = {
        status: "Success",
        msg: "policy token creator gateway online",
    };

    if(debug){
        data.debug = "true";
    }
    
    response.json(data);
});


SERVER.get("/bcc_policy_token_gateway/newtoken/:brokerapikey", (request, response)=>{
    let broker_key = request.param("brokerapikey");
    
    broker_db_connection.query('SELECT * from Broker', function (error, results, fields) {
        if (error){
            response.status(500).json({
                msg: "error connecting to Broker database",
                error: error
            });
        } 
        else{
            //populate brokers array
            let brokers = [];
            for(let i = 0; i < results.length; i++){
                brokers.push(results[i].DataBrokerAPIKey);
            }
            
            //if the broker key does not exist then return invalid key
            if(!brokers.includes(broker_key)){
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
        }
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
                msg: "token valid",
                broker_api_key:  docs[0].broker_api_key
            });
        } else {
            response.status(401).send("Token is not valid");
        }
    });
});
    

// ///// DEBUG ROUTES /////
    
if(debug){

    //add a list of debug routes to print at /debug
    SERVER.get("/debug/", (request, response) =>{
        data = "Debug routes available <br>";
        data += "/debug/mysql    - Get mysql connection details <br>";
        data += "/debug/brokers  - Dump broker table <br>";
        response.send(data)
    });


    // /mysql debug route
    SERVER.get("/debug/mysql", (request, response) =>{    
        response.json({
            data: mysql_details
        });
    });

    // /brokers debug route to list all brokers
    SERVER.get("/debug/brokers", (request, response) =>{            
        broker_db_connection.query('SELECT * from Broker', function (error, results, fields) {
            if (error){
                response.json({
                    error: error
                });
            } 
            else{

                let data = "";
                for(let i = 0; i < results.length; i++){
                    data += results[i].DataBrokerAPIKey + "<br>";
                }
                response.send(data);
            }
          });

    });
}
    