using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parliament : Building
{
    private bool _destroyed;

    private void OnTriggerEnter(Collider other)
    {
        var rioter = other.gameObject.GetComponent<Rioter>();
        if (rioter != null && !_destroyed)
        {
            DestroyBuilding();
        }
    }

    public override void DestroyBuilding()
    {
        _gameController.FailGame();
        base.DestroyBuilding();
    }
}
