using UnityEngine;

public class BetrayTheOwner : Card
{
    public override void Play()
    {
        base.Play();

        battlefieldManager.AddState(opponentCharacterData(), "Betray The Owner", (int)statsData["Curse Duration"]._value);
    }
}