var crypto = require('crypto')
var hash = crypto.createHash('sha256').update('pwd').digest('base64')
const mongoose = require('mongoose')
mongoose.connect('mongodb://localhost:27017/TP2', { useNewUrlParser: true, useUnifiedTopology: true })
const Schema = mongoose.Schema
var EventSchema = new Schema({
  title: { type: String, required: true },
  start: { type: Date, required: true },
  end: { type: Date, required: true },
  owner: { type: String }
})
const Event = mongoose.model('Event', EventSchema)

console.log(hash)

async function testTibibi (i) {
  i = 42
  i = 54
  i = i * 2
  console.log(i)
  return i
}

async function testingThings () {
  var i = 42
  var j = await testTibibi(i)
  console.log(`The j result is ${j}`)
  console.log('This is the result here:')
  console.log(i)
  return i
}

// testingThings()

async function getInCircle () {
  const query = await Event.findOne({ _id: '5dc2d2c2274d4831346947d1' }).exec()
  console.log(query)

  // console.log(query.select('stop_desc'))
  /* await query.exec((err, res) => {
    if (err) return err

    console.log(res.title)
  }) */
  return 'tibibi'
}

var res = getInCircle()
console.log(res)
