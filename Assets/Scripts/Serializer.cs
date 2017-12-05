using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Serializer : MonoBehaviour {

    static readonly string SAVE_FILE = "player.dat";
    static readonly string JSON_ENCRYPTED_KEY = "katjakatjakatjakatjakatjakatjaZY";

    byte[] soup;
    byte[] soupBackIn;

    string filename;
    string jsonFromFile;

    SaveData copy;
    Rijndael crypto;

    public GameObject player;

    public void SetCheckPoint() {
        
    SaveData data = new SaveData()
		{health = player.GetComponent<Player>().health, playerPosition = player.transform.position, /*score = 0,*/enemyList = new List<GameObject>() };
        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var g in enemyGameObjects) {
            data.enemyList.Add(g);
        }


        string json = JsonUtility.ToJson(data);

        crypto = new Rijndael();

        soup =crypto.Encrypt(json, JSON_ENCRYPTED_KEY);

        filename = Path.Combine(Application.persistentDataPath, SAVE_FILE);
        
        if (File.Exists(filename)) {
            File.Delete(filename);
        }

        File.WriteAllBytes(filename, soup);

    }
    public void LoadCheckPoint() {

        //health = copy.health;
        jsonFromFile = File.ReadAllText(filename);
        soupBackIn = File.ReadAllBytes(filename);
        jsonFromFile = crypto.Decrypt(soupBackIn, JSON_ENCRYPTED_KEY);

        copy = JsonUtility.FromJson<SaveData>(jsonFromFile);
        //Debug.Log(copy.enemyList);
        player.transform.position = copy.playerPosition;
        player.GetComponent<Player>().health = copy.health;


    }
}