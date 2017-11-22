using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class BossScript : MonoBehaviour, Damageable {

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

    Vector2 fireDirection;

    [HideInInspector]
    public bool undamageable;

    float fireSegment;

    public Animator bossAnimator;


    // Use this for initialization
    void Start () {

        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();

        arcmover = GetComponent<ArcMover2D>();

        transform.position = rightPosition.position;

        player = GameObject.FindGameObjectWithTag("Player");

        fireSegment = (float) 2/bulletAmount;

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
        bossAnimator.SetBool("velileijuu", true);

        inFirePosition = false;

        yield return new WaitForSeconds(1f);
       
       for(int i = 0; i < attackAreas.Count - safeAreaAmount;)
        {
            int randomArea = Random.Range(0, attackAreas.Count);

            if(!attackAreas[randomArea].GetComponent<AreaDamageScript>().enabled)
            {
                //attackAreas[randomArea].SetActive(true);
                //print(attackAreas[randomArea].name + " active");

                var area = attackAreas[randomArea];

                area.SetActive(true);

                area.GetComponent<MeshRenderer>().enabled = true;

                yield return new WaitForSeconds(1f);

                
                area.GetComponent<MeshRenderer>().enabled = false;


                //Add cool particle effect here

                
                area.GetComponent<AreaDamageScript>().enabled = true;
                area.GetComponent<BoxCollider2D>().enabled = true;

                i++;
            }
            bossAnimator.SetBool("velileijuu", false);


        }
        yield return new WaitForSeconds(1f);

        DeadactivateAttackAreas();

        SwitchPhase();
        SwitchSide();
    }

    #region Damageable implementation

    public bool TakeDamage(int damage)
    {
        print("osuma");
        health -= damage; 
        if (health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    IEnumerator BulletAttack()
    {
        bossAnimator.SetBool("veliammus", true);
        print(fireSegment);
        fireDirection.y = -1;

        inFirePosition = false;
        for (int i = 0; i < bulletAmount; i++)
        {
            //float dirToPlayerX = player.transform.position.x - transform.position.x;

            //print(dirToPlayerX);

            GameObject bullet = Instantiate(bossBullet, bulletSpawner.position, Quaternion.identity);

            bullet.GetComponent<Bullet>().Projectile(damage, bulletSpeed, fireDirection, bulletLifetime);

            fireDirection.y += fireSegment;
            
            yield return new WaitForSeconds(.2f);

            

        }
        yield return new WaitForSeconds(.5f);

        attacksDone++;

        bossAnimator.SetBool("veliammus", false);

        if (attacksDone >= bulletAttacksInLoop)
        {
            SwitchPhase();
        }

        SwitchSide();

    }

    void Die()
    {
        print("bossi kuoli xD");
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
        bossAnimator.SetBool("veliJumpUP", true);

        if(!inBulletMode)
        {
            //arcmover.SetTarget(upperPosition.position);
            //arcmover.TargetReached += ReadyToAttack;

            transform.position = upperPosition.position;
            ReadyToAttack();
            
        }

         else if(Vector2.Distance(transform.position, rightPosition.position) <= positionTolerance)
        {
            arcmover.SetTarget(leftPosition.position);
            arcmover.TargetReached += ReadyToAttack;
            fireDirection.x = 1;


        } else if (Vector2.Distance(transform.position, leftPosition.position) <= positionTolerance)
        {
            
            arcmover.SetTarget(rightPosition.position);
            arcmover.TargetReached += ReadyToAttack;
            fireDirection.x = -1;


        } else if(Vector2.Distance(transform.position, upperPosition.position) <= positionTolerance)
        {

            int randomSide = Random.Range(0, 2);

            if (randomSide == 0)
            {

                arcmover.SetTarget(rightPosition.position);
                arcmover.TargetReached += ReadyToAttack;
                fireDirection.x = -1;

            }
            else
            {

                arcmover.SetTarget(leftPosition.position);
                arcmover.TargetReached += ReadyToAttack;
                fireDirection.x = 1;
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
            area.GetComponent<BoxCollider2D>().enabled = false;
            area.GetComponent<AreaDamageScript>().enabled = false;
            
            area.SetActive(false);
        }
    }

}
