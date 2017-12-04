using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Serializer : MonoBehaviour {

    static readonly string SAVE_FILE = "player.dat";
    static readonly string JSON_ENCRYPTED_KEY = "katjakatjakatjakatjakatjakatjaZY";
    
    public GameObject player;

    public void SetCheckPoint() {
    SaveData data = new SaveData()
        {health = 100, playerPosition = player.transform.position, /*score = 0,*/enemyList = new List<string>() };
        //data.enemyList.Add("Test1");
        //data.enemyList.Add("Test2");


        string json = JsonUtility.ToJson(data);

        Rijndael crypto = new Rijndael();

        byte[] soup =crypto.Encrypt(json, JSON_ENCRYPTED_KEY);
        Debug.Log(json);
        string filename = Path.Combine(Application.persistentDataPath, SAVE_FILE);
        

        if (File.Exists(filename)) {
            File.Delete(filename);
        }

        File.WriteAllBytes(filename, soup);
        Debug.Log("Player saved to " + filename);
        //string jsonFromFile = File.ReadAllText(filename);
        //byte[] soupBackIn = File.ReadAllBytes(filename);
        //string jsonFromFile = crypto.Decrypt(soupBackIn, JSON_ENCRYPTED_KEY);
        
        //SaveData copy = JsonUtility.FromJson<SaveData>(jsonFromFile);
        //Debug.Log(copy.playerPosition);
    }
    public void LoadCheckPoint() {
        //player.transform.position = copy.playerPosition;
        //health = copy.health;

    }
}