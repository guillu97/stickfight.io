var port = process.env.PORT || 3000
io = require('socket.io')(port)
var gameSocket = null

console.log('server started')

var players = new Map()
var numOfPlayers = 0
gameSocket = io.on('connection', function(socket){
    numOfPlayers += 1
    //var playerNum = numOfPlayers
    players.set(socket.id,{}) // the socket id is assign to a null object where we could store things : position...

    console.log('socket connected : ' + socket.id)
    console.log('num of players connected : ' + numOfPlayers)
    console.log(' ')
    console.log(players)
    console.log(' ')

    socket.emit('assignPlayerId',{playerId: socket.id}) // then the socket id to the game

    if(numOfPlayers > 1){
        playersCopy = []
        for (var [key, value] of players) {
            if(key != socket.id){
                playersCopy.push(Object.assign({playerId:key}, value))
            }
        }
        jsonObj = {Players:playersCopy}
        console.log('jsonObj')
        console.log(jsonObj)
        socket.emit('allPlayersPositions', jsonObj) // then the current position of all the players in the game, to spawn them locally
    }

    //socket.broadcast.emit('newPlayer')
    



    

    socket.on('disconnect', function(){
        console.log('socket disconnected : ' + socket.id)
        players.delete(socket.id)
        numOfPlayers -= 1
        console.log('num of players connected : ' + numOfPlayers)
        console.log(' ')
        console.log(players)
        console.log(' ')
        socket.broadcast.emit('playerDisconnected', {playerId: socket.id})
    })

    socket.on('test-event1', function(){
        console.log('got test-event1')
    })

    socket.on('test-event2', function(data){
        console.log('got test-event2')
        console.log(data)

        socket.emit('test-event', {
            test:12345,
            test2: 'test emit event'
        })
    })

    socket.on('test-event3', function(data, callback){
        console.log('got test-event3')
        console.log(data)

        callback({
            test: 123456,
            test2: "test3"
        })
    })

    socket.on('moveInput',function(data,callback){
        console.log('moveInput received')
        mov = {playerId: socket.id, moveInput: data.moveInput}
        socket.broadcast.emit('moveInput', mov)
    })

    socket.on('jump',function(data,callback){
        console.log('jump received')
        socket.broadcast.emit('jump', socket.id)
    })

    socket.on('playerMoved',function(data, callback){
        players.set(socket.id, {pos: data})
        console.log(players)
        console.log('player position received')
        pos = {playerId: socket.id, pos: data}
        socket.broadcast.emit('playerMoved', pos)
    })


})