using UnityEngine;

public class WaitForSeconds : YieldInstruction
{
    #region Variables & Properties

    public float _seconds { get; private set; }

    #endregion

    public WaitForSeconds(float seconds)
    {
        _seconds = seconds;
    }
}