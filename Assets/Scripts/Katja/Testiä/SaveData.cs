using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    public float health;
    public Vector3 playerPosition; // CheckPoint
    public List<string> powerUpS;
}