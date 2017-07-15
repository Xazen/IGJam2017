using Assets.Scripts.Model;
using Zenject;

public class UnitSpawnController
{
	private readonly UnitSpawnModel _unitSpawnModel;
	private GridController _gridController;

	[Inject]
	public void Inject(GridController gridController)
	{
		_gridController = gridController;
	}
	
	public UnitSpawnController()
	{
		_unitSpawnModel = new UnitSpawnModel();
	}

	public bool IsUnitSelected()
	{
		return _unitSpawnModel.SelectedUnit != null;
	}

	public void SetSelectedUnit(Unit selectedUnit)
	{
		_unitSpawnModel.SetSelectedUnit(selectedUnit);
	}

	public Unit GetSelectedUnitPrefab()
	{
		return _unitSpawnModel.SelectedUnit;
	}

	public bool IsUnitSpawnPossibleAtGrid(int x, int y)
	{
		return _gridController.GetCellType(x, y) == CellType.Street;
	}
}
