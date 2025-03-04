using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Events

    public event Action onJoinedLobby;
    public event Action onJoinedRoom;
    public event Action onJoinRoomFailed;
    public event Action onPlayerEnteredRoom;

    #endregion

    public void Connect(string nickName)
    {
        PhotonNetwork.NickName = nickName;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        #if UNITY_EDITOR
        Debug.Log($"PhotonManager/OnConnectedToMaster");
        #endif

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() 
    {
        onJoinedLobby?.Invoke();

        #if UNITY_EDITOR
        Debug.Log($"PhotonManager/OnJoinedLobby");
        #endif   
    }

    public override void OnLeftLobby() 
    {
        PhotonNetwork.JoinLobby();
    }

    public void JoinRandomOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2
        };

        PhotonNetwork.JoinRandomOrCreateRoom(typedLobby: TypedLobby.Default, roomName: PhotonNetwork.NickName, roomOptions: roomOptions);
    }

    public override void OnJoinedRoom() 
    {
        onJoinedRoom?.Invoke();

        #if UNITY_EDITOR
        Debug.Log($"PhotonManager/OnJoinedRoom");
        #endif
    }

    public override void OnJoinRoomFailed(short returnCode, string message) 
    {
        onJoinRoomFailed?.Invoke();

        #if UNITY_EDITOR
        Debug.Log($"PhotonManager/OnJoinedROnJoinRoomFailedoom/Message: {message}");
        #endif
    }

    public override void OnLeftRoom() 
    {
        #if UNITY_EDITOR
        Debug.Log($"PhotonManager/OnLeftRoom");
        #endif
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) 
    {
        onPlayerEnteredRoom?.Invoke();

        #if UNITY_EDITOR
        Debug.Log($"PhotonManager/OnPlayerEnteredRoom");
        #endif

        onJoinedRoom = null;
    }
}
