Engine.Include("Maps/MapLoader.js")

class Main {
    Start(gameMode = "") {
        let mapLoader = new MapLoader()
        mapLoader.Load("Maps/CTF/Mazey_Epic.png")
    }

    OnPlayerConnected(player) {
        Debug.Log(player.Username)
    }

    OnPlayerDisconnected(player) {
        Debug.Log(player.Username)
    }
}
