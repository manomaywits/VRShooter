using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform[] spawnPoints;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(15f);
        while (true)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[count].position, Quaternion.identity);
            count++;
            if (count >= spawnPoints.Length)
            {
                count = 0;
            }
            yield return new WaitForSeconds(Random.Range(2, 6));
        }
      
    }
}
