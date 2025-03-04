using Attributes;
using UnityEngine;

public class MainUIManager : UIManager
{
    #region Components

    [Foldout("Components (MainUIManager)/External")]
    [SerializeField] protected Panel loginPanel;
    [Foldout("Components (MainUIManager)/External")]
    [SerializeField] protected Panel chooseCharacterPanel;
    [Foldout("Components (MainUIManager)/External")]
    [SerializeField] protected Panel lobbyPanel;
    [Foldout("Components (MainUIManager)/External")]
    [SerializeField] protected Panel awaitingMatchPanel;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        loginPanel.Initialize();

        chooseCharacterPanel.Initialize();

        lobbyPanel.Initialize();

        awaitingMatchPanel.Initialize();

        OpenPanel(loginPanel);
    }

    public void OpenChooseCharacterPanel()
    {
        SwapCurrentPanel(chooseCharacterPanel);
    }

    public void OpenLobbyPanel()
    {
        SwapCurrentPanel(lobbyPanel);
    }

    public void OpenAwaitingMatchPanel()
    {
        SwapCurrentPanel(awaitingMatchPanel);
    }
}
