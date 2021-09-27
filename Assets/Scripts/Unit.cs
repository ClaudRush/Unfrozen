public class Unit
{
    private UnitColor _color;
    private int _cellNumber;
    private int _initiative;
    private int _speed;

    public UnitColor Color => _color;
    public int CellNumber => _cellNumber;
    public int Initiative => _initiative;
    public int Speed => _speed;
    public bool WasTurned { get; private set; } = false;

    public Unit(UnitColor color, int cellNumber, int initiative, int speed)
    {
        _color = color;
        _cellNumber = cellNumber;
        _initiative = initiative;
        _speed = speed;
    }

    public override string ToString()
    {
        return _color + ", " + _cellNumber;
    }

    public void SetWasTurned(bool wasTurned)
    {
        WasTurned = wasTurned;
    }
}
public enum UnitColor
{
    Blue,
    Red
}
