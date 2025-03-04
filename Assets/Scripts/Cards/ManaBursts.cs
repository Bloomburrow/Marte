using System;
using UnityEngine;

public class ManaBursts : Card
{
    #region Variables & Properties

    string description;

    #endregion

    public override void Initialize(Func<CharacterData> playerCharacterData, Func<CharacterData> opponentCharacterData)
    {
        battlefieldManager = this.GetSingleton<BattlefieldManager>();

        battlefieldUIManager = this.GetSingleton<BattlefieldUIManager>();

        this.playerCharacterData = playerCharacterData;

        this.playerCharacterData()._statsData["Mana"].onValueChange += UpdateDescriptionText;

        this.opponentCharacterData = opponentCharacterData;

        description = descriptionText.text;

        UpdateDescriptionText();
    }

    void UpdateDescriptionText(float deltaMana = 0)
    {
        string description = this.description;

        string fixedDescription = "";

        for (int i = 0, descriptionLength = description.Length; i < descriptionLength; i++)
        {
            if (description[i] == '{')
            {
                string statDescription = "";

                for (int j = i + 1; j < description.Length; j++)
                {
                    if (description[j] == '}')
                    {
                        string[] stat = statDescription.Split('-');

                        switch (stat[0])
                        {
                            case "S":
                                {
                                    statDescription = statsData[stat[1]]._value.ToString();

                                    break;
                                }
                            case "C":
                                {
                                    statDescription = this.playerCharacterData()._statsData[stat[1]]._value.ToString();

                                    break;
                                }
                        }

                        fixedDescription += statDescription;

                        i = j;

                        break;
                    }
                    else if (j == description.Length - 1)
                    {
                        #if UNITY_EDITOR
                        Debug.Log("ManaBursts/Initialize/Missing } on stat description");
                        #endif

                        i = j;

                        break;
                    }
                    else
                        statDescription += description[j];
                }
            }
            else
                fixedDescription += description[i];
        }

        descriptionText.text = fixedDescription;
    }

    public override void Play()
    {
        base.Play();
    }
}