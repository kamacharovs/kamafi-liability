# Overview

Kamacharov Finance liability microservice

[![build](https://github.com/kamacharovs/aiof-liability/actions/workflows/build.yml/badge.svg)](https://github.com/kamacharovs/aiof-liability/actions/workflows/build.yml)

## How to run it

The recommended way is to use `docker-compose`. That pulls down all the Docker images needed and you will have the full microservices architecture locally in order to get authentication from `aiof-auth` and data from `aiof-data`

### Docker

First we need to run `dotnet publish`

```pw
dotnet publish -c Release -o app/publish/kamafi.liability.core
```

Then we build the `Dockerfile`

```pw
docker build -t gkama/kamafi-liability:dev -f Dockerfile .
```

(Optiona) Run the image

```pw
docker run -it -e ASPNETCORE_ENVIRONMENT='Development' --rm -p 8000:80 gkama/kamafi-liability:dev
```

(Optional) Push to Docker hub

```pw
docker push gkama/kamafi-liability:dev
```

(Optional) Clean up `none` images

```pw
docker rmi $(docker images -f “dangling=true” -q)
```
