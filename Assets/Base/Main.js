Engine.Include("Maps/MapLoader.js")
Engine.Include("Entities/Characters/Builder.js")

var Main = {
    Start: function(gamemode = "") {
        MapLoader.Load("Maps/CTF/Mazey_Epic.png")
        Game.Instantiate(new Builder())
    }
}
