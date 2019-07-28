#bin/bash
cd ~
sudo add-apt-repository -y ppa:bitcoin/bitcoin
sudo apt-get update
sudo apt install -y net-tools software-properties-common build-essential libtool autotools-dev automake pkg-config libssl-dev git python python-pip git libdb4.8-dev libdb4.8++-dev libboost1.65-all-dev autoconf g++ make openssl libcurl4-openssl-dev autogen libsasl2-dev
sudo pip install pathlib2
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
