using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    int nextWave = 0;

    public float timeBetweenWaves = 5f;
    float waveCountDown;

    public Transform[] spawnPoints;

    float searchCountDown = 1f;

    SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountDown = timeBetweenWaves;
        if (spawnPoints.Length == 0)
            Debug.LogError("Error no spawn points");
    }

    private void Update()
    {
        // For checking if all enemies are killed in the spawn. then only next spawn is done
        if(state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive()) {
                WaveCompleted();
                return;
            }
            else
                return;
            
        }

        if(waveCountDown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }


    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;

        if(searchCountDown <= 0)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
            return true; 
    }


    void WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All walves completed");
        }
        else
            nextWave++;
    }

    IEnumerator SpawnWave( Wave _wave) {
        Debug.Log("Spawning wave " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++){
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }


    void SpawnEnemy( Transform _enemy)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}
