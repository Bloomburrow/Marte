using UnityEngine;

public class Possession : Ability
{
    public override void Execute(CharacterData characterData) 
    {
        BattlefieldManager battlefieldManager = this.GetSingleton<BattlefieldManager>();

        battlefieldManager.AddStat(characterData, false, "Evilness", 1f);
    }
}
