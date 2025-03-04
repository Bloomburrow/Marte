using Attributes;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Style Data", menuName = "Scriptable Objects/UI Style Data", order = 1)]
public class UIStyleData : ScriptableObject
{
    #region Variables & Properties

    [Foldout("Variables & Properties")]
    [SerializeField] Sprite background;
    public Sprite _background
    {
        get => background;
    }
    [Foldout("Variables & Properties")]
    [SerializeField] Color backgroundColor;
    public Color _backgroundColor
    {
        get => backgroundColor;
    }
    [Foldout("Variables & Properties")]
    [SerializeField] TMP_FontAsset textFont;
    public TMP_FontAsset _textFont
    {
        get => textFont;
    }
    [Foldout("Variables & Properties")]
    [SerializeField] Color textColor;
    public Color _textColor
    {
        get => textColor;
    }
    [Foldout("Variables & Properties")]
    [SerializeField] bool invertFontColor;
    public bool _invertFontColor
    {
        get => invertFontColor;
    }

    #endregion
}
