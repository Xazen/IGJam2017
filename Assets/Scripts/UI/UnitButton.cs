using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitButton : MonoBehaviour
{
	public Button Button;
	public GameObject UnitPrefab;
    
	private UnitSpawnController _unitSpawnController;

	[Inject]
	public void Inject(UnitSpawnController unitSpawnController)
	{
		_unitSpawnController = unitSpawnController;
	}

	private void Start()
	{
		Button.onClick.RemoveAllListeners();
		Button.onClick.AddListener(OnButtonClicked);
	}

	private void OnButtonClicked()
	{
		_unitSpawnController.SetSelectedUnit(UnitPrefab.GetComponent<Unit>());
	}
}