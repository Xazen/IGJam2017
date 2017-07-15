using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnController
{
	private readonly UnitSpawnModel _unitSpawnModel;
	
	public UnitSpawnController()
	{
		_unitSpawnModel = new UnitSpawnModel();
	}

	public bool IsUnitSelected()
	{
		return _unitSpawnModel.SelectedUnit != null;
	}

	public void SetSelectedUnit(IUnit selectedUnit)
	{
		_unitSpawnModel.SetSelectedUnit(selectedUnit);
	}

	public GameObject GetSelectedUnitPrefab()
	{
		return _unitSpawnModel.SelectedUnit.GetModel();
	}
}
