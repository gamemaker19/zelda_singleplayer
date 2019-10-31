// BASE SETUP
// =============================================================================

// call the packages we need
var express    = require('express');        // call express
var app        = express();                 // define our app using express
var bodyParser = require('body-parser');
var fs = require("fs");
var connect = require('connect');
var serveStatic = require('serve-static');

app.use(serveStatic(__dirname));

// configure app to use bodyParser()
// this will let us get the data from a POST
app.use(bodyParser.urlencoded({ extended: true, limit: '500mb', extended: true, parameterLimit: 500000 }));
app.use(bodyParser.json({limit:'500mb', type:'application/json',extended: true, parameterLimit: 500000}));

var port = process.env.PORT || 8080;        // set our port

// ROUTES FOR OUR API
// =============================================================================
var router = express.Router();              // get an instance of the express Router

app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});

// test route to make sure everything is working (accessed at GET http://localhost:8080/api)
router.get('/', function(req, res) {
    res.json({ message: 'hooray! welcome to our api!' });   
});


router.get('/get-level-images', function(req, res) {
  var dirname = "assets/levelimages/";
  var paths = [];
  
  var fileNames = fs.readdirSync(dirname);
  
  for(var filename of fileNames) {
    paths.push(dirname + filename);
  }

  res.json(paths);

});

router.post('/save-level-images', function(req, res) {
  for(let i = 0; i < req.body.layers.length; i++) {
    let layer = req.body.layers[i];
    var fs = require("fs");
    fs.writeFile('./assets/levelimages/' + req.body.levelName + "_" + String(i) + ".png", layer, "base64", function(err) {});
  }
  res.json({message:"Success"});
});

router.get('/get-spritesheets', function(req, res) {
  var dirname = "assets/spritesheets/";
  var paths = [];
  
  var fileNames = fs.readdirSync(dirname);
  
  for(var filename of fileNames) {
    if(!filename.includes(".")) continue;
    paths.push(dirname + filename);
  }

  res.json(paths);

});

router.get('/get-tilesets', function(req, res) {
  var dirname = "assets/tilesets/";
  var paths = [];
  
  var fileNames = fs.readdirSync(dirname);
  
  for(var filename of fileNames) {
    paths.push(dirname + filename);
  }

  res.json(paths);

});

router.get('/get-backgrounds', function(req, res) {
  var dirname = "assets/backgrounds/";
  var paths = [];
  
  var fileNames = fs.readdirSync(dirname);
  
  for(var filename of fileNames) {
    paths.push(dirname + filename);
    
  }

  res.json(paths);

});

router.get('/get-sprites', function(req, res) {
  var dirname = "assets/sprites/";
  var sprites = [];
  
  var fileNames = fs.readdirSync(dirname);
  
  for(var filename of fileNames) {
    if(!filename.endsWith(".json")) continue;
    var content = fs.readFileSync(dirname + filename, 'utf-8');
    var sprite = JSON.parse(content);
    sprite.path = dirname + filename;
    sprite.name = filename.split(".")[0];
    sprites.push(sprite);
  }

  res.json(sprites);

});

function saveSpriteHelper(req) {
  fs.writeFileSync("./assets/sprites/" + req.name + ".json", JSON.stringify(req));
}

router.post('/save-sprite', function(req, res) {
  saveSpriteHelper(req.body);
  res.json({message:"Success"});
});

router.post('/save-sprites', function(req, res) {
  console.log(req.body.data);
  for(var sprite of req.body.data) {
    saveSpriteHelper(sprite);
  }
  res.json({message:"Success"});
});

router.get('/get-levels', function(req, res) {
  var dirname = "assets/levels/";
  var levels = [];
  
  var fileNames = fs.readdirSync(dirname);
  
  for(var filename of fileNames) {
    if(!filename.endsWith(".json")) continue;
    var content = fs.readFileSync(dirname + filename, 'utf-8');
    var level = JSON.parse(content);
    level.path = dirname + filename;
    levels.push(level);
  }

  res.json(levels);

});

router.post('/save-level', function(req, res) {
  fs.writeFileSync("./assets/levels/" + req.body.name + ".json", JSON.stringify(req.body));
  res.json({message:"Success"});
});

router.post('/save-hash-cache', function(req, res) {
  console.log(req);
  fs.writeFileSync("./hashcache/hashCaches.json", JSON.stringify(req.body));
  res.json({message:"Success"});
});

//////////////////

router.get('/get-resource', function(req, res) {
  var dirname = "assets/" + req.query.resourceName + "s.json";  
  
  var contents = fs.readFileSync(dirname, 'utf-8');
  var resources = JSON.parse(contents);
  
  res.json(resources);

});

router.post('/save-resource', function(req, res) {
  var jsonStr = JSON.stringify(req.body.items);
  if(jsonStr === "undefined" || jsonStr === undefined) jsonStr = "[]";
  //console.log(jsonStr);
  fs.writeFileSync("./assets/" + req.query.resourceName + "s.json", jsonStr);
  res.json({message:"Success"});
});

// REGISTER OUR ROUTES -------------------------------
// all of our routes will be prefixed with /api
app.use('/', router);

// START THE SERVER
// =============================================================================
app.listen(port);
//console.log('Magic happens on port ' + port);
