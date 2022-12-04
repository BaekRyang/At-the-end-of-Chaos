using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using System;

public class ZombieManager : MonoBehaviour
{
    //������� �� ���� �ۼ�Ʈ�� �����ϴ� �Ŵ���

    public static ZombieManager instance;

    string[] idleZombie = new string[3];
    public TrainManager trainManager;
    public Transform spawnSpot;
    [SerializeField] List<GameObject> zombieList = new List<GameObject>();
    [SerializeField] float spawnDistance;
    [SerializeField] float spawnTimeInterval = 1f;

    [NonSerialized] public float health = 100;
    [NonSerialized] public float def = 1;

    GameObject zombieAnchor;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
    }

    void Start()
    {
        idleZombie[0] = "zombie_body_standing";
        idleZombie[1] = "zombie_man_standing";
        idleZombie[2] = "zombie_woman_standing";
        trainManager = GameObject.Find("TrainManager").GetComponent<TrainManager>();
        zombieAnchor = GameObject.Find("ZombieManager");
    }

    void Update()
    {

    }

    public IEnumerator SpawnZombie()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            while (true)
            {
                int angle = UnityEngine.Random.Range(0, 360);
                float x = Mathf.Cos(angle * Mathf.Deg2Rad) * spawnDistance;
                float z = Mathf.Sin(angle * Mathf.Deg2Rad) * spawnDistance;
                Vector3 pos = trainManager.GetTrain(GameManager.instance.trainCount).transform.position + new Vector3(x, 1f, z);
                zombieList.Add(PhotonNetwork.InstantiateRoomObject(idleZombie[UnityEngine.Random.Range(0, 3)], pos, Quaternion.identity));
                zombieList[zombieList.Count - 1].transform.parent = zombieAnchor.transform;
                yield return new WaitForSeconds(spawnTimeInterval);
            }
        }
    }

    public void DestroyZombies()
    {
        for (int i = 0; i < zombieAnchor.transform.childCount; i++)
        {
            zombieAnchor.transform.GetChild(i).GetComponent<Zombie>().Die();
        }
    }

    public void StrongerZombies()
    {
        health *= 1.1f;
        //def += 0.1f;
        //���̵� ������ ü�¸����� �ϰ� ������ Ư�� ���� ���´�.
    }
}
