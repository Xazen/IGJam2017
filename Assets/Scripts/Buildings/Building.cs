﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class Building : MonoBehaviour
{
    public int MaxHealth;
    public Mesh[] BuildingModels;
    public SpriteHealtbar SpriteHealtbar;
    public GameObject DamagedParticle;
    public GameObject InitialDamageParticle;
    public GameObject DestroyedParticle;

    public int BuildingValue;

    private Slider _slider;
    private int _health;

    protected GameController _gameController;
    protected bool _destroyed;
    private GameObject _currentParticle;
    private bool _playedInitialParticle;
    private Color _originalColor;

    void Awake()
    {
        _health = MaxHealth;
        _originalColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_EmissionColor");
    }

    [Inject]
    public void Inject(GameController gc)
    {
        _gameController = gc;
    }

    public void GetDamage(int damage)
    {
        if (damage > 0)
        {
            _gameController.RegisterCasualty(CalculateCasualty(damage));
            if (_health > 0)
            {
                StartCoroutine(BlinkDamange());
            }
        }
            
        //if (_slider == null) {
        //    _slider = Instantiate(HealthSlider,transform.position,Quaternion.identity,transform).GetComponentInChildren<Slider>();
        //    _slider.maxValue = MaxHealth;
        //}

        if (_health == MaxHealth &&
            _health > 0 &&
            damage > 0 && 
            _currentParticle == null)
        {
            _currentParticle = Instantiate(
                DamagedParticle, 
                new Vector3(
                    transform.position.x, 
                    transform.position.y + 0.5f, 
                    transform.position.z), 
                Quaternion.identity);
            
            Instantiate(
                InitialDamageParticle, 
                new Vector3(
                    transform.position.x, 
                    transform.position.y + 1f, 
                    transform.position.z), 
                Quaternion.identity);

            _playedInitialParticle = true;
        }

        
        _health -= damage;

        if (SpriteHealtbar != null)
        {
            SpriteHealtbar.SetHealth((float) _health / MaxHealth);
        }
        
        //_slider.value = _health;
        if (_health <= 0)
        {
            DestroyBuilding();
        }
    }

    private IEnumerator BlinkDamange()
    {
        GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForEndOfFrame();
        GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", _originalColor);    
    }

    public virtual void DestroyBuilding()
    {
        _destroyed = true;
        GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", _originalColor);
        if (_currentParticle)
        {
            Destroy(_currentParticle);
        }
        
        _currentParticle = Instantiate(DestroyedParticle, new Vector3(
                transform.position.x, 
                transform.position.y + 0.5f, 
                transform.position.z), 
            Quaternion.identity);

        if (!_playedInitialParticle)
        {
            Instantiate(
                InitialDamageParticle, 
                new Vector3(
                    transform.position.x, 
                    transform.position.y + 1f, 
                    transform.position.z), 
                Quaternion.identity);
            _playedInitialParticle = true;
        }
        
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    private int CalculateCasualty(int dmg)
    {
        return (BuildingValue / MaxHealth) * dmg;
    }
   
}
