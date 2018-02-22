var io = require('socket.io')(process.envPort||3000);
var shortid = require('shortid');

console.log("Server Started");
//console.log(shortid.generate());

var players = [];

io.on('connection', function(socket){
    var thisPlayerId = shortid.generate();

    players.push(thisPlayerId);

    console.log('Client Connected spawning player id:', thisPlayerId);
    socket.broadcast.emit('spawn player', {id:thisPlayerId});

    players.forEach(function(playerId){
        if(playerId == thisPlayerId) return;

        socket.emit('spawn player', {id:playerId});
        console.log("Adding a player", playerId);
    });
        
    socket.on('move', function(data){
        data.id = thisPlayerId;
        console.log("Player position is: ",  JSON.stringify(data));
        socket.broadcast.emit('move', data);
    });

    socket.on('disconnect', function(){
        console.log("Player disconnected");
        players.splice(players.indexOf(thisPlayerId), 1);
        socket.broadcast.emit('disconnected', {id:thisPlayerId});
    });
});