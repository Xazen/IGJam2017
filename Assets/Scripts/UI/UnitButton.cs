using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitButton : MonoBehaviour
{
	public UnitType UnitType;
	public Button Button;
	public GameObject UnitPrefab;
	public Text CostText;
    
	private UnitSpawnController _unitSpawnController;
	private GameController _gameController;

	[Inject]
	public void Inject(UnitSpawnController unitSpawnController, GameController gameController)
	{
		_gameController = gameController;
		_unitSpawnController = unitSpawnController;
	}

	private void Start()
	{
		Button.onClick.RemoveAllListeners();
		Button.onClick.AddListener(OnButtonClicked);
		CostText.text = _gameController.GetCost(UnitType).ToString();
	}

	private void OnButtonClicked()
	{
		if (_gameController.Budget >= _gameController.GetCost(UnitType))
		{
			_unitSpawnController.SetSelectedUnit(UnitPrefab.GetComponent<Unit>());	
		}
	}
}