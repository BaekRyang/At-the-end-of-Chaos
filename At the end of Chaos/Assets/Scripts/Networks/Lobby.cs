using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";

    public Text connectionInfoText;
    public Button joinBtn;

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinBtn.interactable = false;
        Debug.Log("������");
        connectionInfoText.text = "������...";
    }

    public override void OnConnectedToMaster()
    {
        joinBtn.interactable = true;
        connectionInfoText.text = "Online";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinBtn.interactable = false;
        connectionInfoText.text = "Offline : ���� ��õ� ��...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinBtn.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "���ӿ� ������...";
            PhotonNetwork.JoinRandomRoom();
        } else
        {
            connectionInfoText.text = "Offline : ���� ������ ���� ����. �� �õ���...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    public override void OnJoinRandomFailed(short returnCode, string msg)
    {
        connectionInfoText.text = "�� ������ �������...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "�����";
        PhotonNetwork.LoadLevel("GameScene");
    }

    void Update()
    {
        
    }
}
