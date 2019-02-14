using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count; // number of enemies we want to spawn
        public float rate; // spawn rate
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave { get { return nextWave + 1; } }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f; // in seconds
    private float waveCountDown;
    public float WaveCountDown { get { return waveCountDown; } }

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State { get { return state; } }

    private float searchCountdDown = 1f; // search for enemy every x seconds

    // Start is called before the first frame update
    void Start()
    {
        waveCountDown = timeBetweenWaves;

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("enemy wave spawner: No spawn points");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                // start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else {
            waveCountDown -= Time.deltaTime;
        }
    }

    void WaveCompleted() {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves complete - looping ...");
        }
        else {
            nextWave++;
        }


    }

    bool EnemyIsAlive() {

        searchCountdDown -= Time.deltaTime;

        if (searchCountdDown <= 0f) {
            searchCountdDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave) {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds( 1 / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy) {
        Debug.Log("Spawning enemy " + _enemy.name);

        if (spawnPoints.Length > 0)
        {
            Transform _sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(_enemy, _sp.position, _sp.rotation);
        }
        else {
            Debug.LogError("enemy wave spawner: No spawn points");
        }
    }
}
