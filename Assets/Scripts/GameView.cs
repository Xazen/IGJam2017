using UnityEngine;
using Zenject;

public class GameView : MonoBehaviour
{
    public GameObject EnemySpawnerPrefab;
    public EnemySpawner EnemySpawner;
    
    [Inject]
    public void Inject(GameController gameController)
    {
        gameController.OnEnemySpawn += OnEnemySpawn;
    }

    private void OnEnemySpawn(int enemycount)
    {
        
    }
}
