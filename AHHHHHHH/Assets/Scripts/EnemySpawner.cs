using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] EnemySpawns;
    
    [Header("Waves")]
    public int WaveNumber = 0;
    public int AddWave = 1;
    public float spawnRate = 5f;
    [Header("Enemies")]
    public bool canSpawn = false;
    public float BaseEnemies = 4;
    public float MaxEnemies = 0;
    public float CurrentEnemies = 0;
    public float EnemySpeed = 3;
    public GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        WaveNumber += AddWave;
        print(WaveNumber);
        StartCoroutine("SpawnRate");
    }

    // Update is called once per frame
    void Update()
    {
        MaxEnemies = (BaseEnemies * WaveNumber);

        if (CurrentEnemies < MaxEnemies && canSpawn)
        {
            Transform selectedSpawner = EnemySpawns[Random.Range(0, 4)];


            GameObject e = Instantiate(Enemy, selectedSpawner.position, selectedSpawner.rotation);
            CurrentEnemies += 1;
            MaxEnemies -= 1;
            canSpawn = false;
            StartCoroutine("SpawnRate");
        }
    }
    IEnumerator SpawnRate()
    {
        
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
    }
}
