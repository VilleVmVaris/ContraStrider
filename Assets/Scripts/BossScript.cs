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
    int maxHealth;

    public int bulletAttacksInLoop;

    int attacksDone;

    bool inFirePosition = true;

    bool inBulletMode = true;

    bool paused;

    TimerManager timer;
	GUIManager gui;
    GameObject player;
    GameManager gm;
	AudioManager sounds;

    ArcMover2D arcmover;

    Vector2 fireDirection;

    [HideInInspector]
    public bool undamageable;

    float fireSegment;

    public Animator bossAnimator;

	SpriteMeshInstance[] sprites;
	Color originalColor;
	Color hitFlashColor = new Color(.9f, .3f, .3f, 1f);

	Lightning lightning;

	bool dead = false;
	bool active = false;


    // Use this for initialization
    void Start () {
        maxHealth = health;
		gui = GameObject.Find("GameCanvas").GetComponent<GUIManager>();
        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		sounds = GameObject.Find("Audio").GetComponent<AudioManager>();

        arcmover = GetComponent<ArcMover2D>();

        transform.position = rightPosition.position;

        player = GameObject.FindGameObjectWithTag("Player");

        fireSegment = (float) 2/bulletAmount;

		sprites = GetComponentsInChildren<SpriteMeshInstance>();
		originalColor = sprites[0].color;

		lightning = GetComponentInChildren<Lightning>();

       
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.state != GameManager.Gamestate.Paused)
        {
            paused = false;

            if (Vector3.Distance(player.transform.position, transform.position) < 10f)
            {
                gui.ShowBossHealthBar(this);
                sounds.ambient.Play();
                Camera.main.GetComponent<CameraFollow>().ActivateBossMode();
                active = true;
            }
            if (active)
            {
                if (attackAreas.Count != aoeDamage.attackAreas.Count)
                {
                    attackAreas = aoeDamage.attackAreas;
                }
                if (inFirePosition && inBulletMode)
                {
                    StopCoroutine("AreaAttack");
                    StartCoroutine("BulletAttack");
                }
                else if (inFirePosition && !inBulletMode)
                {
                    StopCoroutine("BulletAttack");
                    StartCoroutine("AreaAttack");
                    bossAnimator.SetBool("velileijuu", true);
                }
            }
        }
        else paused = true;
    }

    IEnumerator AreaAttack()
    {
        if(paused)
        {
            yield return new WaitForSeconds(1f);
        }
        if(((health / maxHealth) * 100) <= 60)
        {
            safeAreaAmount = 2;
        }

        if(((health / maxHealth) * 100) <= 40)
        {
            safeAreaAmount = 1;
        }

        inFirePosition = false;

		sounds.magick.Play();

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

                // We now have a cool particle effect here
				sounds.lightning.Play();
				lightning.Strike(area.transform.position + Vector3.down);

                area.GetComponent<AreaDamageScript>().enabled = true;
                area.GetComponent<BoxCollider2D>().enabled = true;

                i++;
            }
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
		foreach (var sprite in sprites) {
			sprite.color = hitFlashColor;
		}
		timer.Once(FlashSpriteColorBack, .1f);
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

	void FlashSpriteColorBack() {
		foreach (var sprite in sprites) {
			sprite.color = originalColor;
		}
	}

    IEnumerator BulletAttack()
    {
        if (paused)
        {
            yield return new WaitForSeconds(1f);
        }

        bossAnimator.SetBool("valiammus", true);

        //fireDirection.y = -1;
        
        inFirePosition = false;

		yield return new WaitForSeconds(.8f);

		sounds.spell.Play();

        for (int i = 0; i < bulletAmount; i++)
        {
            //float dirToPlayerX = player.transform.position.x - transform.position.x;

            //print(dirToPlayerX);

            //GameObject bullet = Instantiate(bossBullet, bulletSpawner.position, Quaternion.identity);

            //bullet.GetComponent<Bullet>().Projectile(damage, bulletSpeed, fireDirection, bulletLifetime);
            fireDirection = player.transform.position - bulletSpawner.transform.position;

            //fireDirection.y += fireSegment;

            var dir = player.transform.position - bulletSpawner.position;
            dir.y = -1;

            for (int j = 0; j < 3; j++) { 
            GameObject bullet = Instantiate(bossBullet, bulletSpawner.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Projectile(damage, bulletSpeed, new Vector3(fireDirection.x, fireDirection.y - dir.y), bulletLifetime);
            dir.y += 1;
                

            }

            yield return new WaitForSeconds(.2f);

        }
        yield return new WaitForSeconds(.5f);

        attacksDone++;

        bossAnimator.SetBool("valiammus", false);

        if (attacksDone >= bulletAttacksInLoop)
        {
            SwitchPhase();
        }

        SwitchSide();

    }

    void Die()
    {
		dead = true;
		sounds.death.Play();
		inFirePosition = false;
		inBulletMode = false;
		StopCoroutine("BulletAttack");
		StopCoroutine("AreaAttack");
		bossAnimator.SetBool("velikuolee", true);
        print("bossi kuoli xD");
        gm.EndGame();
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
        bossAnimator.SetBool("jumpup", true);

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
		bossAnimator.SetBool("jumpDOWN", true);
        inFirePosition = true;
        
    }

    void DeadactivateAttackAreas()
    {
		lightning.Stop();
        foreach(var area in attackAreas)
        {
            area.GetComponent<BoxCollider2D>().enabled = false;
            area.GetComponent<AreaDamageScript>().enabled = false;
            
            area.SetActive(false);
        }
		bossAnimator.SetBool("velileijuu", false);
    }

    public void Reset()
    {
        health = maxHealth;
    }

}
