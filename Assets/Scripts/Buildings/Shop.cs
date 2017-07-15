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
        if (rioter != null)
        {
            _attackers.Add(rioter);
            rioter.Stop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null)
        {
            GetDamage(rioter.TryAttack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null)
        {
            _attackers.Remove(rioter);
        }
    }




    public override void DestroyBuilding()
    {
        foreach (var rioter in _attackers)
        {
            Debug.Log("shop destroyed");
            rioter.Continue();
        }
        base.DestroyBuilding();
    }
}