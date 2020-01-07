var port = process.env.PORT || 3000
var io = require('socket.io')(port)
var gameSocket = null

const mongoose = require('mongoose')
const crypto = require('crypto')
const Schema = mongoose.Schema
mongoose.connect('mongodb://localhost:27017/stickfighter', { useNewUrlParser: true, useUnifiedTopology: true })
var EventSchema = new Schema({
  username: {
    type: String,
    required: true
  },
  password: {
    type: String,
    reuired: true
  },
  winsCount: {
    type: Number,
    default: 0
  }
})
const Event = mongoose.model('users', EventSchema)

async function saveUser (newUser, password) {
  var result = false
  var test = await Event.findOne({ username: newUser }).exec()
  if (test === null) {
    result = true
    var hash = crypto.createHash('sha256').update(password).digest('base64');
    const reg = new Event({
      username: newUser,
      password: hash
    })

    await reg.save()
    result = true

    return result
  }

  return result
}

async function checkPassword (username, password) {
  var result = false
  var user = await Event.findOne({ username: username }).exec()
  if (user != null) {
    var hash = crypto.createHash('sha256').update(password).digest('base64')
    if (hash === user.password) {
      result = true
    }
  }
  return result
}
async function addWin (username) {
  var count = 0
  var user = await Event.findOne({ username: username }).exec()
  if (user != null) {
    const currentCount = user.winsCount + 1
    user.winsCount = currentCount
    user.save()
    count = currentCount
  }
  return count
}

var players = new Map()
var rooms = new Map()

const NUM_PLAYER_MAX = 4

