using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public DamageSource aoeDamage;

    public List<GameObject> attackAreas;

    public int safeAmount;

    public int health;

    TimerManager timer;

    public bool usedAttack;

	// Use this for initialization
	void Start () {

        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();

        

    }
	
	// Update is called once per frame
	void Update () {

        if(attackAreas.Count != aoeDamage.attackAreas.Count)
        { 
        attackAreas = aoeDamage.attackAreas;
        }

        if (!usedAttack) {
            AreaAttack();

        }
    }


    void AreaAttack()
    {
        print("moi");
       for(int i = 0; i < attackAreas.Count - safeAmount;)
        {
            int randomArea = Random.Range(0, 6);

            if(!attackAreas[randomArea].activeSelf)
            {
                attackAreas[randomArea].SetActive(true);
                print(attackAreas[randomArea].name + " active");
                i++;
            }
        }
       usedAttack = true;
    }

}
