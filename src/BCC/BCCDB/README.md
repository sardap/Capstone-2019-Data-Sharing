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
* `SET_DEFAULT_BROKER` Will add 3 default test broker api keys

## Accessing

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
