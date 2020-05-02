class Main {
    constructor() {
        this.rules = null
    }

    Start(rulesPath) {
        this.rules = Engine.Import(rulesPath)
        this.rules.Start()
    }

    OnPlayerConnected(player) {
        Debug.Log(player.Username)
    }

    OnPlayerDisconnected(player) {
        Debug.Log(player.Username)
    }
}
