using Attributes;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldUIManager : UIManager
{
    #region Components

    [Foldout("Components (BattlefieldUIManager)/External")]
    [SerializeField] protected RecordPanel recordPanel;
    [Foldout("Components (BattlefieldUIManager)/External")]
    [SerializeField] protected InfoPanel playerInfoPanel;
    [Foldout("Components (BattlefieldUIManager)/External")]
    [SerializeField] protected FeedbackPanel playerFeedbackPanel;
    [Foldout("Components (BattlefieldUIManager)/External")]
    [SerializeField] protected Image opponentImage;
    [Foldout("Components (BattlefieldUIManager)/External")]
    [SerializeField] protected InfoPanel opponentInfoPanel;
    [Foldout("Components (BattlefieldUIManager)/External")]
    [SerializeField] protected FeedbackPanel opponentFeedbackPanel;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerInfoPanel.Initialize();

        playerFeedbackPanel.Initialize();

        opponentInfoPanel.Initialize();

        opponentFeedbackPanel.Initialize();
    }

    public void SetPlayerCharacter(CharacterData playerCharacterData)
    {
        playerInfoPanel.SetCharacterData(playerCharacterData);

        playerFeedbackPanel.SetCharacterData(playerCharacterData);
    }

    public void SetOpponentCharacter(CharacterData opponentCharacterData)
    {
        opponentImage.sprite = opponentCharacterData._sprite;

        opponentInfoPanel.SetCharacterData(opponentCharacterData);

        opponentFeedbackPanel.SetCharacterData(opponentCharacterData);
    }

    public void SetPlayerTurn(bool value)
    {
        playerInfoPanel.SetOnTurn(value);
    }

    public void SetOpponentTurn(bool value)
    {
        opponentInfoPanel.SetOnTurn(value);
    }

    public void UpdatePlayerStat(string name)
    {
        switch (name)
        {
            case "Lives":
            {
                playerInfoPanel.UpdateLives();

                break;
            }
            case "Armor":
            {
                playerFeedbackPanel.UpdateArmor();

                break;
            }
            case "Actions Per Turn":
            {
                playerInfoPanel.UpdateActionsPerTurn();

                break;
            }
            case "Mana":
            {
                playerInfoPanel.UpdateMana();

                break;
            }
            case "Evilness":
            {
                playerFeedbackPanel.UpdateEvilness();

                break;
            }
        }
    }

    public void UpdateOpponentStat(string name)
    {
        switch (name)
        {
            case "Lives":
            {
                opponentInfoPanel.UpdateLives();

                break;
            }
            case "Armor":
            {
                opponentFeedbackPanel.UpdateArmor();

                break;
            }
            case "Actions Per Turn":
            {
                opponentInfoPanel.UpdateActionsPerTurn();

                break;
            }
            case "Mana":
            {
                opponentInfoPanel.UpdateMana();

                break;
            }
            case "Evilness":
            {
                opponentFeedbackPanel.UpdateEvilness();

                break;
            }
        }
    }

    public void UpdatePlayerState(string name)
    {
        switch (name)
        {
            case "Confused":
            {
                playerFeedbackPanel.UpdateConfused();

                break;
            }
            case "Betray The Owner":
            {
                playerFeedbackPanel.UpdateBetrayTheOwner();

                break;
            }
        }
    }

    public void UpdateOpponentState(string name)
    {
        switch (name)
        {
            case "Confused":
            {
                opponentFeedbackPanel.UpdateConfused();

                break;
            }
            case "Betray The Owner":
            {
                opponentFeedbackPanel.UpdateBetrayTheOwner();

                break;
            }
        }
    }

    public void UpdatePlayerDeck(int deltaCards)
    {
        playerInfoPanel.UpdateDeck(deltaCards);
    }

    public void UpdateOpponentDeck(int deltaCards)
    {
        opponentInfoPanel.UpdateDeck(deltaCards);
    }

    public void ShowRecord(Sprite recordSprite)
    {
        recordPanel.ShowRecord(recordSprite);
    }
}
