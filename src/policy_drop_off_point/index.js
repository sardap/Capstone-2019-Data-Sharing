import express from "express";
import dotenv from "dotenv";
import * as db from "./database";

dotenv.config();

const APP = express();
APP.use(express.json());
APP.listen(process.env.PORT);

APP.post("/policy_drop_off_point/receivepolicy", (request, response) => {
  let { policy_creation_token, policy_blockchain_location } = request.body;
  console.log(`${policy_creation_token}\n${policy_blockchain_location}`)
  db.add_new_linking_entry_to_user(
    policy_creation_token,
    policy_blockchain_location,
    function(error, rowCount) {
      if (error) console.log(error);
      response.send({ result: !error ? "success" : "failure", msg: error });
    }
  );
});
