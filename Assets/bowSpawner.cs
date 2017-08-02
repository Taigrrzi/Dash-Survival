using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowSpawner : MonoBehaviour {

    public float timeLasted;
    public GameObject bowMan;
    public float basechance;
	// Use this for initialization
	void Start () {
        timeLasted = 0;
        basechance = 0.008f;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Random.value<basechance)
        {
            SpawnBow();
        }
	}

    public void SpawnBow()
    {
        float x, y;
        do
        {
            x = (Random.value * (24)) -12;
            y = (Random.value * (18)) - 8;
        } while ((x>8||x<-8)&&(y>5||y<-5));
        Instantiate<GameObject>(bowMan,new Vector3(x,y,10),Quaternion.identity);
    }
}
