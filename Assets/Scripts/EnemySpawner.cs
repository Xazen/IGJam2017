using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    private DiContainer _diContainer;
    private GameController _gameController;

    [Inject]
    public void Inject(DiContainer diContainer, GameController gameController)
    {
        _gameController = gameController;
        _diContainer = diContainer;
    }
    
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(Enemy, gameObject.transform.position, Quaternion.identity);
        _diContainer.InjectGameObject(enemy);

        var enemyBhv = enemy.GetComponent<Rioter>();

        enemyBhv.MoveTo(new Vector3(_gameController.TargetPoint.x, 0, _gameController.TargetPoint.y));
    }
}