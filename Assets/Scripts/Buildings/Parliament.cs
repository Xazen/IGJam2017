using System.Collections;
using UnityEngine;

public class Parliament : Building
{

    public static GameObject Instance { get; set; }

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
        yield return new WaitForSeconds(3f);
        _gameController.FailGame();
    }

    private void Start()
    {
        Instance = gameObject;
    }


}
