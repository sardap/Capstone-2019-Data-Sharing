import * as mssql from "mssql";
import dotenv from "dotenv";
import * as axios from "axios";

dotenv.config();
const { DB_NAME, DB_USER, DB_PASS, DB_HOST } = process.env;
const config = {
  server: DB_HOST,
  user: DB_USER,
  password: DB_PASS,
  database: DB_NAME
};

async function add_new_linking_entry_to_user(token, location) {
  let result = {};
  try {
    let pool = await mssql.connect(config);
    let query_result = await pool
      .request()
      .input("Token", mssql.VarChar(100), token)
      .query(
        `SELECT * FROM UserTokenLinkings WHERE PolicyCreationToken = @Token;`
      );
    let rows = query_result.recordset || [];
    if (rows.length == 0) {
      console.error(
        "Given token doesn't match any existing linking records..."
      );
      result = {
        success: "failure",
        msg: "Cannot find any matching data sharing policies..."
      };
    } else {
      let row_to_update = rows[rows.length - 1];

      await pool
        .request()
        .input("Id", mssql.UniqueIdentifier, row_to_update.Id)
        .input("Location", mssql.VarChar(100), location)
        .query(
          `UPDATE UserTokenLinkings SET PolicyBlockchainLocation = @Location WHERE Id = @Id;`
        );

      const response = await axios.post(
        process.env.BROKER_URL + "/api/VerifyPolicy/" + policy_creation_token
      );

      if (!response.data.success) throw "Unable to verify policy";

      result = {
        result: "success",
        msg: ""
      };
    }
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
