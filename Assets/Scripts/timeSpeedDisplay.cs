﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeSpeedDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = "" + waveSpawner.waveNumber;
       // GetComponent<Text>().text = ""+playerAI.timeSpeed;
    }
}
