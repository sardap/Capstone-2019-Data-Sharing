import { Connection, Request, TYPES } from "tedious";
import dotenv from "dotenv";

dotenv.config();
const { DB_NAME, DB_USER, DB_PASS, DB_HOST } = process.env;
let config = {
  server: DB_HOST,
  authentication: {
    type: "default",
    options: {
      userName: DB_USER,
      password: DB_PASS
    }
  },
  options: {
    database: DB_NAME
  }
};

function add_new_linking_entry_to_user(token, location, callback) {
  let connection = new Connection(config);

  connection.on("connect", function(error) {
    if (error) console.log(error);
    else {
      console.log(`Inserting new token...`);

      let request = new Request(
        `INSERT INTO ${DB_NAME}.UserTokenBlockchainLinks (Token, Location) OUTPUT INSERTED.Id VALUES (@Token, @Location);`,
        (error, rowCount) => callback(error, rowCount)
      );
      request.addParameter("Token", TYPES.UniqueIdentifier, token);
      request.addParameter("Location", TYPES.NVarChar, location);
      connection.execSql(request);
    }
  });
}

export { add_new_linking_entry_to_user };
