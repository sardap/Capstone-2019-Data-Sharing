#
# Ubuntu Dockerfile
#
# https://github.com/dockerfile/ubuntu
#

# Pull base image.
FROM ubuntu:18.04

# Install pre req
RUN sed -i 's/# \(.*multiverse$\)/\1/g' /etc/apt/sources.list && \
    apt-get update
RUN apt-get install -y build-essential
RUN apt-get install -y software-properties-common
RUN apt-get update

RUN add-apt-repository -y ppa:bitcoin/bitcoin && \
    apt-get update 

RUN apt-get install -y apt-utils
RUN apt-get install -y net-tools
RUN apt-get install -y build-essential
RUN apt-get install -y libtool
RUN apt-get install -y autotools-dev
RUN apt-get install -y automake
RUN apt-get install -y pkg-config
RUN apt-get install -y libssl-dev
RUN apt-get install -y git python
RUN apt-get install -y python-pip
RUN apt-get install -y libdb4.8-dev
RUN apt-get install -y libdb4.8++-dev
RUN apt-get install -y autoconf
RUN apt-get install -y g++
RUN apt-get install -y make
RUN apt-get install -y openssl 
RUN apt-get install -y libcurl4-openssl-dev 
RUN apt-get install -y autogen 
RUN apt-get install -y tar
RUN apt-get install -y wget
RUN apt-get install -y nano

#install boost
RUN add-apt-repository universe && \
    apt-get update 
RUN apt-get install -y libboost-all-dev 

# Install mulitchain
RUN apt-get install bsdmainutils
RUN cd $HOME && \
    git clone https://github.com/sardap/multichain && \
    cd multichain && \
    set MULTICHAIN_HOME=$(pwd) && \
    mkdir v8build && \
    cd v8build && \
    wget https://github.com/MultiChain/multichain-binaries/raw/master/linux-v8.tar.gz && \
    tar -xvzf linux-v8.tar.gz && \
    cd .. && \
    ./autogen.sh && \
    ./configure --with-incompatible-bdb && \
    make && \
    cd src && \
    mv multichaind multichain-cli multichain-util /usr/local/bin && \
    cd $HOME && \
    mkdir .multichain
#
# Set environment variables.
ENV HOME /root
ENV CHAIN_NAME chain1
ENV RPC_PASSWORD mypass
ENV RPC_USERNAME multichainrpc

COPY ./startup.py /app/startup.py

WORKDIR /

RUN mkdir /mnt/storage

ENTRYPOINT [ "python3" ]

CMD [ "app/startup.py" ]