var numOfPlayers = 0
gameSocket = io.on('connection', function (socket) {
  // ### the connection of a player ###

  numOfPlayers += 1
  // var playerNum = numOfPlayers
  players.set(socket.id, {}) //  the socket id is assign to a null object where we could store things : position...
  
  //  ### the disconnection of a player ###
  //  remove player from the rooms and players tabs

  socket.on('disconnect', function () {

    //  if the player is currently in a room
    if (players.get(socket.id) !== undefined && players.get(socket.id).hasOwnProperty('room')) {
      //  send a disconnected event to the other players in the room
      socket.to(players.get(socket.id).room).emit('playerDisconnected', { playerId: socket.id })

      //  remove the player from the room
      var index = rooms.get(players.get(socket.id).room).indexOf(socket.id)
      if (index !== -1) rooms.get(players.get(socket.id).room).splice(index, 1)

      //  if the room becomes empty then remove it
      if (rooms.get(players.get(socket.id).room).length === 0) {
        rooms.delete(players.get(socket.id).room)
      }
    }

    socket.leave(players.get(socket.id).room)

    //  remove the player from the players Map
    players.delete(socket.id)
    //  decrement player number
    numOfPlayers -= 1
  })

  //  ### all the test events ###

  socket.on('test-event1', function () {
    console.log('got test-event1')
  })

  socket.on('test-event2', function (data) {
    console.log('got test-event2')
    console.log(data)

    socket.emit('test-event', {
      test: 12345,
      test2: 'test emit event'
    })
  })

  socket.on('test-event3', function (data, callback) {
    console.log('got test-event3')
    console.log(data)

    callback ({
      test: 123456,
      test2: 'test3'
    })
  })

  //  ### events for the menu ###

  socket.on('login', async function (data, callback) {
    var login = data.login
    var password = data.password
    var loggedIn = await checkPassword(login, password)

    if (loggedIn) {
      players.set(socket.id, { username: login })
    }

    callback({
        loggedIn: loggedIn
    })
  })

  socket.on('register', async function (data, callback) {
    var login = data.login
    var password = data.password
    var loggedIn = await saveUser(login, password)

    if (loggedIn) {
      players.set(socket.id, { username: login })
    }

    callback({
        loggedIn: loggedIn
    })
  })

  socket.on('createRoom', function (data, callback) {
    var roomName = data.roomName
    roomName = roomName.slice(0, roomName.length - 1)

    //  search if the room name is available
    var available = true
    for (const key of rooms.keys()) {
      if (key === roomName) {
        available = false
      }
    }
    if (available === true) {
      rooms.set(roomName, [socket.id])

      if (players.get(socket.id) === undefined) {
        players.set(socket.id, { room: roomName })
      } else {
        Object.assign(players.get(socket.id), { room: roomName })
      }

      socket.join(roomName)
      callback({
        roomAvailable: true
      })
    } else {
      callback({
        roomAvailable: false
      })
    }
  })

  socket.on('joinRoom', function (data, callback) {
    var roomName = data.roomName
    var available = false
    for (const key of rooms.keys()) {
      if (key === roomName) {
      //  check if there are already to much people
        if (rooms.get(roomName).length < NUM_PLAYER_MAX) {
          available = true
        }
        if (rooms.get(roomName).length + 1 === NUM_PLAYER_MAX) {
          available = true
        }
      }
    }
    if (available === true) {
      rooms.get(roomName).push(socket.id)

      Object.assign(players.get(socket.id), { room: roomName })

      socket.join(roomName)

      //  send callback available true
      callback({
        roomAvailable: true
      })
    } else {
      //  send callback available false
      callback({
        roomAvailable: false
      })
    }
  })

  socket.on('requestPlayersPos', () => {
    var allRoomPlayersExceptPlayer = []

    for (var playerId of rooms.get(players.get(socket.id).room)) {
      if (playerId !== socket.id) {
        allRoomPlayersExceptPlayer.push(playerId)
      }
    }

    //  if the player is not alone in the room
    if (Array.isArray(allRoomPlayersExceptPlayer) && allRoomPlayersExceptPlayer.length > 0) {
      var playersCopy = []
      for (playerId of allRoomPlayersExceptPlayer) {
        if (!players.get(playerId).hasOwnProperty('dead') || players.get(playerId).dead === false) {
          playersCopy.push({ playerId: playerId, pos: players.get(playerId).pos })
        }
      }
    }

    var jsonObj = { Players: playersCopy }

    socket.emit('allPlayersPositions', jsonObj) //  then the current position of all the players in the game, to spawn them locally
  })

  socket.on('refreshRoomsList', (data, callback) => {
    //  expected result
    var result = []
    for (var [key, value] of rooms) {
      result.push({ roomName: key, numPlayers: value.length })
    }
    var JSONobj = { Rooms: result }
    callback(JSONobj)
  })

  //  ### For the player movements ###

  socket.on('moveInput', function (data, callback) {
    var mov = { playerId: socket.id, moveInput: data.moveInput }
    socket.to(players.get(socket.id).room).emit('moveInput', mov)
  })

  socket.on('jump', function () {
    socket.to(players.get(socket.id).room).emit('jump', { playerId: socket.id })
  })

  socket.on('playerMoved', function (data, callback) {
    players.set(socket.id, Object.assign(players.get(socket.id), { pos: data }))
    var pos = { playerId: socket.id, pos: data }
    socket.to(players.get(socket.id).room).emit('playerMoved', pos)
  })

  socket.on('playerRotated', function (data, callback) {
    var pos = { playerId: socket.id, pos: data }
    socket.to(players.get(socket.id).room).emit('playerRotated', pos)
  })

  socket.on('velocityChanged', function (data, callback) {
    var json = { playerId: socket.id, velocity: data }
    socket.to(players.get(socket.id).room).emit('velocityChanged', json)
  })

  socket.on('attack', () => {
    socket.to(players.get(socket.id).room).emit('playerAttack', { playerId: socket.id })
  })

  socket.on('endAttack', () => {
    socket.to(players.get(socket.id).room).emit('playerEndAttack', { playerId: socket.id })
  })

  socket.on('playerKilled', (data) => {
    players.set(data.playerId, Object.assign(players.get(data.playerId), { dead: true }))

    //  score : +1 for socket id player

    //  check if player is the last one
    var counterDead = 0
    for (var playerId of rooms.get(players.get(socket.id).room)) {
      if (players.get(playerId).hasOwnProperty('dead') && players.get(playerId).dead === true) {
        counterDead = counterDead + 1
      }
    }
    if (counterDead === 3) {
      //  the player socket id is the winner
      addWin(players.get(socket.id).username)
    }

    socket.to(players.get(socket.id).room).emit('playerKilled', { playerId: data.playerId })
  })

  socket.on('deathByDeathFloor', () => {
    players.set(socket.id, Object.assign(players.get(socket.id), { dead: true }))
    //  send death of the player to the other players
    socket.to(players.get(socket.id).room).emit('playerDeathByDeathFloor', { playerId: socket.id })
  })
})
