using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [System.Serializable]
    public class ItemWave
    {
        public string name;
        public Transform[] item;
        public int count;                               // item count
        public float spawnRate;
    }
    // -----------------------------------

    public ItemWave[] itemWave;                         // array of ItemWaves to spawn
    private int nextWave = 0;                           // index to choose which wave to spawn

    public Transform[] spawnPoints;                     // array of Spawnpoints

    [Header("Wave Parameters")]
    public float timeBetweenWaves = 15.0f;
    public float waveCountdown;

    private float searchCountdown = 1.0f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        // check if still items on Map
        if (state == SpawnState.WAITING)
        {
            if (!ItemOnMap()) // start new Wave etc
            {
                WaveCompleted();
            }
            else // still enemies alive so return
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(itemWave[nextWave]));
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
        if (nextWave + 1 > itemWave.Length - 1)
        {
            nextWave = 0;
            // If Game completed DO IT HERE! Or Multiplier to Enemy States etc.
        }
        else
        {
            nextWave++;
        }

    }

    bool ItemOnMap()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0.0f)
        {
            searchCountdown = 1.0f; // reset countdown
            if (GameObject.FindGameObjectsWithTag("Item").Length == 0)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(ItemWave _wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            int tmpRndNn = Random.Range(0, 2);
            SpawnItem(_wave.item[tmpRndNn]);
            yield return new WaitForSeconds(1.0f / _wave.spawnRate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnItem(Transform _item)
    {
        Transform _spawnPoints = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_item, _spawnPoints.transform.position, _spawnPoints.transform.rotation);
    }
}