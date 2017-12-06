using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    public int health;
    public Vector3 playerPosition; // CheckPoint
    // public float score; Optio
    public List<GameObject> enemyList;
    public List<GameObject> spawnList;
}