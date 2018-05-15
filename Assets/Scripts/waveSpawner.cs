using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner : MonoBehaviour {

    public GameObject[] world1Monsters ;
    public int[] world1MonsterDifficulty;
    public float waveDuration;
    public int initialDifficulty;
    public int currentDifficulty;
    public int difficultyInc;
    public List<GameObject> currentMonsters;
    public float baseSpawnDelay;
    public float spawnTimer;
    public static int waveNumber;
    public GameObject spawnerTarget ;
    public float spawnTargetDelay;

    public float waveTimer;
    
	// Use this for initialization
	void Start () {
        currentDifficulty = initialDifficulty;
        waveTimer = 5;
        waveNumber = 0;
	}

    // Update is called once per frame
    void Update() {
        waveTimer -= Time.deltaTime;
        if (waveTimer < 0)
        {
            SpawnNextWave();
            waveTimer = waveDuration;
            waveNumber++;
        }

        if (currentMonsters.Count>0)
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                GameObject nextMonster = currentMonsters[0];
                currentMonsters.RemoveAt(0);
                SpawnMonster(nextMonster);
                spawnTimer = baseSpawnDelay;
            }
        }

    }

    public void SpawnMonster(GameObject monsterType)
    {
        /*float x, y;
        do
        {
            x = (Random.value * (24)) - 12;
            y = (Random.value * (18)) - 8;
        } while ((x > -8 && x < 8) && (y > -5 || y < 5));*/
        GameObject target = Instantiate<GameObject>(spawnerTarget, new Vector3(Random.value * (16) - 8, Random.value * (10) - 5, 10), Quaternion.identity);
        target.GetComponent<spawnTarget>().spawnDelay = spawnTargetDelay;
        target.GetComponent<spawnTarget>().spawnMonster = monsterType;

    }

    public void SpawnNextWave()
    {
        waveTimer = waveDuration;
        PlanWave(world1Monsters, world1MonsterDifficulty, currentDifficulty);
        currentDifficulty += difficultyInc;
        baseSpawnDelay = waveDuration / currentMonsters.Count;
    }

    public void PlanWave(GameObject[] monsterPool,int[] monsterPoolDif,int totalDifficulty)
    {
        int currentDifficulty = 0;
        int monsterCount = 0;
        while (currentDifficulty<totalDifficulty)
        {
            int rand = Mathf.FloorToInt(Random.Range(0, monsterPool.Length));
            currentMonsters.Add(monsterPool[rand]);
            currentDifficulty += monsterPoolDif[rand];
            monsterCount++;
        }
    }
}
