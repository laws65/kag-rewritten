class Builder {
	constructor() {

	}

	Start() {
		Debug.Log(this.isMine)
		Debug.Log(this.isClient)
		Debug.Log(this.isServer)
	}

	Update() {
		
	}
}

Engine.Export(Builder)