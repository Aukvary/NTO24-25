using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour 
{ 
    private HashSet<Unit> _allunits = new();

    public IEnumerable<Unit> AllUnits => _allunits;

    public bool Add(Unit unit) =>
        _allunits.Add(unit);
    public bool Remove(Unit unit) =>
        _allunits.Remove(unit);
}