Engine.Include("Scripts/Map/MapLoader.js")
Engine.Include("Entities/Characters/Builder.js")

var Main = {
    Start: function(gamemode = "") {
        MapLoader.Load("Maps/Mazey_Epic.png")
        Engine.Instantiate(new Builder())
    }
}
