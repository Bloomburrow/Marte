using UnityEngine;

public class GranHunter : Ability
{
    public override void Execute(CharacterData characterData) 
    {
        BattlefieldManager battlefieldManager = this.GetSingleton<BattlefieldManager>();

        battlefieldManager.ImproveCards<Ambush>(characterData, "Deal <color=red>{S-Damage+1}</color> piercing damage per each card played this round.");
    }
}
