using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    private DiContainer _diContainer;

    [Inject]
    public void Inject(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }
    
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(Enemy, gameObject.transform.position, Quaternion.identity);
        _diContainer.InjectGameObject(enemy);
        enemy.GetComponent<PathfinderAgent>().CalculatePath(new Vector3(19, 0, 15));
    }
}