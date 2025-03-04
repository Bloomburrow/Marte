using Attributes;
using System;
using UnityEngine;

[Serializable]
public class CardData
{
    #region Variables & Properties

    [SerializeField] bool isHolographic;
    public bool _isHolographic
    {
        get => isHolographic;
        set
        {
            if(value)
                isHolographic = value;
        }
    }
    [LevelSlider]
    [SerializeField] Vector2Int level;
    public int _level
    {
        get => level.x;
        set
        {
            if (value > level.x)
            {
                int lastLevel = level.x;

                level.x = value;

                onLevelUp?.Invoke(level.x, lastLevel);
            }
        }

    }
    public int _maxLevel
    {
        get => level.y;
    }
    [SerializeField] int fragments;
    public int _fragments
    {
        get => fragments;
        set
        {
            int nextLevel = _level + 1;

            int maxLevel = _maxLevel;

            int fragmentsPerLevel = 20;

            while (nextLevel < maxLevel)
            {
                fragmentsPerLevel /= 2;

                maxLevel--;
            }

            if (value > fragmentsPerLevel)
                value = fragmentsPerLevel;

            if (value > fragments)
            {
                fragments = value;

                if (fragments == fragmentsPerLevel)
                {
                    _level = nextLevel;

                    fragments = 0;
                }
            }
        }
    }

    #endregion

    #region Events

    public event Action<int,int> onLevelUp;

    #endregion

    CardData() { }

    public CardData(CardData cardData)
    {
        isHolographic = cardData.isHolographic;
        level.x = cardData.level.x;
        fragments = cardData.fragments;
    }

    public CardData(bool isHolographic, int level, int maxLevel, int fragments)
    {
        this.isHolographic = isHolographic;
        this.level.x = level;
        this.level.y = maxLevel;
        this.fragments = fragments;
    }
}
