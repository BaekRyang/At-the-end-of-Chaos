using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviourPun, IPunObservable
{
    GameObject train;
    Rigidbody rigid;

    //������ ���
    [SerializeField] GameObject target;

    [SerializeField]
    int health = 10;
    float def = 0;

    [SerializeField] float speed;
    int attackPoint;

    //���ݱ����� ��� �ð�
    float attackDelay = 1f;
    //���� ������ ��� �ð�
    [SerializeField] float attackDelayTime;

    public PhotonView pv;


    void Start()
    {
        train = GameObject.Find("TrainManager");
        rigid = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        //target = train.GetComponent<TrainManager>().GetTrain(GameManager.instance.trainCount);
        //rigid.velocity = (target.transform.position - transform.position).normalized * Time.deltaTime * speed;
        //Vector3 vec = transform.position;
        //vec.x -= Time.deltaTime * 2f;
        //transform.position = vec;
        target = train.GetComponent<TrainManager>().GetTrain(GameManager.instance.trainCount);
        Vector3 zombieToTarget = target.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(zombieToTarget.x, zombieToTarget.z) * Mathf.Rad2Deg, 0));
        if (GameManager.instance.timeState == TimeState.nightStart)
        {
            Stronger();
        }
        else if (GameManager.instance.timeState != TimeState.night)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        target = train.GetComponent<TrainManager>().GetTrain(GameManager.instance.trainCount);
        Vector3 zombieToTarget = target.transform.position - transform.position;
        //transform.position += ((target.transform.position - transform.position).normalized * speed + Vector3.left) * Time.deltaTime;
        zombieToTarget = zombieToTarget.normalized * speed + Vector3.left;
        zombieToTarget.y = rigid.velocity.y;
        rigid.velocity = zombieToTarget;

        //Vector3 vec = transform.position;
        //vec.x -= Time.deltaTime;
        //transform.position = vec;
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (collision.gameObject == train)
        //{
        //    attackDelayTime += Time.deltaTime;
        //    if (attackDelayTime > attackDelay)
        //    {
        //        attackDelayTime = 0f;
        //        AttackFromZombie();
        //    }
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == train)
        {
            attackDelayTime = 0f;
        }
    }

    //void AttackFromPlayer(float damage, float defPen)
    //{
    //    health -= (int)((1f - def * (1f - defPen)) * damage);
    //    if (health <= 0) Die();
    //}

    void AttackToTrain()
    {
        Debug.Log("���� ����!");
    }

    void Die()
    {
        StopAllCoroutines();
        PhotonNetwork.Destroy(gameObject);
    }

    void Stronger()
    {
        health = (int)(health * 1.1f);
        def += 0.1f;
    }

    [PunRPC]
    void AttackFromPlayer(float damage, float pierce, Vector3 vec)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(vec.normalized * 10f, ForceMode.Impulse);
        health -= (int)((1f - def * (1f - pierce) * damage));
        if (health <= 0) Die();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
    