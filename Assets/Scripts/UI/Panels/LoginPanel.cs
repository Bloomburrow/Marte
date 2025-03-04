using Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : Panel
{
    #region Variables & Properties

    [Foldout("Variables & Properties")]
    [SerializeField] Sprite background;

    #endregion

    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] GameDataManager gameDataManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] PhotonManager photonManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] MainUIManager mainUIManager;

    [Foldout("Components/External")]
    [SerializeField] TMP_InputField nickNameInputField;
    [Foldout("Components/External")]
    [SerializeField] Button LoginButton;

    #endregion

    public override void Initialize()
    {
        gameDataManager = this.GetSingleton<GameDataManager>();

        photonManager = this.GetSingleton<PhotonManager>();

        mainUIManager = this.GetSingleton<MainUIManager>();
    }

    public override void OnOpen()
    {
        mainUIManager.SetBackground(background);

        mainUIManager.SetBackgroundColor(Color.white);

        nickNameInputField.text = "";

        LoginButton.interactable = true;
    }

    public void Connect()
    {
        if (photonManager
            &&
            nickNameInputField.text != string.Empty)
        {
            LoginButton.interactable = false;

            photonManager.onJoinedLobby += OnJoinedLobby;

            photonManager.Connect(nickNameInputField.text);
        }
    }

    void OnJoinedLobby()
    {
        LoginButton.interactable = true;

        if (mainUIManager)
        {
            if (gameDataManager._hasCharacter)
                mainUIManager.OpenLobbyPanel();
            else
                mainUIManager.OpenChooseCharacterPanel();
        }

        photonManager.onJoinedLobby -= OnJoinedLobby;
    }
}
