import * as mssql from "mssql";
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
  let result = {};
  try {
    let pool = await mssql.connect(config);
    await pool
      .request()
      .input("Id", mssql.UniqueIdentifier, uuidv4())
      .input("UserId", mssql.UniqueIdentifier, user)
      .input("Token", mssql.VarChar(100), token)
      .input("Location", mssql.VarChar(100), location)
      .query(
        `INSERT INTO UserTokenLinkings (Id, UserId, PolicyCreationToken, PolicyBlockchainLocation) VALUES (@Id, @UserId, @Token, @Location);`
      );

    result = {
      result: "success",
      msg: ""
    };
  } catch (err) {
    console.trace(err);
    result = {
      result: "failure",
      msg: `${err}`
    };
  }
  mssql.close();
  return result;
}

export { add_new_linking_entry_to_user };
