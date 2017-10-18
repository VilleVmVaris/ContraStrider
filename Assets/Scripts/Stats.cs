using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    [Header("Int")]
    public int health;
    public int damage;
    public int burstInterval; // Amount of Ticks

    [Header("Float")]
    public float projectileSpeed;
    public float moveSpeed;
    public float destroyDelay;
    public float shootingDistance;
}
