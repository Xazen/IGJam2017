using System;
using UnityEngine;
using Zenject;

public class UnitSpawnView : MonoBehaviour
{
	private const double PositionCompareTolerance = 0.001f;
	
	[Header("General")]
	public Camera MainCamera;
	
	[Header("Span materials")]
	public Color SpawnPossibleMaterialColor;
	public Color SpawnImpossibleMaterialColor;

	private Vector2 _targetUnitSpawnPreviewPosition;
	private GameObject _unitSpawnPreviewGo;
	private Vector3 _mouseMapHitPosition;
	
	private UnitSpawnController _unitSpawnController;
	private Unit _unitSpawnPreview;
	private GameController _gameController;

	[Inject]
	public void Inject(UnitSpawnController unitSpawnController, GameController gameController)
	{
		_unitSpawnController = unitSpawnController;
		_gameController = gameController;
	}

	public void Update()
	{
		// No unit selected, just return
		if (_unitSpawnController == null || !_unitSpawnController.IsUnitSelected())
		{
			return;
		}

		if (IsMouseOverMap(out _mouseMapHitPosition))
		{
			_targetUnitSpawnPreviewPosition = new Vector3(
				Mathf.RoundToInt(_mouseMapHitPosition.x),
				Mathf.RoundToInt(_mouseMapHitPosition.z));
			UpdateSpawnPreview();
		}

		if (Input.GetKeyDown(KeyCode.Mouse0) && IsSpawnPossible())
		{
			_unitSpawnPreviewGo.GetComponent<Unit>().SpawnUnit();
			_gameController.UpdateBudget(-_gameController.GetCost(_unitSpawnPreviewGo.GetComponent<Unit>().UnitType));
			ResetSpawnPending();
			_unitSpawnController.ResetSelectedUnit();
		}
	}

	private bool IsMouseOverMap(out Vector3 mousePosition)
	{
		Ray	ray = MainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			mousePosition = hit.point;
			return true;
		}
		// Just move the preview somewhere outside the visible area
		mousePosition = new Vector3(-1000, -1000, -1000);
		return false;
	}

	private void UpdateSpawnPreview()
	{
		if (!IsSpawnPending() && _unitSpawnController.IsUnitSelected())
		{
			// Instantiate prefab
			_unitSpawnPreview = _unitSpawnController.GetSelectedUnitPrefab();
			_unitSpawnPreviewGo = Instantiate(_unitSpawnPreview.GetModel(), _targetUnitSpawnPreviewPosition,
				Quaternion.identity);
			_unitSpawnPreviewGo.transform.position = new Vector3(_targetUnitSpawnPreviewPosition.x, 0, _targetUnitSpawnPreviewPosition.y);
		}
		else if (IsSpawnPreviewPositionOutdated())
		{
			//Update prefab position
			_unitSpawnPreviewGo.transform.position = new Vector3(_targetUnitSpawnPreviewPosition.x, 0, _targetUnitSpawnPreviewPosition.y);

			if (IsSpawnPossible())
			{
				Debug.Log("possible");
				_unitSpawnPreviewGo.GetComponent<Unit>().SetMaterialColor(SpawnPossibleMaterialColor);
			}
			else
			{
				Debug.Log("impossible");
				_unitSpawnPreviewGo.GetComponent<Unit>().SetMaterialColor(SpawnImpossibleMaterialColor);
			}
		}
	}

	private bool IsSpawnPossible()
	{
		if (!IsSpawnPending())
		{
			return false;
		}
		return _unitSpawnController.IsUnitSpawnPossibleAtGrid(
			(int) _targetUnitSpawnPreviewPosition.x,
			(int) _targetUnitSpawnPreviewPosition.y);
	}

	private bool IsSpawnPreviewPositionOutdated()
	{
		if (_unitSpawnPreviewGo == null)
		{
			return false;
		}
		return !(Math.Abs(_unitSpawnPreviewGo.transform.position.x - _targetUnitSpawnPreviewPosition.x) < PositionCompareTolerance) ||
		       !(Math.Abs(_unitSpawnPreviewGo.transform.position.z - _targetUnitSpawnPreviewPosition.y) < PositionCompareTolerance);
	}

	private bool IsSpawnPending()
	{
		return _unitSpawnPreview != null;
	}

	private void ResetSpawnPending()
	{
		_unitSpawnPreview = null;
		_unitSpawnPreviewGo = null;
	}
}
