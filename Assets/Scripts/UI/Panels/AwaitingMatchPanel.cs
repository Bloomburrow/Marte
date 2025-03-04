using Attributes;
using TMPro;
using UnityEngine;

public class AwaitingMatchPanel : Panel
{
    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] GameDataManager gameDataManager;

    [Foldout("Components/External")]
    [SerializeField] TMP_Text titleText;

    #endregion

    public override void Initialize()
    {
        gameDataManager = this.GetSingleton<GameDataManager>();
    }

    public override void SetUIStyle(UIStyleData UIStyleData)
    {
        titleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
        {
            titleText.color = InvertColor(titleText.color);

            _fontColorIsInverted = true;
        }
        else if (_fontColorIsInverted)
            titleText.color = InvertColor(titleText.color);
    }

    public override void OnOpen()
    {
        if (gameDataManager._charactersData.Count > 0)
        {
            CharacterData currentCharacterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

            SetUIStyle(currentCharacterData._UIStyleData);
        }
    }
}
