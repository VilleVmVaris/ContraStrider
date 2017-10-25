using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {

    public List<string> powerUpS = new List<string>();
   
    void Start () {
        // powerUpS.Add("heal");
	}
    void OnCollisionEnter(Collision other) {
        // muista tarkistaa layer(PowerUp), törmäys
        var power = other.gameObject.name;
        powerUpS.Add("" + power);
        Destroy(other.gameObject);
    }
}
