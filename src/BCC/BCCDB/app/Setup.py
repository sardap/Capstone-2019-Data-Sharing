#!/usr/bin/env python3
import os
import mysql.connector
import time

#docker build -t bccdb:latest .; docker stop mariadb; docker rm mariadb; docker run --name=mariadb -e PUID=1000 -e PGID=1000 -e MYSQL_ROOT_PASSWORD=mypass -e MYSQL_DATABASE=main -p 3306:3306 --restart unless-stopped bccdb:latest;
#docker build -t bccdb:latest .; 
#docker stop mariadb; 
#docker rm mariadb; 
#docker run --name=mariadb -e PUID=1000 -e PGID=1000 -e MYSQL_ROOT_PASSWORD=mypass -e MYSQL_DATABASE=main -p 3306:3306 --restart unless-stopped bccdb:latest;

def main():
	con = True
	while(con):
		try:
			mydb = mysql.connector.connect(
				host = "127.0.0.1",
				user = "root",
				passwd = os.environ['MYSQL_ROOT_PASSWORD'],
				port = 3306
			)
			cur = mydb.cursor(buffered=True)
			con = False
		except:
			time.sleep(1)

	database = os.environ['MYSQL_DATABASE']
	cur.execute("CREATE DATABASE IF NOT EXISTS " + database + ";")
	cur.execute("USE " + database + ";")
	cur.execute("CREATE TABLE IF NOT EXISTS Broker ( \
		ID int NOT NULL AUTO_INCREMENT, \
		DataBrokerAPIKey varchar(255) NOT NULL, \
		BrokerName varchar(255), \
		DropOffLocation varchar(1024), \
		PRIMARY KEY (ID) \
	) ENGINE = InnoDB;")
	cur.execute("CREATE TABLE IF NOT EXISTS Policy ( \
		OffChainPolicyID varchar(500) NOT NULL, \
		APIAddress varchar(255) NOT NULL, \
		DataCust Int NOT NULL,	\
		DataType Int NOT NULL,	\
		OnchainAddress varchar(255) NOT NULL, \
		DataBrokerID int NOT NULL, \
		PRIMARY KEY (OffChainPolicyID, DataBrokerID), \
		FOREIGN KEY (DataBrokerID) REFERENCES Broker(ID) \
	) ENGINE = InnoDB;")

	cur.execute("SELECT * FROM Broker;")
	data = cur.fetchone()

	if(('SET_DEFAULT_BROKER' in os.environ and os.environ['SET_DEFAULT_BROKER'].lower() == "true") and (data is None)):
		cur.execute("INSERT INTO Broker(DataBrokerAPIKey, BrokerName) VALUES('broker1', 'It Just Works');")
		cur.execute("INSERT INTO Broker(DataBrokerAPIKey, BrokerName) VALUES('broker2', 'No Worries INC');")
		cur.execute("INSERT INTO Broker(DataBrokerAPIKey) VALUES('broker3');")
		cur.close()
	
	mydb.commit()
	mydb.close()

	#Work around so it doesn't leave exit signal which kills the containtor
	while(True):
		time.sleep(1)

main()