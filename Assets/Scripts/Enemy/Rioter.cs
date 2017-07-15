using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfinderAgent))]
public class Rioter : MonoBehaviour
{
    [SerializeField] private int _damagePoints;
    [SerializeField] private float _attackRate;

    [SerializeField] private PathfinderAgent _pathfinder;

    private Vector3 _target;
    private float _lastAttackTimestamp;


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
    }




    public void Stop()
    {
        _pathfinder.Stop();
    }

    public void Continue()
    {
        _pathfinder.CalculatePath(_target);
    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
