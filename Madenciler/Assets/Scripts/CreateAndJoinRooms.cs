using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomInput;
    public TMP_InputField joinRoomInput;

    public Button createRoomButton;
    public Button joinRoomButton;

    private void Update()
    {
        createRoomButton.interactable = createRoomInput.text.Trim().Length != 0;
        joinRoomButton.interactable = joinRoomInput.text.Trim().Length != 0;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}

