import os
import subprocess
import socket
import subprocess
import time

def main():
    os.system("multichain-util create $CHAIN_NAME")
    os.system("echo \"rpcallowip=0.0.0.0/0\" >> ~/.multichain/$CHAIN_NAME/multichain.conf")

    p = subprocess.Popen("multichaind {} -daemon -rpcport=25565 -rpcpassword={} -rpcusername={}".format(os.environ["CHAIN_NAME"], os.environ["RPC_PASSWORD"], os.environ["RPC_USERNAME"]), shell=True)

    #Workaround it's a prototype 
    time.sleep(5)

    if("STREAM_NAME" in os.environ):
       subprocess.run("multichain-cli $CHAIN_NAME -rpcuser=$RPC_USERNAME -rpcport=25565 -rpcpassword=$RPC_PASSWORD create stream {} false".format(os.environ["STREAM_NAME"]), shell=True)

    while(True):
        time.sleep(1)
    
main()
