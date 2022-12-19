using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.right * GameManager.instance.groundSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bot")
        {
            PhotonNetwork.Destroy(gameObject);
        } else if (collision.gameObject.tag == "Objects")
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }
}
