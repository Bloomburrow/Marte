using Attributes;
using UnityEngine;

public class RainbowPower : Ability
{
    #region Variables & Properties

    [Foldout("Variables & Properties (RainbowPower)")]
    [SerializeField] Sprite illustration;

    #endregion

    public override void Execute(CharacterData characterData) 
    {
        BattlefieldManager battlefieldManager = this.GetSingleton<BattlefieldManager>();

        battlefieldManager.EvolveCards<Punch, RainbowPunch>(characterData, "Rainbow Punch", illustration, "Deals <color=red>{S-Damage+1}</color> damage. Has a 20% chance to confuse.");
    }
}
