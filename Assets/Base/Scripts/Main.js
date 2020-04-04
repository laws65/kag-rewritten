Engine.Include("Scripts/Map/MapLoader.js");

var Main = {
    Start: function(gamemode = "") {
        MapLoader.Load("Maps/Mazey_Epic.png")
    }
}
