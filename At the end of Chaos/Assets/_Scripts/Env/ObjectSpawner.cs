using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    List<List<GameObject>> objects;
    public List<string> objectNames;
    WaitForSeconds waitTime;
    Transform anchor;

    int rand;
    Vector3 rot;
    Vector3 vect;

    void Start()
    {
        objects = new List<List<GameObject>>();
        objectNames = new List<string>();
        waitTime = new WaitForSeconds(0.3f);
        anchor = GameObject.Find("MasterGameObject").transform.Find("Objects");
        vect.x = 70f;
        vect.y = 0f;
        //z == -24 ~ 20

        Object[] data = Resources.LoadAll("Prefabs/Nature");

        foreach (Object obj in data) {
            objectNames.Add(obj.name);
        }

        StartCoroutine(Spawn());
    }

    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        if (GameManager.instance.timeState == TimeState.night || GameManager.instance.timeState == TimeState.nightStart)
        {
            rand = Random.Range(0, objectNames.Count);
            vect.z = Random.Range(-20f, -2f);
            vect.y = Random.Range(-0.3f, 0f);
            rot.y = Random.Range(0f, 360f);
            
            GameObject obj = PhotonNetwork.InstantiateRoomObject("Prefabs/Nature/" + objectNames[rand], vect, Quaternion.Euler(rot));
            obj.transform.SetParent(anchor);

            rand = Random.Range(0, objectNames.Count);
            vect.z = Random.Range(16f, 2f);
            obj = PhotonNetwork.InstantiateRoomObject("Prefabs/Nature/" + objectNames[rand], vect, Quaternion.Euler(rot));
            obj.transform.SetParent(anchor);
        }

        yield return waitTime;
        StartCoroutine(Spawn());
    }
}
