#This file is licensed under the GPLv2 - To get a copy visit https://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html

FROM linuxserver/mariadb:latest

RUN apt-get update

RUN apt-get remove -y python3-pip
RUN apt-get install -y python3-dev
RUN apt-get install -y python3-pip

COPY ./requirements.txt /requirements.txt

RUN pip3 install -r requirements.txt

COPY . /
RUN chmod 777 /app -R

ENTRYPOINT ["/init"]

CMD [ "./app/Setup.py" ]
