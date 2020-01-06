var port = process.env.PORT || 3000
io = require('socket.io')(port)
var gameSocket = null

console.log('server started')

var players = new Map()
var rooms = new Map()
// debug
//rooms.set('test2',['playerId'])
//rooms.set('test3',['playerId'])

const NUM_PLAYER_MAX = 4

var numOfPlayers = 0
gameSocket = io.on('connection', function(socket){



    // ### the connection of a player ###

    numOfPlayers += 1
    //var playerNum = numOfPlayers
    players.set(socket.id,{}) // the socket id is assign to a null object where we could store things : position...

    // debug
    //players.set(socket.id,{pos:'test',room:'test2'})
    //socket.join(players.get(socket.id).room)

    console.log('socket connected : ' + socket.id)
    console.log('num of players connected : ' + numOfPlayers)
    console.log(' ')
    console.log(players)
    console.log(' ')

    // if the player is not the first one then send the other player position
    if(numOfPlayers >= 1){
        playersCopy = []
        for (var [key, value] of players) {
            if(key != socket.id){
                playersCopy.push({playerId:key, pos:value.pos})
            }
        }
        jsonObj = {Players:playersCopy}

        // debug
        console.log('jsonObj')
        console.log(jsonObj)

        // debug
        //io.in(players.get(socket.id).room).emit('allPlayersPositions', jsonObj)

        socket.to(players.get(socket.id).room).emit('allPlayersPositions', jsonObj) // then the current position of all the players in the game, to spawn them locally
    }

    //socket.broadcast.emit('newPlayer')
    



    
    // ### the disconnection of a player ###
    // remove player from the rooms and players tabs

    socket.on('disconnect', function(){
        console.log('socket disconnected : ' + socket.id)
        
        console.log('players.get(socket.id)')
        console.log(players.get(socket.id))

        console.log('')
        console.log('players before disconnection')
        console.log(players)
        console.log('')

        console.log('players.get(socket.id)')
        console.log(players.get(socket.id))
        console.log('')

        // if the player is currently in a room
        if(players.get(socket.id) !== undefined && players.get(socket.id).hasOwnProperty('room')){

            
            // send a disconnected event to the other players in the room
            socket.to(players.get(socket.id).room).emit('playerDisconnected', {playerId: socket.id})

            console.log('debug in disconnection')
            console.log(rooms)

            console.log(rooms.get(players.get(socket.id).room))
            // remove the player from the room
            var index = rooms.get(players.get(socket.id).room).indexOf(socket.id);
            console.log(index);
            if (index !== -1) rooms.get(players.get(socket.id).room).splice(index, 1);

            // if the room becomes empty then remove it
            if(rooms.get(players.get(socket.id).room).length === 0){
                rooms.delete(players.get(socket.id).room)
            }
        }

        socket.leave(players.get(socket.id).room)
        

        // remove the player from the players Map
        players.delete(socket.id)
        // decrement player number
        numOfPlayers -= 1

        // debug
        console.log('num of players connected : ' + numOfPlayers)
        console.log(' ')
        console.log(players)
        console.log(' ')
        
    })





    // ### all the test events ### 

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



    // ### events for the menu ###

    socket.on('createRoom', function(data, callback){
        console.log('createRoom received')
        console.log(data)

        console.log('rooms before creation')
        console.log(rooms)

        var roomName = data.roomName
        // remove the '\r' at the end
        roomName = roomName.slice(0,roomName.length - 1)
        console.log('')
        console.log(roomName)
        console.log('')

        // search if the room name is available
        var available = true;
        for (const key of rooms.keys()) {
            console.log(key)
            if(key === roomName){
                available = false;
            }
        }
        if(available === true){
            rooms.set(roomName,[socket.id])
    
            if(players.get(socket.id) === undefined){
                players.set(socket.id,{room:roomName})
            }
            else{
                Object.assign(players.get(socket.id), {room:roomName})
            }

            socket.join(roomName)
            callback({
                roomAvailable: true
            })
        }
        else{
            callback({
                roomAvailable: false
            })
        }
        console.log('room availability')
        console.log(available)
        console.log('rooms after creation')
        console.log(rooms)
        
    })

    socket.on('joinRoom', function(data, callback){
        console.log('joinRoom received')
        console.log(data)

        roomName = data.roomName
        available = false;
        for (const key of rooms.keys()) {
            console.log(key)
            if(key === roomName){
                // check if there are already to much people
                if(rooms.get(roomName).length < NUM_PLAYER_MAX){
                    available = true;
                }
                if(rooms.get(roomName).length + 1 === NUM_PLAYER_MAX){
                    available = true
                    //TODO: start the game
                }
            }
        }
        if(available === true){
            // join room
            rooms.get(roomName).push(socket.id)
            console.log("debug in join")
            console.log(rooms.get(roomName))

            Object.assign(players.get(socket.id),{room:roomName})

            socket.join(roomName)

            // send callback available true
            callback({
                roomAvailable: true
            })
        }
        else{
            // send callback available false
            callback({
                roomAvailable: false
            })
        }

    })

    socket.on('refreshRoomsList', (data,callback) => {
        console.log('refreshRoomsList received')
        console.log(rooms)

        // expected result
        //const expectedResult = [{roomName:'name',numPlayers:3}, {...}, ...]
        var result = []
        for(var [key, value] of rooms){
            //console.log(key)
            //console.log(value.length)
            result.push({roomName:key,numPlayers:value.length})
        }
        JSONobj = {Rooms:result}
        console.log(JSONobj)
        callback(JSONobj)
    })


    // ### For the player movements ###

    socket.on('moveInput',function(data,callback){
        console.log('moveInput received')
        mov = {playerId: socket.id, moveInput: data.moveInput}
        socket.to(players.get(socket.id).room).emit('moveInput', mov)
    })

    socket.on('jump',function(data,callback){
        console.log('jump received')
        socket.to(players.get(socket.id).room).emit('jump', socket.id)
    })

    socket.on('playerMoved',function(data, callback){
        players.set(socket.id, Object.assign(players.get(socket.id),{pos: data}))
        console.log(players)
        console.log('player position received')
        pos = {playerId: socket.id, pos: data}
        socket.to(players.get(socket.id).room).emit('playerMoved', pos)
    })


})