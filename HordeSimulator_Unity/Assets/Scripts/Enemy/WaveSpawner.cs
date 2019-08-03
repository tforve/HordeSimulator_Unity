using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState{SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;           // enemy count
        public float spawnRate;
    }
    // -----------------------------------

    public Wave[] wave;                                 // array of EnemyWaves to spawn
    private int nextWave = 0;                           // index to choose which wave to spawn

    public Transform[] spawnPoints;                     // array of Spawnpoints

    [Header("Wave Parameters")]
    public float timeBetweenWaves = 5.0f;
    public float waveCountdown;

    private float searchCountdown = 1.0f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        // check if still enemies alive
        if(state == SpawnState.WAITING)
        {
            if(!EnemyIsAlive()) // start new Wave etc
            {
                WaveCompleted();
            }
            else // still enemies alive so return
            {
                return;
            }
        }

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(wave[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }
    
    //reset Wave
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        //looping if wave Array finised, Completed all waves
        if(nextWave +1 > wave.Length -1)
        {
            nextWave = 0;
            // If Game completed DO IT HERE! Or Multiplier to Enemy States etc.
        }
        else
        {
            nextWave++;
        }

    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0.0f)
        {
            searchCountdown = 1.0f; // reset countdown
            if(GameObject.FindGameObjectsWithTag("Enemy") == null)
            {
                return false;
            }
        }
        
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave:" + _wave.name);
        state = SpawnState.SPAWNING;
        
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1.0f / _wave.spawnRate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemies:" + _enemy.name);
        Transform _spawnPoints = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _spawnPoints.transform.position, _spawnPoints.transform.rotation);
    }
}
