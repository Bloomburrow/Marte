using UnityEngine;

public class RainbowPunch : Punch
{
    protected override void OnDissolved()
    {
        base.OnDissolved();

        bool confuse = Random.Range(0, 101) < 20;

        if (confuse)
            battlefieldManager.AddState(opponentCharacterData(), "Confused", 1);
    }
}
