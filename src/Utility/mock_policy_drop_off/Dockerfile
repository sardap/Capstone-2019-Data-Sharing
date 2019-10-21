FROM ubuntu:16.04

RUN apt-get update -y 
RUN apt-get install -y python3-pip python3-dev

COPY ./requirements.txt /requirements.txt

WORKDIR /

RUN pip3 install -r requirements.txt

COPY . /

EXPOSE 5000

ENTRYPOINT [ "python3" ]

CMD [ "app/Startup.py" ]
