using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    [SerializeField] private List<EnemySpawnData> enemySpawnDatas;
    private List<EnemySpawnData> enemySpawnPool = new List<EnemySpawnData>();
    private List<GameObject> enemiesToSpawn = new List<GameObject>();

    private Dictionary<EnemySpawnData, ValueTuple<int, int>> enemyDataDictionary = new Dictionary<EnemySpawnData, ValueTuple<int, int>>();
    private int spawnPoolTotalWeight;

    public int currentWave { get; private set; }

    [SerializeField] private float waveDuration;

    private float waveTimer;
    private float spawnTimer;
    private float spawnInterval;

    private void FixedUpdate()
    {
        if (enemiesToSpawn.Count > 0)
        {
            if (spawnTimer <= 0)
            {
                SpawnEnemy(enemiesToSpawn[0]);
                enemiesToSpawn.RemoveAt(0);
                spawnTimer = spawnInterval + spawnTimer;
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;

            }
        }
        else
        {
            waveTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0)
        {
            GenerateWave();
        }
    }

    private void GenerateWave()
    {
        currentWave++;
        int waveValue = currentWave * 3;

        if (currentWave < 4) waveValue += currentWave;

        ReWriteEnemyDataDictionary();
        GenerateEnemies(waveValue);

        spawnInterval = waveDuration / enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }

    private void GenerateEnemies(int costLeft)
    {
        foreach (var enemy in enemySpawnDatas)
        {
            if (currentWave == enemy.firstWave)
            {
                enemySpawnPool.Add(enemy);
                ReWriteEnemyDataDictionary();
            }

            if (currentWave == enemy.lastWave + 1 && enemySpawnPool.Contains(enemy))
            {
                enemySpawnPool.Remove(enemy);
                ReWriteEnemyDataDictionary();
            }
        }

        int failuresToFindEnemyMax = 5;
        List<GameObject> randomEnemies = new List<GameObject>();

        while (costLeft > 0)
        {
            EnemySpawnData randomEnemy = GetRandomEnemyData();
            int randomEnemyCost = randomEnemy.cost;

            if (randomEnemyCost <= costLeft)
            {
                randomEnemies.Add(randomEnemy.enemyPrefab);
                costLeft -= randomEnemyCost;
            }
            else
            {
                failuresToFindEnemyMax--;
                if (failuresToFindEnemyMax == 0) break;
            }
        }

        enemiesToSpawn.Clear();

        foreach (var enemy in randomEnemies)
        {
            enemiesToSpawn.Add(enemy);
        }
    }

  private EnemySpawnData GetRandomEnemyData()
  {
      int randomWeight = UnityEngine.Random.Range(0, spawnPoolTotalWeight);
      EnemySpawnData enemyData = GetEnemyWithWeight(randomWeight);

      return enemyData;
  
      EnemySpawnData GetEnemyWithWeight(int weight)
      {
          foreach (var enemy in enemySpawnPool)
          {
              ValueTuple<int, int> rangeTuple = (0, 0);
              if (enemyDataDictionary.TryGetValue(enemy, out rangeTuple) == false)
              {
                  throw new IndexOutOfRangeException("EnemyData not in Dictionary");
              }
  
              if (rangeTuple.Item1 <= weight && rangeTuple.Item2 > weight)
              {
                  //Debug.Log(rangeTuple + " " + weight);
                  return enemy;
              }
          }
  
          throw new ArgumentOutOfRangeException("Did not find an EnemyData with given weight");
      }
  }

    private void ReWriteEnemyDataDictionary()
    {
        int totalWeight = 0;
        enemyDataDictionary.Clear();

        foreach (var enemy in enemySpawnPool)
        {
            if (enemy.weight + enemy.weightAddedPerWave * currentWave <= 0)
            {
                enemySpawnPool.Remove(enemy);
                ReWriteEnemyDataDictionary();
                return; ;
            }

            ValueTuple<int, int> tuple = (totalWeight, 0);
            totalWeight += enemy.weight + enemy.weightAddedPerWave * currentWave;
            tuple.Item2 = totalWeight;
            enemyDataDictionary.Add(enemy, tuple);
        }

        spawnPoolTotalWeight = totalWeight;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float distance = UnityEngine.Random.Range(7.5f, 20f);
        Vector3 direction = new Vector3(UnityEngine.Random.rotation.x, 0, UnityEngine.Random.rotation.z).normalized;
        Vector3 playerPosition = GameManager.Instance.Player.position;
        Vector3 spawnPosition = playerPosition + direction * distance;
        spawnPosition.y = 0;

        NavMeshHit hit;
        NavMesh.SamplePosition(spawnPosition, out hit, 100, -1); 

        return hit.position;
    }


    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public int cost;
        public int weight;
        public int weightAddedPerWave;

        public int firstWave;
        public int lastWave;
    }
}
