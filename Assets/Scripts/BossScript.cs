using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public List<GameObject> attackAreas;

    public DamageSource aoeDamage;

    public int safeAreaAmount;

    public Transform upperPosition;

    public Transform leftPosition;

    public Transform rightPosition;

    public float positionTolerance;

    public GameObject bossBullet;

    public Transform bulletSpawner;

    public int damage;

    public float bulletSpeed;

    public float bulletLifetime;

    public float bulletTurn;

    public int bulletAmount;

    public int health;

    public int bulletAttacksInLoop;

    int attacksDone;

    bool inFirePosition = true;

    bool inBulletMode = true;

    TimerManager timer;

    GameObject player;

    ArcMover2D arcmover;


    // Use this for initialization
    void Start () {

        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();

        arcmover = GetComponent<ArcMover2D>();

        transform.position = rightPosition.position;

        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {

        if(attackAreas.Count != aoeDamage.attackAreas.Count)
        { 
        attackAreas = aoeDamage.attackAreas;
        }

        if(inFirePosition && inBulletMode) {
            StopCoroutine("AreaAttack"); 
            StartCoroutine("BulletAttack");
            
        } else if (inFirePosition && !inBulletMode)
        {
            StopCoroutine("BulletAttack");
            StartCoroutine("AreaAttack");
        }
    }


    IEnumerator AreaAttack()
    {
        inFirePosition = false;

        yield return new WaitForSeconds(1f);
       
       for(int i = 0; i < attackAreas.Count - safeAreaAmount;)
        {
            int randomArea = Random.Range(0, 6);

            if(!attackAreas[randomArea].activeSelf)
            {
                attackAreas[randomArea].SetActive(true);
                print(attackAreas[randomArea].name + " active");
                i++;
                yield return new WaitForSeconds(.5f);
            }
        }
        yield return new WaitForSeconds(1f);
        DeadactivateAttackAreas();
        print("alueet pistetty hoi");

        SwitchPhase();
        SwitchSide();
    }

    IEnumerator BulletAttack()
    {
        inFirePosition = false;
        for (int i = 0; i < bulletAmount; i++)
        {
            var fireDirection = player.transform.position - transform.position;

            GameObject bullet = Instantiate(bossBullet, bulletSpawner.position, Quaternion.identity);

            bullet.GetComponent<Bullet>().Projectile(damage, bulletSpeed, fireDirection, bulletLifetime);

            yield return new WaitForSeconds(.2f);

        }
        yield return new WaitForSeconds(.5f);

        attacksDone++;

        if (attacksDone >= bulletAttacksInLoop)
        {
            SwitchPhase();
        }

        SwitchSide();

    }

    void SwitchPhase()
    {
        if(inBulletMode)
        {
            inBulletMode = false;

        } else
        {
            attacksDone -= bulletAttacksInLoop;
            inBulletMode = true;
        }
    }

    void SwitchSide()
    {
        if(!inBulletMode)
        {
            //arcmover.SetTarget(upperPosition.position);
            //arcmover.TargetReached += ReadyToAttack;

            transform.position = upperPosition.position;
            ReadyToAttack();
            
        }

         else if(Vector3.Distance(transform.position, rightPosition.position) <= positionTolerance)
        {
            arcmover.SetTarget(leftPosition.position);
            arcmover.TargetReached += ReadyToAttack;


        } else if (Vector3.Distance(transform.position, leftPosition.position) <= positionTolerance)
        {
            
            arcmover.SetTarget(rightPosition.position);
            arcmover.TargetReached += ReadyToAttack;


        } else if(Vector3.Distance(transform.position, upperPosition.position) <= positionTolerance)
        {

            print("pitäis lähtee");

            int randomSide = Random.Range(0, 2);

            if (randomSide == 0)
            {

                arcmover.SetTarget(rightPosition.position);
                arcmover.TargetReached += ReadyToAttack;

            }
            else
            {

                arcmover.SetTarget(leftPosition.position);
                arcmover.TargetReached += ReadyToAttack;
            }
        }

    }

     void ReadyToAttack()
    {
        inFirePosition = true;
    }

    void DeadactivateAttackAreas()
    {
        foreach(var area in attackAreas)
        {
            area.SetActive(false);
        }
    }

}
