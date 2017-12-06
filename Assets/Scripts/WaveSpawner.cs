using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public List<GameObject> enemies;
    public List<GameObject> newEnemies;
    public GameObject previousWave;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < transform.childCount; i++)
        {
            enemies.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.SetActive(false);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnWave()
    {

        //wave.SetActive(true);
        //gameObject.SetActive(false);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(true);
            GameObject newEnemy = Instantiate(enemies[i], enemies[i].transform.position, Quaternion.identity);
            newEnemies.Add(newEnemy);
            enemies[i].SetActive(false);
            if(previousWave != null) { 
            previousWave.GetComponent<WaveSpawner>().ClearPrevious();
            }
            foreach(var enemy in newEnemies)
            {
                enemy.GetComponent<EggRobot>().spawned = true;
            }

        }
    }

    public void ClearPrevious()
    {
        foreach(GameObject go in enemies)
        {
            Destroy(go);
        }

        foreach(GameObject go in newEnemies)
        {
            Destroy(go);
        }
    }

    public void OnReload()
    {
        foreach(GameObject enemy in newEnemies)
        {
            Destroy(enemy);
        }
        newEnemies.Clear();
    }
}
