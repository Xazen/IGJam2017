using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfinderAgent))]
public class Rioter : MonoBehaviour
{
    [SerializeField] private int _damagePoints;

    [SerializeField] private PathfinderAgent _pathfinder;

    private Vector3 _target;

    public int DamagePoints
    {
        get
        {
            return _damagePoints;
        }

        set
        {
            _damagePoints = value;
        }
    }

    public void MoveTo(Vector3 target)
    {

        _target = target;
        _pathfinder.CalculatePath(target);
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
