using UnityEngine;
using UnityEngine.UI;

public abstract class Building : MonoBehaviour
{

    public int MaxHealth;
    public Mesh[] BuildingModels;
    public GameObject HealthSlider;

    private Slider _slider;
    private int _health;

    void Awake()
    {
        _health = MaxHealth;
    }

    public void GetDamage(int damage)
    {
        //if (_slider == null) {
        //    _slider = Instantiate(HealthSlider,transform.position,Quaternion.identity,transform).GetComponentInChildren<Slider>();
        //    _slider.maxValue = MaxHealth;
        //}
        _health -= damage;
        //_slider.value = _health;
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
