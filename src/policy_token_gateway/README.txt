BCC Policy Token Creation Gateway

Written in NodeJS using libraries Express, UUID, and nedb
    - Express is used to provide HTTP support for our HTTP routes
    - NEDB is a small object database, using a subset of MongoDB APIs.  Making it suitable for a prototype which can then be moved to Mongo if it outgrows NEDB
    - UUID is being used to generate a UUID which is used as a token

Setup and run dependencies:
    - nodejs 
    - npm 

To run:
    - run `$npm install` to pull down the dependencies
    - run `$node index.js` to start the server

Environment variables: 
    - Software expects a PORT environment variable.  If no PORT variable exists it will default to 8080
    - The dockerfile exposes port 8080

Running in Docker:
    - Build with `$docker build -t name/app-name .`
    - For persistent storage mount a directory in the container with /usr/src/app/storage
        - eg `docker run -p 8080:8080 -v $(pwd)/storage:/usr/src/app/storage andrew/token-app` will mount ./storage into the container

Broker keys:
    - Becoming a broker isn't a technical process.  For the prototype the accepted broker API keys are ["broker0", "broker1", "broker2"]

Additional information:
    - The database will be stored in storage/nedb.db 
    - Routes:
        - Generate a new token:     /bcc_policy_token_gateway/newtoken/:brokerapikey
        - Validate existing token:  /bcc_policy_token_gateway/checktoken/:token
