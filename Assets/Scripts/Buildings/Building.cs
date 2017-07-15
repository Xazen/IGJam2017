using UnityEngine;

public abstract class Building : MonoBehaviour
{

    public int MaxHealth;
    public Mesh[] BuildingModels;

    private int _health;

    void Awake()
    {
        _health = MaxHealth;
    }

    public void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DestroyBuilding();
        }
    }

    public virtual void DestroyBuilding()
    {
        Destroy(gameObject);
    }

   
}
