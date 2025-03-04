using System;
using UnityEngine;

[Serializable]
public class StateData
{
    #region Variables & Properties

    [SerializeField] string name;
    public string _name
    {
        get => name;
    }
    [SerializeField] int turns;
    public int _leftTurns
    {
        get => turns;
        set => turns = value;
    }

    #endregion

    private StateData() { }

    public StateData(string name, int turns)
    {
        this.name = name;

        this.turns = turns;
    }
}
