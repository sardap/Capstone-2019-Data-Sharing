import * as mssql from "mssql/msnodesqlv8";
import dotenv from "dotenv";
import uuidv4 from "uuid/v4";

dotenv.config();
const { DB_NAME, DB_USER, DB_PASS, DB_HOST } = process.env;
const config = {
  server: DB_HOST,
  user: DB_USER,
  password: DB_PASS,
  database: DB_NAME
};

async function add_new_linking_entry_to_user(user, token, location) {
  mssql
    .connect(config)
    .then(pool => {
      return pool
        .request()
        .input("Id", mssql.UniqueIdentifier, uuidv4())
        .input("UserId", mssql.UniqueIdentifier, user)
        .input("Token", mssql.VarChar, token)
        .input("Location", mssql.VarChar, location)
        .query(
          `INSERT INTO UserTokenLinkings (Id, UserId, PolicyCreationToken, PolicyBlockchainLocation) VALUES (@Id, @UserId, @Token, @Location);`
        );
    })
    .then(() => {
      return {
        result: "success",
        msg: ""
      };
    })
    .catch(err => {
      console.error(`${err}`);
      return {
        result: "failure",
        msg: `${err}`
      };
    })
    .then(result => {
      mssql.close();
      return result;
    });
}

export { add_new_linking_entry_to_user };
