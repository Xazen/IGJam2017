using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameView : MonoBehaviour
{
    private const float SpawnFrequency = 0.1f;
    
    [Header("Buildings")] 
    public Transform StartingPosition;
    public List<GameObject> ShopList;
    public GameObject CongressCenter;
    
    [Header("Prefabs")]
    public GameObject EnemySpawnerPrefab;
    
    private GameController _gameController;
    private EnemySpawner _enemySpawner;
    private DiContainer _diContainer;

    [Inject]
    public void Inject(GameController gameController, DiContainer diContainer)
    {
        _diContainer = diContainer;
        _gameController = gameController;
        _gameController.OnEnemySpawn += OnEnemySpawn;
    }

    public void Start()
    {
        GameObject enemySpawner = Instantiate(EnemySpawnerPrefab, new Vector3(_gameController.StartingPoint.x, 0, _gameController.StartingPoint.y), Quaternion.identity);
        _enemySpawner = enemySpawner.GetComponent<EnemySpawner>();
        _diContainer.InjectGameObject(enemySpawner);
        _gameController.StartGame();
    }

    private void OnEnemySpawn(int enemyCount)
    {
        StartCoroutine(SpawnEnemiesWithInterval(enemyCount, SpawnFrequency));
    }

    private IEnumerator SpawnEnemiesWithInterval(int enemyCount, float spawnFrequency)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            _enemySpawner.SpawnEnemy();
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
