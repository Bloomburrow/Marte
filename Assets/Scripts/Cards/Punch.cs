using UnityEngine;

public class Punch : Card, IEvolvable
{
    public void Evolve<T1>(CharacterData characterData, string newName, Sprite newIllustration, string newDescription)
    {
        Punch evolution = (Punch)gameObject.AddComponent(typeof(T1));

        evolution.gameObject.name = newName;

        evolution.identificationNumber = identificationNumber;

        evolution.gameplayIdentificationNumber = gameplayIdentificationNumber;

        evolution.type = type;

        evolution.data = new CardData(data);

        evolution.playerCharacterData = playerCharacterData;

        evolution.opponentCharacterData = opponentCharacterData;

        evolution.tagsData = tagsData;

        evolution.statsData = new StatsData(statsData);

        evolution.battlefieldManager = battlefieldManager;

        evolution.battlefieldUIManager = battlefieldUIManager;

        evolution.cardsEffectsManager = cardsEffectsManager;

        evolution.transform = transform;

        evolution.spriteRenderer = spriteRenderer;

        evolution.outlineSpriteRenderer = outlineSpriteRenderer;

        evolution.iconSpriteRenderer = iconSpriteRenderer;

        evolution.nameTitleSpriteRenderer = nameTitleSpriteRenderer;

        evolution.nameText = nameText;

        evolution.nameText.text = newName;

        evolution.nameMeshRenderer = nameMeshRenderer;

        evolution.illustrationSpriteRenderer = illustrationSpriteRenderer;

        evolution.illustrationSpriteRenderer.sprite = newIllustration;

        evolution.descriptionSpriteRenderer = descriptionSpriteRenderer;

        evolution.descriptionText = descriptionText;

        string description = newDescription;

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
                                    if (stat[1].Contains("+"))
                                    {
                                        stat = stat[1].Split("+");

                                        statDescription = (statsData[stat[0]]._value + float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("-"))
                                    {
                                        stat = stat[1].Split("-");

                                        statDescription = (statsData[stat[0]]._value - float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("*"))
                                    {
                                        stat = stat[1].Split("*");

                                        statDescription = (statsData[stat[0]]._value * float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("/"))
                                    {
                                        stat = stat[1].Split("/");

                                        statDescription = (statsData[stat[0]]._value / float.Parse(stat[1])).ToString();
                                    }
                                    else
                                        statDescription = statsData[stat[1]]._value.ToString();

                                    break;
                                }
                            case "C":
                                {
                                    if (stat[1].Contains("+"))
                                    {
                                        stat = stat[1].Split("+");

                                        statDescription = (characterData._statsData[stat[0]]._value + float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("-"))
                                    {
                                        stat = stat[1].Split("-");

                                        statDescription = (characterData._statsData[stat[0]]._value - float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("*"))
                                    {
                                        stat = stat[1].Split("*");

                                        statDescription = (characterData._statsData[stat[0]]._value * float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("/"))
                                    {
                                        stat = stat[1].Split("/");

                                        statDescription = (characterData._statsData[stat[0]]._value / float.Parse(stat[1])).ToString();
                                    }
                                    else
                                        statDescription = characterData._statsData[stat[1]]._value.ToString();

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
                        Debug.Log("Card/Initialize/Missing } on stat description");
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

        evolution.descriptionText.text = fixedDescription;

        evolution.descriptionMeshRenderer = descriptionMeshRenderer;

        Destroy(this);
    }

    protected override void OnDissolved()
    {
        base.OnDissolved();

        CharacterData playerCharacterData = this.playerCharacterData();

        CharacterData opponentCharacterData = this.opponentCharacterData();

        if (playerCharacterData._statesData.Contains("Confused"))
        {
            bool confuse = Random.Range(0, 101) < 50;

            if (confuse)
                opponentCharacterData = playerCharacterData;
        }

        if (playerCharacterData._statesData.Contains("Betray The Owner"))
        {
            bool betrayed = Random.Range(0, 101) < 50;

            if (betrayed)
                opponentCharacterData = playerCharacterData;
        }

        if (tagsData.Contains("Piercing Damage"))
            opponentCharacterData._statsData["Lives"]._value -= statsData["Damage"]._value * battlefieldManager._cardsPlayedInTurn;
        else
        {
            float amor = opponentCharacterData._statsData["Armor"]._value;

            float damage = statsData["Damage"]._value;

            if (amor > 0)
            {
                opponentCharacterData._statsData["Armor"]._value -= damage * battlefieldManager._cardsPlayedInTurn;

                float leftDamage = amor - damage * battlefieldManager._cardsPlayedInTurn;

                if (leftDamage > 0)
                    opponentCharacterData._statsData["Lives"]._value -= leftDamage;
            }
            else
                opponentCharacterData._statsData["Lives"]._value -= statsData["Damage"]._value * battlefieldManager._cardsPlayedInTurn;
        }

        battlefieldManager.UpdateUIStat(opponentCharacterData, "Lives");

        battlefieldManager.UpdateUIStat(opponentCharacterData, "Armor");
    }
}
