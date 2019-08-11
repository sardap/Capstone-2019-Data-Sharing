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


const database = new NEDB("nedb.db");
database.loadDatabase();

/* setup server */
const SERVER = EXPRESS();
SERVER.listen(port, ()=> console.log("server listening on port: " + port));


/* default route - use to check status of server*/
SERVER.get("/", (request, response) =>{    
    response.json({
        status: "Success",
        msg: "api server online",
    });
});


SERVER.get("/bcc_policy_gateway/newtoken/:databrokerapikey", (request, response)=>{
    let data = {};
    data.time = Date.now();
    data.token = UUIDv4();
    data.broker_api_key = request.param("databrokerapikey");
    database.insert(data);

    response.json({
        status: "success",
        policy_creation_token: data.token
    });
});
