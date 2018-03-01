using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    string gameDataFilePath = "/Streaming Assets/data.json";

	// Use this for initialization
	void Start () {
        GameData jsonObj = new GameData();
        jsonObj.playerName = "Rob";
        jsonObj.score = 2000;
        jsonObj.timePlayed = 12000.456f;
        jsonObj.lastLogin = "March 1st 2018";

        string json = JsonUtility.ToJson(jsonObj);
        string filePath = Application.dataPath + gameDataFilePath;

        File.WriteAllText(filePath, json);

        Debug.Log(json);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
