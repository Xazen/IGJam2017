using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
	public UnitType UnitType;
	public GameObject Prefab;
    public int MaxHealth;
    public Color OriginalColor;
	public Color OriginalEmissionColor;

	private MeshRenderer _renderer;

    private List<Rioter> _attackers = new List<Rioter>();
    private int _health;
    private bool _destroyed;

    public abstract void OnUnitSpawned();

	public void Start()
	{
		_renderer = GetComponentInChildren<MeshRenderer>();
	    _health = MaxHealth;
	}

    

    public GameObject GetModel()
	{
		return Prefab;
	}

	public void SetMaterialColor(Color materialColor)
	{
		_renderer.material.color = materialColor;
		_renderer.material.SetColor("_EmissionColor", materialColor);
	}

	public void ResetMaterial()
	{
		_renderer.material.color = OriginalColor;
		_renderer.material.SetColor("_EmissionColor", OriginalEmissionColor);
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
            DestroyUnit();
        }
    }

    public virtual void DestroyUnit()
    {
        foreach (var rioter in _attackers)
        {
            rioter.Continue();
        }
        _destroyed = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !_attackers.Contains(rioter) && !_destroyed)
        {
            _attackers.Add(rioter);
            rioter.Stop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !_destroyed)
        {
            GetDamage(rioter.TryAttack());
        }
    }
}
