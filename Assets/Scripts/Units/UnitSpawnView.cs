using System;
using UnityEngine;
using Zenject;

public class UnitSpawnView : MonoBehaviour
{
	private const double PositionCompareTolerance = 0.001f;

	[Header("General")]
	public Camera MainCamera;
	
	[Header("Span materials")]
	public Material SpawnPossibleMaterial;
	public Material SpawnImpossibleMaterial;

	private Vector2 _targetUnitSpawnPreviewPosition;
	private GameObject _unitSpawnPreviewGo;
	private Vector3 _mouseMapHitPosition;
	
	private UnitSpawnController _unitSpawnController;
	private Unit _unitSpawnPreview;

	[Inject]
	public void Inject(UnitSpawnController unitSpawnController)
	{
		_unitSpawnController = unitSpawnController;
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
			_unitSpawnPreview.ResetMaterial();
			ResetSpawnPending();
		}
	}

	private bool IsMouseOverMap(out Vector3 mousePosition)
	{
		Ray	ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
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
		if (!IsSpawnPending())
		{
			_unitSpawnPreview = _unitSpawnController.GetSelectedUnitPrefab();
			_unitSpawnPreviewGo = Instantiate(_unitSpawnPreview.GetModel(), _targetUnitSpawnPreviewPosition,
				Quaternion.identity);
		}
		else if (IsSpawnPreviewPositionOutdated())
		{
			_unitSpawnPreviewGo.transform.position = _targetUnitSpawnPreviewPosition;

			if (IsSpawnPossible())
			{
				_unitSpawnPreview.SetMaterial(SpawnPossibleMaterial);
			}
			else
			{
				_unitSpawnPreview.SetMaterial(SpawnImpossibleMaterial);
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
