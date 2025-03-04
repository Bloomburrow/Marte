using Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterPanel : Panel
{
    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] GameDataManager gameDataManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] MainUIManager mainUIManager;

    [Foldout("Components/External")]
    [SerializeField] TMP_Text characterNameText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text characterLevelText;
    [Foldout("Components/External")]
    [SerializeField] Image characterBackgroundImage;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] Image characterImage;
    [Foldout("Components/External")]
    [SerializeField] Button previousCharacterButton;
    [Foldout("Components/External")]
    [SerializeField] Button nextCharacterButton;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text characterOffensiveTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterCriticTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterCriticText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text characterDefensiveTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterLivesTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterLivesText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterInitialArmorTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterInitialArmorText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text characterIntelligenceTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterInitialHandTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterInitialHandText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterActionsPerTurnTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text characterActionsPerTurnText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text manaTitleText;
    [Foldout("Components/External")]
    [Indent(1)][SerializeField] TMP_Text manaText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text characterAbilityText;
    [Foldout("Components/External")]
    [SerializeField] Button chooseCharacterButton;

    #endregion

    public override void Initialize()
    {
        gameDataManager = this.GetSingleton<GameDataManager>();

        mainUIManager = this.GetSingleton<MainUIManager>();
    }

    public override void SetUIStyle(UIStyleData UIStyleData)
    {
        mainUIManager.SetBackground(UIStyleData._background);

        mainUIManager.SetBackgroundColor(UIStyleData._backgroundColor);

        characterNameText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
        {
            characterNameText.color = InvertColor(characterNameText.color);

            _fontColorIsInverted = true;
        }
        else if (_fontColorIsInverted)
            characterNameText.color = InvertColor(characterNameText.color);

        characterLevelText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterLevelText.color = InvertColor(characterLevelText.color);
        else if (_fontColorIsInverted)
            characterLevelText.color = InvertColor(characterLevelText.color);

        characterOffensiveTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterOffensiveTitleText.color = InvertColor(characterOffensiveTitleText.color);
        else if (_fontColorIsInverted)
            characterOffensiveTitleText.color = InvertColor(characterOffensiveTitleText.color);

        characterCriticTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterCriticTitleText.color = InvertColor(characterCriticTitleText.color);
        else if (_fontColorIsInverted)
            characterCriticTitleText.color = InvertColor(characterCriticTitleText.color);

        characterCriticText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterCriticText.color = InvertColor(characterCriticText.color);
        else if (_fontColorIsInverted)
            characterCriticText.color = InvertColor(characterCriticText.color);

        characterDefensiveTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterDefensiveTitleText.color = InvertColor(characterDefensiveTitleText.color);
        else if (_fontColorIsInverted)
            characterDefensiveTitleText.color = InvertColor(characterDefensiveTitleText.color);

        characterLivesTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterLivesTitleText.color = InvertColor(characterLivesTitleText.color);
        else if (_fontColorIsInverted)
            characterLivesTitleText.color = InvertColor(characterLivesTitleText.color);

        characterLivesText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterLivesText.color = InvertColor(characterLivesText.color);
        else if (_fontColorIsInverted)
            characterLivesText.color = InvertColor(characterLivesText.color);

        characterInitialArmorTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterInitialArmorTitleText.color = InvertColor(characterInitialArmorTitleText.color);
        else if (_fontColorIsInverted)
            characterInitialArmorTitleText.color = InvertColor(characterInitialArmorTitleText.color);

        characterInitialArmorText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterInitialArmorText.color = InvertColor(characterInitialArmorText.color);
        else if (_fontColorIsInverted)
            characterInitialArmorText.color = InvertColor(characterInitialArmorText.color);

        characterIntelligenceTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterIntelligenceTitleText.color = InvertColor(characterIntelligenceTitleText.color);
        else if (_fontColorIsInverted)
            characterIntelligenceTitleText.color = InvertColor(characterIntelligenceTitleText.color);

        characterInitialHandTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterInitialHandTitleText.color = InvertColor(characterInitialHandTitleText.color);
        else if (_fontColorIsInverted)
            characterInitialHandTitleText.color = InvertColor(characterInitialHandTitleText.color);

        characterInitialHandText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterInitialHandText.color = InvertColor(characterInitialHandText.color);
        else if (_fontColorIsInverted)
            characterInitialHandText.color = InvertColor(characterInitialHandText.color);

        characterActionsPerTurnTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterActionsPerTurnTitleText.color = InvertColor(characterActionsPerTurnTitleText.color);
        else if (_fontColorIsInverted)
            characterActionsPerTurnTitleText.color = InvertColor(characterActionsPerTurnTitleText.color);

        characterActionsPerTurnText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterActionsPerTurnText.color = InvertColor(characterActionsPerTurnText.color);
        else if (_fontColorIsInverted)
            characterActionsPerTurnText.color = InvertColor(characterActionsPerTurnText.color);

        manaTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            manaTitleText.color = InvertColor(manaTitleText.color);
        else if (_fontColorIsInverted)
            manaTitleText.color = InvertColor(manaTitleText.color);

        manaText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            manaText.color = InvertColor(manaText.color);
        else if (_fontColorIsInverted)
            manaText.color = InvertColor(manaText.color);

        characterAbilityText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
            characterAbilityText.color = InvertColor(characterAbilityText.color);
        else if (_fontColorIsInverted)
        {
            characterAbilityText.color = InvertColor(characterAbilityText.color);

            _fontColorIsInverted = false;
        }
    }

    public override void OnOpen()
    {
        if (gameDataManager._charactersData.Count > 0)
        {
            CharacterData currentCharacterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

            SetCharacter(currentCharacterData);

            SetUIStyle(currentCharacterData._UIStyleData);
        }

        previousCharacterButton.gameObject.SetActive(false);

        nextCharacterButton.gameObject.SetActive(gameDataManager._charactersData.Count > 1);
    }

    void SetCharacter(CharacterData characterData)
    {
        characterNameText.text = characterData._name;

        characterLevelText.text = $"Level: {characterData._level}";

        characterBackgroundImage.color = characterData._UIStyleData._backgroundColor + new Color(0.04f, 0.04f, 0.04f, 0f);

        characterImage.sprite = characterData._sprite;

        characterCriticText.text = $": {characterData._statsData["Critic"]._value}%";

        characterLivesText.text = $": {characterData._statsData["Lives"]._value}";

        characterInitialArmorText.text = $": {characterData._statsData["Armor"]._value}";

        characterInitialHandText.text = $": {characterData._statsData["Hand"]._value}";

        characterActionsPerTurnText.text = $": {characterData._statsData["Actions Per Turn"]._value}";

        manaText.text = $": {characterData._statsData["Mana"]._value}";

        characterAbilityText.text = characterData._ability._description;
    }

    public void PreviousCharacter()
    {
        if (gameDataManager._selectedCharacterIndex - 1 >= 0)
        {
            gameDataManager._selectedCharacterIndex --;

            CharacterData currentCharacterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

            SetCharacter(currentCharacterData);

            SetUIStyle(currentCharacterData._UIStyleData);

            if (gameDataManager._selectedCharacterIndex == 0)
                previousCharacterButton.gameObject.SetActive(false);

            if (!nextCharacterButton.gameObject.activeInHierarchy)
                nextCharacterButton.gameObject.SetActive(true);
        }
    }

    public void NextCharacter()
    {
        if (gameDataManager._selectedCharacterIndex + 1 < gameDataManager._charactersData.Count)
        {
            gameDataManager._selectedCharacterIndex ++;

            CharacterData currentCharacterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

            SetCharacter(currentCharacterData);

            SetUIStyle(currentCharacterData._UIStyleData);

            if (!previousCharacterButton.gameObject.activeInHierarchy)
                previousCharacterButton.gameObject.SetActive(true);

            if (gameDataManager._selectedCharacterIndex == gameDataManager._charactersData.Count - 1)
                nextCharacterButton.gameObject.SetActive(false);
        }
    }

    public void ChooseCharacter()
    {
        gameDataManager.SavePlayerData();

        CharacterData characterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

        characterData._isUnlocked = true;

        for (int i = 0, initialDeckCardsCount = characterData._initialDeckCardsIndexes.Count; i < initialDeckCardsCount; i++)
        {
            int initialDeckCardIndex = characterData._initialDeckCardsIndexes[i];

            int cardIndex = gameDataManager._cardsData[initialDeckCardIndex].Add(new CardData(false, 1, gameDataManager._cardsPrefabs[initialDeckCardIndex]._data._maxLevel,  0));

            characterData._deckData._cardsIndexes[i] = new Vector2Int(characterData._initialDeckCardsIndexes[i], cardIndex);
        }

        gameDataManager.SaveCardsData();

        gameDataManager.SaveCharactersMetaData();

        mainUIManager.OpenLobbyPanel();
	}
}
