using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using Zenject;

public abstract class Unit : MonoBehaviour
{
	public UnitType UnitType;
	public GameObject Prefab;
    public int MaxHealth;
    public Color OriginalColor;
	public Color OriginalEmissionColor;
	public Animator Animator;
	public SpriteHealtbar SpriteHealtbar;

	private MeshRenderer _renderer;

    private List<Rioter> _attackers = new List<Rioter>();
    private int _health;
    protected bool Destroyed;
	private bool _isSpawned;
	private GameController _gameController;
    private GridController _gridController;

	public abstract void OnUnitSpawned();

	[Inject]
	public void Inject(GameController gameController, GridController gridController)
	{
		_gameController = gameController;
	    _gridController = gridController;
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
		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < meshRenderers.Length; i++)
		{
			meshRenderers[i].material.color = materialColor;
			meshRenderers[i].material.SetColor("_EmissionColor", materialColor);
		}
	}

	public void SpawnUnit()
	{

        _gridController.WorldToCell(transform.position).Type = CellType.Building;
		_isSpawned = true;
		ResetMaterial();
	}

	public bool IsSpawned()
	{
		return _isSpawned;
	}
	
	private void ResetMaterial()
	{
		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < meshRenderers.Length; i++)
		{
			meshRenderers[i].material.color = OriginalColor;
			meshRenderers[i].material.SetColor("_EmissionColor", OriginalEmissionColor);
		}
	}

    public void GetDamage(int damage)
    {
	    if (!_isSpawned)
	    {
		    return;
	    }
        //if (_slider == null) {
        //    _slider = Instantiate(HealthSlider,transform.position,Quaternion.identity,transform).GetComponentInChildren<Slider>();
        //    _slider.maxValue = MaxHealth;
        //}
        _health -= damage;
	    SpriteHealtbar.SetHealth((float) _health / MaxHealth);
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

		MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < meshRenderers.Length; i++)
		{
			meshRenderers[i].material.SetColor("_EmissionColor", Color.red);
		}
		yield return new WaitForEndOfFrame();
        
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < meshRenderers.Length; i++)
		{
			meshRenderers[i].material.SetColor("_EmissionColor", originalColor);
		}
	}

    public virtual void DestroyUnit()
    {
        Destroyed = true;
	    _isSpawned = false;
        _gridController.WorldToCell(transform.position).Type = CellType.Street;
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
	    StartCoroutine(PlayDestroyUnit());
    }

	private IEnumerator PlayDestroyUnit()
	{
		if (Animator != null && UnitType == UnitType.Barrier)
		{
			Animator.SetTrigger("Dissolve");
			yield return new WaitForSeconds(2);
			if (gameObject)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
    {

        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !_attackers.Contains(rioter) && !Destroyed)
        {
	        if (IsSpawned())
	        {
            	_attackers.Add(rioter);
		        rioter.Stop();    
	        }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {   
        Rioter rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !Destroyed)
        {
	        if (!_isSpawned)
	        {
		        return;
	        }
            GetDamage(rioter.TryAttack());
        }
    }
}
