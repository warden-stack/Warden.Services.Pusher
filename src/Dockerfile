FROM node:latest
COPY . /app
WORKDIR /app

RUN ["npm", "install"]
 
EXPOSE 15000/tcp
ENV NODE_ENV docker
 
ENTRYPOINT ["npm", "start"]
