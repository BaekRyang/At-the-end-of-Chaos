using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServerManager : MonoBehaviour, IPunObservable
{
    public string[] playerNames = new string[4];

    GameObject LobbyObject;
    GameObject MainCamera;

    void Start()
    {
        PhotonNetwork.UseRpcMonoBehaviourCache = true;
        //�ʿ��� ������Ʈ�� ã�´�.
        LobbyObject = GameObject.Find("LobbyManager");
        MainCamera = GameObject.Find("Main Camera");

        //�κ��� �ִ� �÷��̾� �̸��� ������� �κ�� ���ش�.
        playerNames = LobbyObject.GetComponent<Lobby>().playerNames;
        Destroy(LobbyObject);

        for (int i = 0; i < 4; i++)
        {
            if (playerNames[i] == PhotonNetwork.NickName)
            {
                //�Ѿ�� �г��Ӱ� �� �г����� ���ؼ� �´� ��ȣ�� ĳ���͸� �����.
                string prefabName = "Player_" + (i + 1);
                //�׸��� ī�޶� ������ ���� ���� ĳ���͸� ����Ѵ�.
                MainCamera.GetComponent<CameraMovement>().player = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);

            }
        }
        
    }

    void Update()
    {
        
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GameManager.instance.seed);
        }
        else
        {
            GameManager.instance.seed = (int)stream.ReceiveNext();
        }
    }
}
