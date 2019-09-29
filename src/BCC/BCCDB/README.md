# BCCDB Com

## license
This com is under GPLv2 refer to the license file in this dir

## Docker Info

### Building Image
No special build instructions

### Startup Info
Requires the following environment variables be set 
* `MYSQL_ROOT_PASSWORD`
* `MYSQL_PASSWORD`
* `MYSQL_USER`
* `MYSQL_DATABASE`
* `PUID` Set to 1000
* `PGID` Set to 1000

#### Inserting Default Data
***WARNING!!!*** This is a bit funky but Make due.
Each broker needs to be a "," separated with the following fields
* `BrokerName,DropOffLocation,WalletAddress`
Then Set the key to be `BROKER_(NUMBER)` starting at zero
Example: `BROKER_0=Chains,nothingtolose.biz,f4b71d3e759cc6490dee67e6ff80e729`
With the 2nd broker being 
Example: `BROKER_1=sing,doyouhear.people,6f1642b2416d3bc66589f0c6ff40bb18`

## Accessing
Can access via mysql 

## Testing component

1 exec into the container with
 ```
 docker exec -it CONTAINER bash
 ```
 <br/>2. log into the MYSQL cli and type in password
 ```
 mysql -h mysql -h IP -u USERNAME -P 3306 -p
 ```
 <br/>3. get into the main database
 ```
 use DATABASE_NAME;
 ```
 <br/>4. Run this command
 ```
 select * from Broker;
 ```
 You should see this
 ```
 +----+------------------+----------------+-----------------+
| ID | DataBrokerAPIKey | BrokerName     | DropOffLocation |
+----+------------------+----------------+-----------------+
|  1 | broker1          | It Just Works  | NULL            |
|  2 | broker2          | No Worries INC | NULL            |
|  3 | broker3          | NULL           | NULL            |
+----+------------------+----------------+-----------------+
 ```
 <br/>5. Test entries aren't re added. 
1. Make sure that you have a volume mounted at ```/config```
2. Run seps 2. 3. 4. again All results should be the same

<br/>6. Run steps on 2. 3. 4. again on a remote host
