using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameView : MonoBehaviour
{
    private const float SpawnFrequency = 0.3f;
    
    [Header("Buildings")] 
    public Transform[] StartingPositions;
    public List<GameObject> ShopList;
    public GameObject CongressCenter;
    
    [Header("Prefabs")]
    public GameObject EnemySpawnerPrefab;
    
    [Header("End View")]
    public GameObject EndView;
    
    private GameController _gameController;
    private List<EnemySpawner> _enemySpawners;
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
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        List<Vector3> startingPointPositions = new List<Vector3>();
        for (int i = 0; i < StartingPositions.Length; i++)
        {
            startingPointPositions.Add(StartingPositions[i].transform.position);
        }
        _gameController.Setup(
             startingPointPositions.ToArray(),
             CongressCenter.transform.position,
             EndView);
        
        _enemySpawners = new List<EnemySpawner>();

        for (int i = 0; i < startingPointPositions.Count; i++)
        {
            Vector2 startingPos = _gameController.StartingPoint[i];
            GameObject enemySpawner = Instantiate(EnemySpawnerPrefab, new Vector3(startingPos.x, 0, startingPos.y), Quaternion.identity);
            _enemySpawners.Add(enemySpawner.GetComponent<EnemySpawner>());
            _diContainer.InjectGameObject(enemySpawner);
        }
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
            int index = Random.Range(0, Mathf.Min(_enemySpawners.Count, _gameController.GetCurrentGameRound()));
            _enemySpawners[index].SpawnEnemy();
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
