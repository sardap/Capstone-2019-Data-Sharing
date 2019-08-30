FROM ubuntu:16.04

RUN apt-get update -y && \
    apt-get install -y python3-pip python3-dev

COPY ./requirements.txt /requirements.txt

WORKDIR /

RUN pip3 install -r requirements.txt

COPY . /

ENV DEPLOYER_IP=192.168.0.1:80
ENV FETCHER_IP=192.168.0.1:80
ENV POLICY_TOKEN_IP=192.168.0.1:80

EXPOSE 5000

ENTRYPOINT [ "python3" ]

CMD [ "app/startup.py" ]
