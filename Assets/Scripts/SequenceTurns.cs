using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SequenceTurns : MonoBehaviour
{
    private int _displayedCount = 20;
    private int _roundNumber = 1;

    private List<Unit> _units = new List<Unit>();
    private List<Unit> _nextRoundsUnits = new List<Unit>();
    private List<Unit> _currentRoundUnits = new List<Unit>();

    private Unit _currentUnit;

    void Start()
    {
        InitUnits();
        SortingUnits(_roundNumber);
        InitFirstUnit();
        ReloadDisplayedUnits();
    }

    private void InitUnits()
    {
        _units.Add(new Unit(UnitColor.Red, 1, 8, 4));
        _units.Add(new Unit(UnitColor.Red, 2, 8, 4));
        _units.Add(new Unit(UnitColor.Red, 3, 9, 5));
        _units.Add(new Unit(UnitColor.Red, 4, 4, 3));
        _units.Add(new Unit(UnitColor.Red, 5, 2, 3));
        _units.Add(new Unit(UnitColor.Red, 6, 3, 4));
        _units.Add(new Unit(UnitColor.Red, 7, 1, 1));

        _units.Add(new Unit(UnitColor.Blue, 1, 6, 6));
        _units.Add(new Unit(UnitColor.Blue, 2, 8, 5));
        _units.Add(new Unit(UnitColor.Blue, 3, 9, 5));
        _units.Add(new Unit(UnitColor.Blue, 4, 8, 4));
        _units.Add(new Unit(UnitColor.Blue, 5, 2, 3));
        _units.Add(new Unit(UnitColor.Blue, 6, 4, 2));
        _units.Add(new Unit(UnitColor.Blue, 7, 1, 1));
    }

    private void SortingUnits(int roundNumber)
    {
        _units = _units.OrderByDescending(x => x.Initiative)
            .ThenByDescending(x => x.Speed)
            .ThenBy(x => x.CellNumber)
            .ThenByDescending(x => roundNumber % 2 == 1 ? x.Color == UnitColor.Red : x.Color == UnitColor.Blue)
            .ToList();
    }

    private void InitFirstUnit()
    {
        _currentUnit = _units.First();
        _currentUnit.SetWasTurned(true);
    }

    private void ReloadDisplayedUnits()
    {
        Utils.ClearConsole();

        _nextRoundsUnits.Clear();
        _currentRoundUnits.Clear();

        var currentRound = _roundNumber;

        while (_nextRoundsUnits.Count + _currentRoundUnits.Count + 1 < _displayedCount)
        {
            Debug.Log("\t round: " + currentRound);

            if (currentRound == _roundNumber)
            {
                Debug.Log(_currentUnit.ToString());

                for (int i = 0; i < _units.Count; i++)
                    AddingAndPrintingUnits(_currentRoundUnits, !_units[i].WasTurned, i);
            }
            else
            {
                for (int i = 0; i < _units.Count; i++)
                    AddingAndPrintingUnits(_nextRoundsUnits, _nextRoundsUnits.Count + _currentRoundUnits.Count + 1 < _displayedCount, i);
            }
            currentRound++;
            SortingUnits(currentRound);
        }
    }

    private void AddingAndPrintingUnits(List<Unit> units, bool condition, int i)
    {
        if (condition)
        {
            units.Add(_units[i]);
            Debug.Log(units[units.Count - 1].ToString());
        }
    }

    public void KillNextEnemyUnit()
    {
        var nextEnemy = GetNextEnemy();
        _currentRoundUnits.Remove(nextEnemy);
        _units.Remove(nextEnemy);

        SkipTurn();
    }

    private Unit GetNextEnemy()
    {
        var nextEnemy = _units.Where(x => _currentUnit.Color != x.Color).First();

        if (_currentRoundUnits.Count != 0)
            nextEnemy = _currentRoundUnits.Where(x => _currentUnit.Color != x.Color).First();
        return nextEnemy;
    }

    public void SkipTurn()
    {
        if (IsEndRound())
        {
            _units.ForEach(x => x.SetWasTurned(false));
            InitFirstUnit();
            _roundNumber++;
        }
        else
        {
            _currentUnit = _currentRoundUnits.First();
            _currentUnit.SetWasTurned(true);
        }
        ReloadDisplayedUnits();
    }

    private bool IsEndRound()
    {
        return _currentRoundUnits.Count == 0;
    }
}
