GameDebug.Log("Testing logging")
GameDebug.LogError("Testing error logging")
GameDebug.LogWarning("Testing warning logging")

var obj = GameUtils.ParseJson("Scripts/Map/Colors.json")
GameDebug.Log(obj[0].Color)
