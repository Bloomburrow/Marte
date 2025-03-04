using Attributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeedbackPanel : Panel
{
    #region Variables & Properties

    Func<StatsData> characterStatsData;
    Func<StatesData> characterStatesData;

    [Foldout("Variables & Properties/Stats")]
    [SerializeField] GameObject amorPanel;
    [Foldout("Variables & Properties/Stats")]
    [SerializeField] GameObject evilnessPanel;
    [Foldout("Variables & Properties/States")]
    [SerializeField] GameObject confusePanel;
    [Foldout("Variables & Properties/States/Curses")]
    [SerializeField] GameObject BetrayTheOwnerPanel;

    #endregion

    #region Components

    [Foldout("Components/External/Stats")]
    [SerializeField] TMP_Text amorText;
    [Foldout("Components/External/Stats")]
    [SerializeField] TMP_Text evilnessText;
    [Foldout("Components/External/States")]
    [SerializeField] TMP_Text confuseText;
    [Foldout("Components/External/States/Curses")]
    [SerializeField] TMP_Text BetrayTheOwnerText;

    #endregion

    public override void SetUIStyle(UIStyleData UIStyleData)
    {
        amorText.font = UIStyleData._textFont;

        evilnessText.font = UIStyleData._textFont;
    }
    public void SetCharacterData(CharacterData characterData)
    {
        characterStatsData = () =>
        {
            return characterData._statsData;
        };

        characterStatesData = () =>
        {
            return characterData._statesData;
        };
    }

    public void UpdateArmor()
    {
        float armor = characterStatsData()["Armor"]._value;

        amorPanel.SetActive(armor > 0);

        if (armor == 0)
            amorText.text = "";
        else
            amorText.text = $"{armor}";
    }

    public void UpdateEvilness()
    {
        float evilness = characterStatsData()["Evilness"]._value;

        evilnessPanel.SetActive(evilness > 0);

        if (evilness == 0)
            evilnessText.text = "";
        else
            evilnessText.text = $"{evilness}";
    }

    public void UpdateConfused()
    {
        float confusedLeftTurns = characterStatesData()["Confused"]._leftTurns;

        confusePanel.SetActive(confusedLeftTurns > 0);

        if (confusedLeftTurns == 0)
            confuseText.text = "";
        else
            confuseText.text = $"{confusedLeftTurns}";
    }

    public void UpdateBetrayTheOwner()
    {
        float betrayTheOwnerLeftTurns = characterStatesData()["Betray The Owner"]._leftTurns;

        BetrayTheOwnerPanel.SetActive(betrayTheOwnerLeftTurns > 0);

        if (betrayTheOwnerLeftTurns == 0)
            BetrayTheOwnerText.text = "";
        else
            BetrayTheOwnerText.text = $"{betrayTheOwnerLeftTurns}";
    }
}
