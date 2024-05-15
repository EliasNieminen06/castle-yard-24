using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
        int waveValue = currentWave * 10;
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
            }

            if (currentWave == enemy.lastWave + 1 && enemySpawnPool.Contains(enemy))
            {
                enemySpawnPool.Remove(enemy);
            }
        }

        int failuresToFindEnemyMax = 5;
        List<GameObject> randomEnemies = new List<GameObject>();

        while (costLeft > 0)
        {
            int randomEnemyIndex = Random.Range(0, enemySpawnPool.Count);
            int randomEnemyCost = enemySpawnPool[randomEnemyIndex].cost;

            if (randomEnemyCost <= costLeft)
            {
                randomEnemies.Add(enemySpawnPool[randomEnemyIndex].enemyPrefab);
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

  //private UpgradeItem GetRandomEnemy(List<GameObject> listToAddTo)
  //{
  //    Dictionary<EnemySpawnData, ValueTuple<int, int>> itemDictionary = new Dictionary<EnemySpawnData, ValueTuple<int, int>>();
  //    int totalWeight = 0;
  //
  //    foreach (var upgradeItem in itemDrawPool)
  //    {
  //        ValueTuple<int, int> tuple = (totalWeight, 0);
  //        totalWeight += upgradeItem.weight;
  //        tuple.Item2 = totalWeight - 1;
  //        itemDictionary.Add(upgradeItem, tuple);
  //    }
  //
  //    int randomWeight = UnityEngine.Random.Range(1, totalWeight);
  //    //Debug.Log("Trying to get upgrade with a weight of: " + randomWeight);
  //    UpgradeItem item = GetUpgradeWithWeight(randomWeight);
  //
  //    while (listToAddTo.Contains(item))
  //    {
  //        randomWeight = UnityEngine.Random.Range(0, totalWeight);
  //        //Debug.Log("Trying to get upgrade with a weight of: " + randomWeight);
  //        item = GetUpgradeWithWeight(randomWeight);
  //        if (listToAddTo.Count == itemDrawPool.Count) break;
  //    }
  //
  //    //Debug.Log("Got Upgrade: " + item);
  //    return item;
  //
  //    UpgradeItem GetUpgradeWithWeight(int weight)
  //    {
  //        foreach (var upgradeItem in itemDrawPool)
  //        {
  //            ValueTuple<int, int> rangeTuple = (0, 0);
  //            if (itemDictionary.TryGetValue(upgradeItem, out rangeTuple) == false)
  //            {
  //                throw new IndexOutOfRangeException("Upgrade not in Dictionary");
  //            }
  //
  //            if (rangeTuple.Item1 <= weight && rangeTuple.Item2 >= weight)
  //            {
  //                //Debug.Log(rangeTuple + " " + weight);
  //                return upgradeItem;
  //            }
  //        }
  //
  //        throw new ArgumentOutOfRangeException("Did not find an upgrade with given weight");
  //    }
  //}

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity); ;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float distance = Random.Range(7.5f, 20f);
        Vector3 direction = new Vector3(Random.rotation.x, 0, Random.rotation.z).normalized;
        Vector3 playerPosition = GameManager.Instance.Player.position;
        Vector3 spawnPosition = playerPosition + direction * distance;
        return spawnPosition;
    }


    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public int cost;
        public int weight;

        public int firstWave;
        public int lastWave;
    }
}
