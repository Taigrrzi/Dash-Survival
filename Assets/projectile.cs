using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    public float shotSpeed;
    public float shotDamage;
    public bool canHitPlayer;
    public bool canHitEnemy;
    public GameObject target;

    public Vector2 dir;
    public Rigidbody2D rbd;

	// Use this for initialization
	void Start () {
        rbd = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        rbd.velocity = dir * shotSpeed * playerAI.timeSpeed;
	}

    public void Create(float sSpeed,float sDamage,bool hitPlayer, bool hitEnemy,GameObject tar)
    {
        shotSpeed = sSpeed;
        shotDamage = sDamage;
        canHitEnemy = hitEnemy;
        canHitPlayer = hitPlayer;
        target = tar;


        Vector3 tempDir = target.transform.position - transform.position;
        dir = tempDir;
        dir = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            switch (other.tag)
            {
                case "Player":
                    playerAI.player.currentHealth -= shotDamage;
                    Destroy(gameObject);
                    break;
                case "Enemy":
                    //Destroy(gameObject);
                    break;
                case "Wall":
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
