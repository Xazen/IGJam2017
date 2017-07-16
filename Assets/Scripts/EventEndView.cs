using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EventEndView : MonoBehaviour
{
    public Text CausualityCostText;
    public Text DestroyedShopText;
    public Text SolvedDemoText;
    public Text WoundedPolicemenText;
    
    private GameController _gameController;

    [Inject]
    public void Inject(GameController gameController)
    {
        Debug.Log("event end inject");
        _gameController = gameController;
    }

    private void Start()
    {
        CausualityCostText.text = _gameController.CasualtyCounter + " Euro";
        DestroyedShopText.text = _gameController.DestroyedShopCounter.ToString();
        SolvedDemoText.text = _gameController.SolvedDemoCounter.ToString();
        WoundedPolicemenText.text = _gameController.WoundedPolicemenCounter.ToString();
    }
}
