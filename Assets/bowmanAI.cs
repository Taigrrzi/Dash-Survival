using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowmanAI : enemyAI {

    public float moveSpeed = 4;
    public float shotRange = 12;
    public float shotDelay = 2;
    public float shotSpeed = 30;
    public float shotTimer = 0;
    public float shotDamage = 10;

    public GameObject arrow;
    private Rigidbody2D rbd;
    public Vector2 dir;
    public enum AiState {Charging,Shooting,Approaching}

    public AiState myState;

	// Use this for initialization
	void Start () {
        moveSpeed = 4;
        shotRange = 7;
        shotDelay = 2;
        shotSpeed = 15;
        shotDamage = 10;
        rbd = GetComponent<Rigidbody2D>();
        myState = AiState.Approaching;
	}
	
	// Update is called once per frame
	void Update () {
        switch (myState)
        {
            case AiState.Charging:
                if (Vector3.Distance(transform.position, playerAI.player.transform.position) > shotRange)
                {
                    EnterState(AiState.Approaching);
                }
                if (shotTimer>0)
                {
                    shotTimer -= Time.deltaTime * playerAI.timeSpeed;
                } else
                {
                    EnterState(AiState.Shooting);
                }
                break;
            case AiState.Shooting:
                break;
            case AiState.Approaching:
                if (Vector3.Distance(transform.position, playerAI.player.transform.position) > shotRange)
                {
                    Vector3 tempDir = playerAI.player.transform.position - transform.position;
                    dir = tempDir;
                    dir = dir.normalized;
                    rbd.velocity = dir*moveSpeed*playerAI.timeSpeed;
                } else
                {
                    EnterState(AiState.Charging);
                }
                break;
            default:
                break;
        }
    }

    void ExitState ()
    {
        switch (myState)
        {
            case AiState.Charging:
                break;
            case AiState.Shooting:
                //Debug.Log
                shotTimer = shotDelay;
                break;
            case AiState.Approaching:
                break;
            default:
                break;
        }
    }

    void EnterState (AiState enteredState)
    {
        ExitState();
        switch (enteredState)
        {
            case AiState.Charging:
                myState = AiState.Charging;
                rbd.velocity = Vector2.zero;
                break;
            case AiState.Shooting:
                shotTimer = 0;
                GameObject newArrow = Instantiate<GameObject>(arrow,transform.position,Quaternion.identity);
                newArrow.GetComponent<projectile>().Create(shotSpeed, shotDamage, true, false,playerAI.player.gameObject);
                rbd.velocity = Vector2.zero;
                myState = AiState.Shooting;
                EnterState(AiState.Charging);
                break;
            case AiState.Approaching:
                myState = AiState.Approaching;
                break;
            default:
                break;
        }
    }
}
