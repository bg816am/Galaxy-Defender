using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float _gameCeiling = 8f;
    private bool _stopSpawning = false;
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] private GameObject[] powerUps = default;
    [SerializeField] GameObject enemyContainer = default;
    [SerializeField] private GameObject enemyPrefab = default;
    [SerializeField] private float enemyDelay = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }
    
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnDelay);
        while (_stopSpawning == false)
        {
            Vector3 spawnEnemyPosition = new Vector3(Random.Range(-8f,8f),_gameCeiling,0);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnEnemyPosition, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(enemyDelay);
        }
    }
    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(spawnDelay);
        while (_stopSpawning == false)
        {
            Vector3 spawnPowerUpPosition = new Vector3(Random.Range(-9.5f,9.5f), _gameCeiling,0);
            float randomX = Random.Range(2f, 9f);
            int randomPowerUp = Random.Range(0, 4);
            Instantiate(powerUps[randomPowerUp], spawnPowerUpPosition, Quaternion.identity);
            yield return new WaitForSeconds(randomX);
        }
        
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
    