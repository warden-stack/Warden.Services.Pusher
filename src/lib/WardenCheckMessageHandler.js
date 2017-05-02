//WardenCheckMessageHandler.js

module.exports = function(io) {
  this.io = io;

  this.publishRemarkResolved = (event) => {
    let message = {
    };
    console.log('Publishing warden_check_created message');
    this.io.sockets.emit('warden_check_created', message);
  };
}