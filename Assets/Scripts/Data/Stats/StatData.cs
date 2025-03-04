using System;
using UnityEngine;

[Serializable]
public class StatData
{
    #region Variables & Properties

    [SerializeField] bool isMoment;

    [SerializeField] string name;
    public string _name
    {
        get => name;
    }
    [SerializeField] float value;
    public float _value
    {
        get => value;
        set
        {
            float lastValue = this.value;

            this.value = value;

            if (this.value < 0)
                this.value = 0;

            onValueChange?.Invoke(this.value - lastValue);
        }

    }
    [SerializeField] bool increasePerLevel;
    [SerializeField] float valueIncreasePerLevel; 
    public float _valueIncreasePerLevel
    {
        get => valueIncreasePerLevel;

    }

    #endregion

    #region Events

    public event Action<float> onValueChange;

    #endregion

    StatData() { }

    public StatData(bool isMoment, string name, float value, bool increasePerLevel, float valueIncreasePerLevel) 
    {
        this.isMoment = isMoment;

        this.name = name;

        this.value = value;

        this.increasePerLevel = increasePerLevel;   

        this.valueIncreasePerLevel = valueIncreasePerLevel;
    }
}
