using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public enum EnemyType { Grounded, Flying }

public class Enemy : MonoBehaviour {


	// TODO: Figure out if we need a generic enemy class?


//    GameObject player;
//    Vector3 startPosition;

	void Start () {
//        player = GameObject.FindGameObjectWithTag("Player");
//        startPosition = transform.position;
    }
    
    void Update () {
        Move();

    }

    void Move() {
//        if (stats.health > 0) {
//            canShoot = CheckDistanceX(transform.position, player.transform.position, stats.shootingDistance);
//            Vector2 offset = transform.position - player.transform.position;
//            Vector2 startPosOffset = transform.position - startPosition;
//            if (enemyType == EnemyType.Grounded) {
//                if (CheckDistanceX(startPosition, player.transform.position, stats.chaseDistance)) {
//                    if (canShoot && Mathf.Abs(offset.x) > stats.shootingDistance / 2) {
//                        transform.Translate(Vector2.left * Time.deltaTime * stats.moveSpeed / 2);
//                        anim.SetBool("munaanimation", true);
//                    } else if (canShoot) {
//                        anim.SetBool("munaanimation", false);
//                    } else {
//                        transform.Translate(Vector2.left * Time.deltaTime * stats.moveSpeed);
//                        anim.SetBool("munaanimation", true);
//                    }
//                } else if (Mathf.Abs(startPosOffset.x) > stats.chaseDistance / 3 && !canShoot) {
//                    transform.Translate(Vector2.right * Time.deltaTime * stats.moveSpeed);
//                    anim.SetBool("munaanimation", false);
//                }
//            }
//        }
    }

}
