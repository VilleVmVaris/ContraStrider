using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public Transform upperPosition;

    public Transform leftPosition;

    public Transform rightPosition;

    ArcMover2D arcmover;

    public DamageSource aoeDamage;

    public List<GameObject> attackAreas;

    public int safeAmount;

    public int health;

    TimerManager timer;

    public bool usedAttack;

    public float positionTolerance;

	// Use this for initialization
	void Start () {

        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();

        arcmover = GetComponent<ArcMover2D>();

        transform.position = leftPosition.position;

        SwitchSide();

    }
	
	// Update is called once per frame
	void Update () {

        if(attackAreas.Count != aoeDamage.attackAreas.Count)
        { 
        attackAreas = aoeDamage.attackAreas;
        }

        

        // if (!usedAttack) {
        //   AreaAttack();


        //}
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

    void BulletAttack()
    {

    }

    void SwitchSide()
    {
        if(Vector3.Distance(transform.position, rightPosition.position) <= positionTolerance)
        {
            arcmover.SetTarget(leftPosition.position);
            arcmover.TargetReached += SwitchSide;
            print("vasen");

        } else if (Vector3.Distance(transform.position, leftPosition.position) <= positionTolerance)
        {
            arcmover.SetTarget(rightPosition.position);
            arcmover.TargetReached += SwitchSide;
            print("oikea");

        } else if(Vector3.Distance(transform.position, upperPosition.position) <= positionTolerance)
        {
            print("random");
            int randomSide = Random.Range(0, 1);

            if (randomSide == 0)
            {
                arcmover.SetTarget(rightPosition.position);
                arcmover.TargetReached += SwitchSide;

            }
            else
            {
                arcmover.SetTarget(leftPosition.position);
                arcmover.TargetReached += SwitchSide;
            }
        }

    }

        

}
