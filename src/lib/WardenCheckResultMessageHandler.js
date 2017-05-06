//WardenCheckMessageHandler.js

module.exports = function(io) {
  this.io = io;

  this.publishWardenCheckResultProcessed = (event) => {
    console.log('Publishing warden_check_result_processed message');
    this.io.sockets.emit('warden_check_result_processed', event);
  };
}