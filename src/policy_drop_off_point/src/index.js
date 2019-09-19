import express from "express";
import dotenv from "dotenv";
import * as db from "./database";

dotenv.config();

const APP = express();
APP.use(express.json());
APP.listen(process.env.PORT);

APP.post("/policy_drop_off_point/receivepolicy", (request, response) => {
  let {
    policy_creation_token,
    policy_blockchain_location,
    user_id
  } = request.body;

  db.add_new_linking_entry_to_user(
    user_id,
    policy_creation_token,
    policy_blockchain_location
  ).then(result => {
    console.table([result]);
    response.send(result);
  });
});

console.info("Listening for request...");
