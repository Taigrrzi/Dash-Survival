using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kamikaziAI : enemyAI
{

    public float moveSpeed = 4;
    public int damage = 20;
    
    private Rigidbody2D rbd;
    public Vector2 dir;
    
    // Use this for initialization
    void Start()
    {
        moveSpeed = 4;
        rbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempDir = playerAI.player.transform.position - transform.position;
        dir = tempDir;
        dir = dir.normalized;
        rbd.velocity = dir * moveSpeed * playerAI.timeSpeed;    
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                if (playerAI.player.myState != playerAI.playerState.Dashing)
                {
                    playerAI.player.currentHealth -= damage;
                }
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
