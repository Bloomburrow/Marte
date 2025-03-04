using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatesData
{
    #region Variables & Properties

    [SerializeField] List<StateData> statesData;
    public StateData this[int index] => statesData[index];
    public StateData this[string stateName]
    {
        get
        {
            for (int i = 0, statesDataCount = statesData.Count; i < statesDataCount; i++)
            {
                if (statesData[i]._name == stateName)
                    return statesData[i];
            }

            return null;
        }
    }
    public int _count => statesData.Count;

    #endregion

    public StatesData(StatesData statesData)
    {
        this.statesData = statesData.statesData;
    }

    public bool Contains(string name)
    {
        for (int i = 0, statesDataCount = statesData.Count; i < statesDataCount; i++)
        {
            if (statesData[i]._name == name)
                return true;
        }

        return false;
    }

    public void Add(StateData stateData)
    {
        statesData.Add(stateData);
    }

    public void Insert(int index, StateData stateData)
    {
        statesData.Insert(index, stateData);
    }

    public void Remove(string name)
    {
        for (int i = 0, statesDataCount = statesData.Count; i < statesDataCount; i++)
        {
            if (statesData[i]._name == name)
                statesData.RemoveAt(i);
        }
    }
}
