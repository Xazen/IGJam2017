public class UnitSpawnModel
{
    private IUnit _selectedUnit;
    public IUnit SelectedUnit 
    {
        get
        {
            return _selectedUnit; 
        } 
    }

    public void SetSelectedUnit(IUnit selectedUnit)
    {
        _selectedUnit = selectedUnit;
    }
}
