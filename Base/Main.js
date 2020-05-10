class Main {
    constructor() {
        
    }

    Start(rulesPath) {
        let rulesClass = Engine.Import(rulesPath)

        this.rules = Engine.FromClass(rulesClass)
        this.rules.Start()
    }

    OnPlayerConnected(player) {
        this.rules.OnPlayerConnected(player)
    }

    OnPlayerDisconnected(player) {
        this.rules.OnPlayerDisconnected(player)
    }
}

Engine.Export(Main)