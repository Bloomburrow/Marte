using Attributes;
using System;
using TMPro;
using UnityEngine;

public class InfoPanel : Panel
{
    #region Variables & Porperties

    Func<StatsData> characterStatsData;

    [Foldout("Variables & Properties")]
    [SerializeField] GameObject onTurn;

    float livesBarSize;
    float manaBarSize;
    int deck;

    #endregion

    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] GameDataManager gameDataManager;

    [Foldout("Components/External")]
    [SerializeField] TMP_Text nameText;
    [Foldout("Components/External")]
    [SerializeField] RectTransform livesBarRectTranform;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text livesText;
    [Foldout("Components/External")]
    [SerializeField] RectTransform manaBarRectTranform;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text manaText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text criticText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text actionsPerTurnText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text deckText;

    #endregion

    public override void Initialize()
    {
        livesBarSize = livesBarRectTranform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x - 10f;

        manaBarSize = manaBarRectTranform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x - 10f;

        gameDataManager = this.GetSingleton<GameDataManager>();
    }

    public override void SetUIStyle(UIStyleData UIStyleData)
    {
        nameText.font = UIStyleData._textFont;

        nameText.color = UIStyleData._textColor;

        livesText.font = UIStyleData._textFont;

        manaText.font = UIStyleData._textFont;

        criticText.font = UIStyleData._textFont;

        actionsPerTurnText.font = UIStyleData._textFont;

        deckText.font = UIStyleData._textFont;
    }

    public void SetCharacterData(CharacterData characterData)
    {
        this.characterStatsData = () =>
        {
            return characterData._statsData;
        };

        StatsData characterStatsData = characterData._statsData;

        nameText.text = characterData._name;

        float lives = characterStatsData["Lives"]._value;

        float maxLives = characterStatsData["Max Lives"]._value;

        livesBarRectTranform.offsetMax = new Vector2(-(((livesBarSize - 10f) / (-maxLives) * lives) + livesBarSize), livesBarRectTranform.offsetMax.y);

        livesText.text = $"{lives}/{maxLives}";

        float mana = characterStatsData["Mana"]._value;

        float maxMana = characterStatsData["Max Mana"]._value;

        manaBarRectTranform.offsetMax = new Vector2(-(((manaBarSize - 10f) / (-maxMana) * mana) + manaBarSize), manaBarRectTranform.offsetMax.y);

        manaText.text = $"{mana}/{maxMana}";

        criticText.text = $":{characterStatsData["Critic"]._value}";

        float actionsPerTurn = characterStatsData["Actions Per Turn"]._value;

        float maxActionsPerTurn = characterStatsData["Max Actions Per Turn"]._value;

        actionsPerTurnText.text = $":{actionsPerTurn}/{maxActionsPerTurn}";

        deck = characterData._deckData._size;

        deckText.text = $":{deck}";
    }

    public void SetOnTurn(bool value)
    {
        onTurn.SetActive(value);
    }

    public void UpdateLives()
    {
        StatsData characterStatsData = this.characterStatsData();

        float lives = characterStatsData["Lives"]._value;

        float maxLives = characterStatsData["Max Lives"]._value;

        livesBarRectTranform.offsetMax = new Vector2(-(((livesBarSize - 10f) / (-maxLives) * lives) + livesBarSize), livesBarRectTranform.offsetMax.y);

        livesText.text = $"{lives}/{maxLives}";
    }

    public void UpdateMana()
    {
        StatsData characterStatsData = this.characterStatsData();

        float mana = characterStatsData["Mana"]._value;

        float maxMana = characterStatsData["Max Mana"]._value;

        manaBarRectTranform.offsetMax = new Vector2(-(((manaBarSize - 10) / (-maxMana) * mana) + manaBarSize), manaBarRectTranform.offsetMax.y);

        manaText.text = $"{mana}/{maxMana}";
    }

    public void UpdateActionsPerTurn()
    {
        StatsData characterStatsData = this.characterStatsData();

        float actionsPerTurn = characterStatsData["Actions Per Turn"]._value;

        float maxActionsPerTurn = characterStatsData["Max Actions Per Turn"]._value;

        actionsPerTurnText.text = $":{actionsPerTurn}/{maxActionsPerTurn}";
    }

    public void UpdateDeck(int deltaCards)
    {
        deck += deltaCards;

        deckText.text = $":{deck}";
    }
}
