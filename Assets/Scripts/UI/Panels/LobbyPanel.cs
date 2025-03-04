using Attributes;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyPanel : Panel
{
    #region Components

    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] GameDataManager gameDataManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] PhotonManager photonManager;
    [Foldout("Components/Singletons")]
    [ReadOnly][SerializeField] MainUIManager mainUIManager;

    [Foldout("Components/External")]
    [SerializeField] TMP_Text currenciesText;
    [Foldout("Components/External")]
    [SerializeField] TMP_Text quickMatchTitleText;
    [Foldout("Components/External")]
    [SerializeField] Button playButton;

    #endregion

    public override void Initialize()
    {
        gameDataManager = this.GetSingleton<GameDataManager>();

        photonManager = this.GetSingleton<PhotonManager>();

        mainUIManager = this.GetSingleton<MainUIManager>();
    }

    public override void SetUIStyle(UIStyleData UIStyleData)
    {
        mainUIManager.SetBackground(UIStyleData._background);

        mainUIManager.SetBackgroundColor(UIStyleData._backgroundColor);

        currenciesText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
        {
            currenciesText.color = InvertColor(currenciesText.color);

            _fontColorIsInverted = true;
        }
        else if (_fontColorIsInverted)
            currenciesText.color = InvertColor(currenciesText.color);

        quickMatchTitleText.font = UIStyleData._textFont;

        if (UIStyleData._invertFontColor)
        {
            quickMatchTitleText.color = InvertColor(quickMatchTitleText.color);

            _fontColorIsInverted = true;
        }
        else if (_fontColorIsInverted)
            quickMatchTitleText.color = InvertColor(quickMatchTitleText.color);

        TMP_Text playButtonText = playButton.gameObject.GetComponentInChildren<TMP_Text>();

        playButtonText.font = UIStyleData._textFont;
    }

    public override void OnOpen()
    {
        if (gameDataManager._charactersData.Count > 0)
        {
            CharacterData currentCharacterData = gameDataManager._charactersData[gameDataManager._selectedCharacterIndex];

            SetUIStyle(currentCharacterData._UIStyleData);
        }

        playButton.interactable = true;
    }

    public void Play()
    {
        playButton.interactable = false;

        photonManager.onJoinedRoom += OnJoinedRoom;

        photonManager.onJoinRoomFailed += OnJoinedRoomFailed;

        photonManager.JoinRandomOrCreateRoom();
    }

    void OnJoinedRoom()
    {
        mainUIManager.OpenAwaitingMatchPanel();

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            PhotonNetwork.CurrentRoom.IsVisible = false;

            SceneManager.LoadScene("Battlefield");
        }
        else
            photonManager.onPlayerEnteredRoom += OnPlayerEnteredRoom;

        photonManager.onJoinedRoom -= OnJoinedRoom;

        photonManager.onJoinRoomFailed -= OnJoinedRoomFailed;
    }

    void OnJoinedRoomFailed()
    {
        playButton.interactable = true;

        photonManager.onJoinedRoom -= OnJoinedRoom;

        photonManager.onJoinRoomFailed -= OnJoinedRoomFailed;
    }

    void OnPlayerEnteredRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        PhotonNetwork.CurrentRoom.IsVisible = false;

        SceneManager.LoadScene("Battlefield");

        photonManager.onPlayerEnteredRoom -= OnPlayerEnteredRoom;
    }
}
