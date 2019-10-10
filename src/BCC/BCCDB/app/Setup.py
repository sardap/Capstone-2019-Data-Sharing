#!/usr/bin/env python3
#This file is licensed under the GPLv2 - To get a copy visit https://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html
import os
import mysql.connector
import time

#docker build -t bccdb:latest .; docker stop mariadb; docker rm mariadb; docker run --name=mariadb -e PUID=1000 -e PGID=1000 -e MYSQL_ROOT_PASSWORD=mypass -e MYSQL_DATABASE=main -p 3306:3306 --restart unless-stopped bccdb:latest;
#docker build -t bccdb:latest .; 
#docker stop mariadb; 
#docker rm mariadb; 
#docker run --name=mariadb -e PUID=1000 -e PGID=1000 -e MYSQL_ROOT_PASSWORD=mypass -e MYSQL_DATABASE=main -p 3306:3306 --restart unless-stopped bccdb:latest;

def add_broker(cur, api_key, create_info):
	split = create_info.split(',')
	print(split)
	cur.execute("INSERT INTO Broker(DataBrokerAPIKey, BrokerName, DropOffLocation, WalletAddress) VALUES('{}', '{}', '{}', '{}');".format(api_key, split[0], split[1], split[2]))

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
		WalletAddress varchar(255), \
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

	broker_num = 0
	while(("BROKER_{}".format(broker_num) in os.environ) and (data is None)):
		add_broker(cur, "broker{}".format(broker_num), os.environ["BROKER_{}".format(broker_num)])
		broker_num += 1
	
	mydb.commit()
	mydb.close()

	#Work around so it doesn't leave exit signal which kills the containtor
	while(True):
		time.sleep(1)

main()