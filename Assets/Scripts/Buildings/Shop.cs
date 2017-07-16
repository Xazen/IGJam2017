using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using Zenject;

public class Shop : Building
{
    private List<Rioter> _attackers = new List<Rioter>();
    private GameController _gameController;
    private GridController _gridController;

    [Inject]
    public void Inject(GameController gameController, GridController gridController)
    {
        _gameController = gameController;
        _gridController = gridController;
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


    public override void DestroyBuilding()
    {
        foreach (var rioter in _attackers)
        {
            rioter.Continue();
        }
        _gameController.DestroyedShopCounter++;
        base.DestroyBuilding();
    }
}