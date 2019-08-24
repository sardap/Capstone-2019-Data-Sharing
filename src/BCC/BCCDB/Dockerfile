FROM linuxserver/mariadb:latest

RUN apt-get update

RUN apt-get remove -y python3-pip
RUN apt-get install -y python3-dev
RUN apt-get install -y python3-pip

COPY ./requirements.txt /requirements.txt

RUN pip3 install -r requirements.txt

COPY . /

ENTRYPOINT ["/init"]

CMD [ "./app/Setup.py" ]
