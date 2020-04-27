Engine.Include("Maps/MapLoader.js")
Engine.Include("Entities/Characters/Builder.js")

var Main = {
    Start: function(gameMode = "") {
        MapLoader.Load("Maps/CTF/Mazey_Epic.png")
    }
}
