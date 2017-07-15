public class UnitSpawnModel
{
    private Unit _selectedUnit;
    public Unit SelectedUnit 
    {
        get
        {
            return _selectedUnit; 
        } 
    }

    public void SetSelectedUnit(Unit selectedUnit)
    {
        _selectedUnit = selectedUnit;
    }
}
