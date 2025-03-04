using UnityEngine;

public class Ambush : Card, IImprovable
{
    public void Improve(CharacterData characterData, string newDescription)
    {
        tagsData.Add("Piercing Damage");

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

                                        statDescription = (statsData[stat[0]]._value += float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("-"))
                                    {
                                        stat = stat[1].Split("-");

                                        statDescription = (statsData[stat[0]]._value -= float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("*"))
                                    {
                                        stat = stat[1].Split("*");

                                        statDescription = (statsData[stat[0]]._value *= float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("/"))
                                    {
                                        stat = stat[1].Split("/");

                                        statDescription = (statsData[stat[0]]._value /= float.Parse(stat[1])).ToString();
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

                                        statDescription = (characterData._statsData[stat[0]]._value += float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("-"))
                                    {
                                        stat = stat[1].Split("-");

                                        statDescription = (characterData._statsData[stat[0]]._value -= float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("*"))
                                    {
                                        stat = stat[1].Split("*");

                                        statDescription = (characterData._statsData[stat[0]]._value *= float.Parse(stat[1])).ToString();
                                    }
                                    else if (stat[1].Contains("/"))
                                    {
                                        stat = stat[1].Split("/");

                                        statDescription = (characterData._statsData[stat[0]]._value /= float.Parse(stat[1])).ToString();
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

        descriptionText.text = fixedDescription;
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
