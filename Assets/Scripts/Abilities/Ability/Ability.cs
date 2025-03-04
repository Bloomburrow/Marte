using Attributes;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    #region Variables & Properties

    [Foldout("Variables & Properties (Ability)")]
    [SerializeField] protected Moment moment;
    public Moment _moment
    {
        get => moment;
    }
    [Foldout("Variables & Properties (Ability)")]
    [TextArea][SerializeField] protected string description;
    public string _description
    {
        get => description;
    }

    #endregion
     
    public abstract void Execute(CharacterData characterData);
}
