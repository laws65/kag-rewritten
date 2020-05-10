Engine.Import("Rules/Rules.js")

class SandboxRules extends Rules {
    constructor() {
        super()

        this.name = "Sandbox"
        this.description = "Build whatever your heart desires. Respect other folk's creations!"
    }

    Start() {
        Debug.Log(this.name)
        Debug.Log(this.description)

        MapLoader.Load("Maps/CTF/Mazey_Epic.png")
    }
    
    OnPlayerConnected(player) {
        Debug.Log(player.Username + " has joined the game.")

        Engine.Instantiate("Entities/Characters/Builder.js")
    }

    OnPlayerDisconnected(player) {

    }
}

Engine.Export(SandboxRules)
