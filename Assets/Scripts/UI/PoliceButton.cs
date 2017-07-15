using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PoliceButton : MonoBehaviour
{
    public Button UnitButton;
    public GameObject UnitPrefab;
    
    private UnitSpawnController _unitSpawnController;

    [Inject]
    public void Inject(UnitSpawnController unitSpawnController)
    {
        _unitSpawnController = unitSpawnController;
    }

    private void Start()
    {
        UnitButton.onClick.RemoveAllListeners();
        UnitButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _unitSpawnController.SetSelectedUnit(UnitPrefab.GetComponent<IUnit>());
    }
}
