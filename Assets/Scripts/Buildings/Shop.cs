using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Building
{
    private List<Rioter> _attackers = new List<Rioter>();

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


    public override void DestroyBuilding()
    {
        foreach (var rioter in _attackers)
        {
            rioter.Continue();
        }
        base.DestroyBuilding();
    }
}