import os
import subprocess
import socket

def main():
    os.system("multichain-util create $CHAIN_NAME")
    os.system("echo \"rpcallowip=0.0.0.0/0\" >> ~/.multichain/$CHAIN_NAME/multichain.conf")
    os.system("multichaind -rpcport=25565 -rpcpassword=$RPC_PASSWORD -rpcusername=$RPC_USERNAME $CHAIN_NAME")
        
main()
