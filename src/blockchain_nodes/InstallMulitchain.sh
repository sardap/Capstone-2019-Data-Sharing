#bin/bash

cd ~

sudo add-apt-repository -y ppa:bitcoin/bitcoin

sudo apt-get update
sudo apt install -y net-tools
sudo apt-get install -y software-properties-common
sudo apt-get install -y build-essential libtool autotools-dev automake pkg-config libssl-dev git python python-pip
sudo apt install -y git
sudo apt install -y docker

sudo apt install -y docker-compose
## For some reason this fixes a docker-compose issue no idea why
sudo curl -L "https://github.com/docker/compose/releases/download/1.24.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

sudo apt install -y build-essential
sudo apt-get install -y libdb4.8-dev libdb4.8++-dev
sudo pip install pathlib2
sudo apt-get install -y libboost1.65-all-dev
sudo apt-get install -y libdb++-dev

sudo apt upgrade -y

cd ~

git clone https://github.com/MultiChain/multichain.git
cd multichain
set MULTICHAIN_HOME=$(pwd)
mkdir v8build
cd v8build
wget https://github.com/MultiChain/multichain-binaries/raw/master/linux-v8.tar.gz
tar -xvzf linux-v8.tar.gz
cd ..
./autogen.sh
./configure --with-incompatible-bdb
make
cd src
sudo mv multichaind multichain-cli multichain-util /usr/local/bin