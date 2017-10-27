using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {

    public List<string> powerUpS = new List<string>();
   // Lisätään GameManageriin
    void Start () {
        // powerUpS.Add("heal");
	}
    void OnTriggerEnter2D(Collider2D other) {
        // muista tarkistaa layer(PowerUp), törmäys
        if (other.gameObject.layer == 20) {
            var power = other.gameObject.name;
            powerUpS.Add("" + power);
            Debug.Log("Törmäys?");
            Destroy(other.gameObject);
        }
    }
    void Update() {
        
    }
}
