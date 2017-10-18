using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    [Header("Int")]
    public int health;
    public int damage;
    public int burstInterval; // Amount of Ticks
    public int burstAmount;

    [Header("Float")]
    public float projectileSpeed;
    public float moveSpeed;
    public float bulletDestroyDelay;
    public float shootingDistance;
    public float chaseDistance;
}
