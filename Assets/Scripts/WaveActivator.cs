using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveActivator : MonoBehaviour {
	public WaveSpawner wave;
    public bool spawned;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {if(!spawned) { 
            //wave.SetActive(true);
            wave.SpawnWave();
            gameObject.SetActive(false);
            spawned = true;
            }
        }
    }
}
