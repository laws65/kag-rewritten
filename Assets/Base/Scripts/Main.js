Debug.Log("Testing logging")
Debug.LogError("Testing error logging")
Debug.LogWarning("Testing warning logging")

var obj = Utils.ParseJson("Scripts/Map/Colors.json")
Debug.Log(obj[0].Color)
