using UnityEngine;

public class Parliament : Building
{
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
