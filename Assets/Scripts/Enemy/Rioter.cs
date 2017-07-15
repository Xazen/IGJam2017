using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PathfinderAgent))]
public class Rioter : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private float _invicibiltySeconds;
    [SerializeField] private int _damagePoints;
    [SerializeField] private float _attackRate;

    [SerializeField] private PathfinderAgent _pathfinder;

    public delegate void RioterDie(Rioter rioter);
    public event RioterDie OnRioterDie;

    private Vector3 _target;
    private float _lastAttackTimestamp;
    private int _currentHealth;
    private float _currentInvicibilitySecounds;

    public int TryAttack()
    {
        if (Time.time - _lastAttackTimestamp >= _attackRate)
        {
            Debug.Log("attack");
            _lastAttackTimestamp = Time.time;
            return _damagePoints;
        }
        return 0;
    }

    public void MoveTo(Vector3 target)
    {
        _target = target;
        _pathfinder.CalculatePath(target);
    }

    private void Start()
    {
        _lastAttackTimestamp = Time.time;
        _currentHealth = _health;
        gameObject.tag = TagConstants.Enemy;
    }

    private void Update()
    {
        if (_currentInvicibilitySecounds > 0)
        {
            _currentInvicibilitySecounds -= Time.deltaTime;
        }
    }

    public void Stop()
    {
        _pathfinder.Stop();
    }

    public void Continue()
    {
        _pathfinder.CalculatePath(_target);
    }

    public void TryTakeDamage(int damage)
    {
        if (_currentInvicibilitySecounds <= 0)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            _currentInvicibilitySecounds = _invicibiltySeconds;
        }
        
        if (_currentHealth == 0)
        {
            gameObject.transform.DOMoveY(-1, 2, true).OnComplete(OnDieEnd);
            if (OnRioterDie != null)
            {
                OnRioterDie(this);
            }
        }
    }
    
    private void OnDieEnd()
    {
        Destroy(gameObject);
    }
}
