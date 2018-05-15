using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTarget : MonoBehaviour {

    public GameObject spawnMonster;
    public float spawnDelay;
    float timer;
    Vector3 startScale;
	// Use this for initialization
	void Start () {
        timer = 0;
        startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer>spawnDelay)
        {
            Instantiate<GameObject>(spawnMonster, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        transform.localScale = startScale * (spawnDelay / timer);
		
	}
}
