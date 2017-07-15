using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class Building : MonoBehaviour
{

    public int MaxHealth;
    public Mesh[] BuildingModels;
    public GameObject HealthSlider;

    public int BuildingValue;

    private Slider _slider;
    private int _health;

    protected GameController _gameController;

    void Awake()
    {
        _health = MaxHealth;
    }

    [Inject]
    public void Inject(GameController gc)
    {
        _gameController = gc;
    }

    public void GetDamage(int damage)
    {
        if(damage > 0)
            _gameController.RegisterCasualty(CalculateCasualty(damage));
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

    private int CalculateCasualty(int dmg)
    {
        return (BuildingValue / MaxHealth) * dmg;
    }
   
}
