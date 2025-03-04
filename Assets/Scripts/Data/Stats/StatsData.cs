using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatsData
{
    #region Variables & Properties

    [SerializeField] List<StatData> statsData;
    public StatData this[int index] => statsData[index];
    public StatData this[string statName]
    {
        get 
        {
            for (int i = 0, statsDataCount = statsData.Count; i < statsDataCount; i++)
            {
                if (statsData[i]._name == statName)
                    return statsData[i];
            }

            return null;
        }
    }
    public int _count => statsData.Count;

    #endregion

    public StatsData(StatsData statsData)
    {
        this.statsData = statsData.statsData;
    }

    public bool Contains(string name)
    {
        for (int i = 0, statsDataCount = statsData.Count; i < statsDataCount; i++)
        {
            if (statsData[i]._name == name)
                return true;
        }

        return false;
    }

    public void Add(StatData statData)
    {
        statsData.Add(statData);
    }

    public void Insert(int index, StatData statData)
    {
        statsData.Insert(index, statData);
    }

    public void Remove(string name)
    {
        for (int i = 0, statsDataCount = statsData.Count; i < statsDataCount; i++)
        {
            if (statsData[i]._name == name)
                statsData.RemoveAt(i);
        }
    }
}
