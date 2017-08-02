using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;

public class playerAI : MonoBehaviour {

    private Rigidbody2D rbd;
    public static playerAI player;

    public float dashTime = 0.1f;
    public float minDashSpeed = 10f;
    public float maxDashSpeed = 100f;

    public float chargedDashSpeed = 0;
    public Vector2 dashDir;
    Vector3 dir;
    public float dashTimer;
    public enum playerState {Charging,Dashing,Idle }
    public playerState myState;

    public static float timeSpeed= 1;
    public float chargeMinTimeSpeed = 0.1f;
    public float slowChargeMaxTime = 1f;
    public float dashChargeMaxTime = 1f;
    Vector3 pointPos;

    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += pointersPressedHandler;
            TouchManager.Instance.PointersUpdated += pointersUpdatedHandler;
            TouchManager.Instance.PointersReleased += pointersReleasedHandler;
        }
    }

    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed -= pointersPressedHandler;
            TouchManager.Instance.PointersReleased -= pointersReleasedHandler;
        }
    }

    // Use this for initialization
    void Start () {
    player = this;
        
        minDashSpeed = 30f;
        maxDashSpeed = 120f;
        dashTime = 0.1f;
        chargeMinTimeSpeed = 0.1f;

        dashChargeMaxTime = 3f;
        slowChargeMaxTime = 1f;
        

        dashTimer = 0;
        myState = playerState.Idle;
        rbd = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
        float h = pointPos.x;
        float v = pointPos.y;
        float angle = -Mathf.Atan2(h, v) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update () {

        switch (myState)
        {
            case playerState.Charging:
                if (chargedDashSpeed<maxDashSpeed)
                {
                    chargedDashSpeed +=  ((maxDashSpeed-minDashSpeed)/ dashChargeMaxTime) * Time.deltaTime;
                }/* else
                {
                    EnterState(playerState.Dashing);
                }*/
                if (timeSpeed>chargeMinTimeSpeed)
                {
                    timeSpeed = Mathf.Lerp(timeSpeed,chargeMinTimeSpeed, Time.deltaTime*4);
                } else
                {
                    timeSpeed = chargeMinTimeSpeed;
                }
                break;
            case playerState.Dashing:
                timeSpeed = Mathf.Lerp(timeSpeed, 1, Time.deltaTime * 4);
                if (dashTimer > 0)
                {
                    rbd.velocity = dashDir * chargedDashSpeed * timeSpeed;
                    dashTimer -= Time.deltaTime*timeSpeed;
                }
                else
                {
                    EnterState(playerState.Idle);
                }
                break;
            case playerState.Idle:
                    timeSpeed = Mathf.Lerp(timeSpeed, 1, Time.deltaTime * 4);
                break;
            default:
                break;
        }
        
	}

    private void pointersPressedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            EnterState(playerState.Charging);
        }
    }
    private void pointersReleasedHandler (object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            if (myState == playerState.Charging)
            {
                EnterState(playerState.Dashing);
            }
        }
    }
    private void pointersUpdatedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            pointPos = Camera.main.ScreenToWorldPoint(new Vector3(pointer.Position.x, pointer.Position.y, 10));
            dir = pointPos - transform.position;
            dashDir = dir;
            dashDir = dashDir.normalized;
        }
    }

    void ExitState()
    {
        switch (myState)
        {
            case playerState.Charging:
                break;
            case playerState.Dashing:
                break;
            case playerState.Idle:
                break;
            default:
                break;
        }
    }

    void EnterState(playerState stateEntered)
    {
        ExitState();
        switch (stateEntered)
        {
            case playerState.Charging:
                chargedDashSpeed = minDashSpeed;
                //rbd.velocity = Vector2.zero;
                myState = playerState.Charging;
                break;
            case playerState.Dashing:
                Debug.Log("Dash: " + chargedDashSpeed);
                dashTimer = dashTime;
                myState = playerState.Dashing;
                break;
            case playerState.Idle:
                rbd.velocity = Vector2.zero;
                myState = playerState.Idle;
                break;
            default: Debug.Log("Player State Machine changed to invalid enum");
                break;
        }
    }
}
