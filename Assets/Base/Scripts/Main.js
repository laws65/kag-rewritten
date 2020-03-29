Debug.Log("Testing logging")
Debug.LogError("Testing error logging")
Debug.LogWarning("Testing warning logging")

var obj = Utils.FromJson("Scripts/Map/Colors.json")
Debug.Log(obj)

var json = Utils.ToJson(obj)
Debug.Log(json)

Assert.AreEqual(obj, json)
