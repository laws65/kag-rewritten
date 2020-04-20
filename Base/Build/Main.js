"use strict";

Engine.Include("Maps/MapLoader.js");
Engine.Include("Entities/Characters/Builder.js");
var Main = {
  Start: function Start() {
    var gameMode = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
    MapLoader.Load("Maps/CTF/Mazey_Epic.png");
    Game.Instantiate("Player", new Builder());
  }
};