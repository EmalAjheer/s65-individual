Docker helpfull commands:
- docker network ls (list of docker networks)
- docker ps (list of running images)
- docker images ls (list of all images)

Docker building/running: (https://markheath.net/post/docker-tooling-vs2019)
- docker build -t api-gateway .
- docker run -it -p 8082:80 --rm --name gateway api-gateway

- docker build -t tweet-service .
- docker run -it -p 8100:80 --rm --name tweetservice tweet-service


(azure ocelot docker: https://www.youtube.com/watch?v=U7I2Yli9NZw&ab_channel=FrankBoucherFrankBoucher)


Postgres & Docker:
- docker run --name postgres -e POSTGRES_PASSWORD=admin -d postgres (Aanmaken postgress docker container met naam en wachtwoord)
- psql -h localhost -p 5432 -U postgres (In container komen)
- \l (lijst met databases die gemaakt zijn)
- \c 'naamdb'; (connect to db)
- create table student(); 
- \dt; (list of relations/tables)
- \d student; (in table gaan)