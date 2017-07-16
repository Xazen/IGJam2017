using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
    protected bool Destroyed;
	private bool _isSpawned;
	private GameController _gameController;

	public abstract void OnUnitSpawned();

	[Inject]
	public void Inject(GameController gameController)
	{
		_gameController = gameController;
	}
	
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

	public void SpawnUnit()
	{
		_isSpawned = true;
		ResetMaterial();
	}
	
	private void ResetMaterial()
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
	    StartCoroutine(BlinkDamange());
        //_slider.value = _health;
        if (_health <= 0)
        {
            DestroyUnit();
        }
    }
	
	private IEnumerator BlinkDamange()
	{
		Color originalColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_EmissionColor");
		GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
		yield return new WaitForEndOfFrame();
		GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", originalColor);    
	}

    public virtual void DestroyUnit()
    {
        Destroyed = true;
        foreach (var rioter in _attackers)
        {
            rioter.Continue();
        }
	    switch (UnitType)
	    {
		    case UnitType.Police:
			    _gameController.WoundedPolicemenCounter++;
			    break;
		    case UnitType.Barrier:
			    _gameController.CasualtyCounter += 1280;
			    break;
		    default:
			    throw new ArgumentOutOfRangeException();
	    }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !_attackers.Contains(rioter) && !Destroyed && _isSpawned)
        {
            _attackers.Add(rioter);
            rioter.Stop();
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        Rioter rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !Destroyed)
        {
            GetDamage(rioter.TryAttack());
        }
    }
}
