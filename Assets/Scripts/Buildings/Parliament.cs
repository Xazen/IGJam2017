using System.Collections;
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
        base.DestroyBuilding();
        StartCoroutine(TriggerFailGame());
    }

    private IEnumerator TriggerFailGame()
    {
        yield return new WaitForSeconds(2.5f);
        _gameController.FailGame();
    }
}
