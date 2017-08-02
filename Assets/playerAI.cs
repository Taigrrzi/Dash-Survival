﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;

public class playerAI : MonoBehaviour {

    private Rigidbody2D rbd;

    public float dashTime = 0.1f;
    public float dashSpeedInc = 80;
    public float minDashSpeed = 10f;
    public float maxDashSpeed = 100f;

    public float chargedDashSpeed = 0;
    public Vector2 dashDir;
    public float dashTimer;
    public enum playerState {Charging,Dashing,Idle }
    public playerState myState;

    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += pointersPressedHandler;
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
        dashSpeedInc = 80f;
        minDashSpeed = 20f;
        maxDashSpeed = 100f;
        dashTime = 0.1f;
        
         
        dashTimer = 0;
        myState = playerState.Idle;
        rbd = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (myState)
        {
            case playerState.Charging:
                if (chargedDashSpeed<maxDashSpeed)
                {
                    chargedDashSpeed += dashSpeedInc * Time.deltaTime;
                } else
                {
                    EnterState(playerState.Dashing);
                }
                break;
            case playerState.Dashing:
                if (dashTimer > 0)
                {
                    rbd.velocity = dashDir * chargedDashSpeed;
                    dashTimer -= Time.deltaTime;
                }
                else
                {
                    EnterState(playerState.Idle);
                }
                break;
            case playerState.Idle:
                break;
            default:
                break;
        }
        
	}

    private void pointersPressedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint(new Vector3(pointer.Position.x, pointer.Position.y, 10)) - transform.position;
            dashDir = dir;
            dashDir = dashDir.normalized;
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

    void EnterState(playerState stateEntered)
    {
        switch (stateEntered)
        {
            case playerState.Charging:

                chargedDashSpeed = minDashSpeed;
                rbd.velocity = Vector2.zero;
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