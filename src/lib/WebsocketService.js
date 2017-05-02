//WebsocketService.js
var express = require('express');
var http = require('http');
var socketio = require('socket.io');
var amqp = require('amqp');
var RabbitMqConnection = require('./RabbitMqConnection.js');
var OperationMessageHandler = require('./OperationMessageHandler.js');
var WardenCheckMessageHandler = require('./WardenCheckMessageHandler.js');


module.exports = function(configuration) {
  this.configuration = configuration;
  var port = this.configuration.port || 15000;
  var rabbitMqConfig = this.configuration.rabbitMq;

  this.run = () => {
    console.log('Starting Warden Websockets Service...');
    var app = express();
    var server = http.createServer(app);
    var io = socketio(server);

    var operationMessageHandler = new OperationMessageHandler(io);
    var wardenCheckMessageHandler = new WardenCheckMessageHandler(io);
    var rmqConnection = new RabbitMqConnection(rabbitMqConfig);

    rmqConnection.subscribe('warden.messages.events.operations',
      'operationupdated.#',
      operationMessageHandler.publishOperationUpdated);
      

    rmqConnection.connect();

    function onSocketConnection(socket) {
      console.log('connected');
      socket.on('initialize', (data) => {
        console.log('initialized');
      });
    }

    io.on('connection', onSocketConnection);

    server.listen(port, () => {
      console.log('Server listening at port %d', port);
    });

    app.get('/', (req, res) => {
      res.send('Warden Websockets Service');
    });
  }
};