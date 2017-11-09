using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Serializer : MonoBehaviour {

    static readonly string SAVE_FILE = "player.dat";

    public GameObject player;

    void Start() {
        SaveData data = new SaveData()
        { health = 100, playerPosition = player.transform.position, powerUpS = new List<string>() };
        data.powerUpS.Add("Test1");
        data.powerUpS.Add("Test2");

        string json = JsonUtility.ToJson(data);
        Rijndael crypto = new Rijndael();
        byte[] soup =crypto.Encrypt(json, "katjakatjakatjakatjakatjakatjaZY");
        // Debug.Log(json);
        string filename = Path.Combine(Application.persistentDataPath, SAVE_FILE);

        if (File.Exists(filename)) {
            File.Delete(filename);
        }

        File.WriteAllBytes(filename, soup);

        //Debug.Log("Player saved to " + filename);
        //string jsonFromFile = File.ReadAllText(filename);
        //SaveData copy = JsonUtility.FromJson<SaveData>(jsonFromFile);
        //Debug.Log(copy.playerPosition);
    }
}