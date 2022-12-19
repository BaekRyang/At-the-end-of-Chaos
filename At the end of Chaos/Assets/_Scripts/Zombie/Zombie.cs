using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviourPun, IPunObservable
{
    protected GameObject train;
    protected GameObject ground;
    protected Rigidbody rigid;

    //������ ���
    [SerializeField] protected GameObject target;

    [SerializeField] public int health;
    [SerializeField] protected float def;
    [SerializeField] protected float speed;
    protected int attackPoint;

    //���ݱ����� ��� �ð�
    protected float attackDelay = 1f;
    //���� ������ ��� �ð�
    [SerializeField] protected float attackDelayTime;

    public bool targeting = true;

    public PhotonView pv;

    protected int takenDamage = 0;

    IEnumerator ie;


    void Start()
    {
        health = ((int)ZombieManager.instance.health);
        def = ZombieManager.instance.def;
        speed *= ZombieManager.instance.speedMultiplier;
        train = GameObject.Find("TrainManager");
        rigid = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    protected void Update()
    {
        //target = train.GetComponent<TrainManager>().GetTrain(GameManager.instance.trainCount);
        //rigid.velocity = (target.transform.position - transform.position).normalized * Time.deltaTime * speed;
        //Vector3 vec = transform.position;
        //vec.x -= Time.deltaTime * 2f;
        //transform.position = vec;
        target = train.GetComponent<TrainManager>().GetTrain(GameManager.instance.trainCount);
        Vector3 zombieToTarget = target.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(zombieToTarget.x, zombieToTarget.z) * Mathf.Rad2Deg, 0));

        //if (GameManager.instance.timeState == TimeState.nightStart)
        //{
        //    Debug.Log("STR");
        //    Stronger();
        //}
        //else if (GameManager.instance.timeState != TimeState.night)
        //{
        //    Die();
        //}
    }

    protected virtual void FixedUpdate()
    {
        if (targeting)
        {
            target = train.GetComponent<TrainManager>().GetTrain(GameManager.instance.trainCount);
            Vector3 zombieToTarget = target.transform.position - transform.position;
            //transform.position += ((target.transform.position - transform.position).normalized * speed + Vector3.left) * Time.deltaTime;
            zombieToTarget = zombieToTarget.normalized * speed + Vector3.left;
            zombieToTarget.y = rigid.velocity.y;
            rigid.velocity = zombieToTarget;
        }

        //Vector3 vec = transform.position;
        //vec.x -= Time.deltaTime;
        //transform.position = vec;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            ie = Attack();
            StartCoroutine(ie);
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Train")
        {
            if (ie != null)
            {
                StopCoroutine(ie);
                ie = null;
            }
        }
    }

    //void AttackFromPlayer(float damage, float defPen)
    //{
    //    health -= (int)((1f - def * (1f - defPen)) * damage);
    //    if (health <= 0) Die();
    //}

    public void Die()
    {
        StopAllCoroutines();
        PhotonNetwork.Destroy(gameObject);
    }


    public void AttackFromPlayer(float damage, float pierce, Vector3 vec)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(vec.normalized * 10f, ForceMode.Impulse);

        takenDamage = (int)((1 - (def * (1 - pierce / 100)) / 100) * damage);
        health -= takenDamage;

        DamageDisplayManager.instance.Display(takenDamage, transform.position);

        //Debug.Log("Damage : " + "((1 - (" + def + " * (1 - " + pierce + " / 100)) / 100) * " + damage +")" + " =>(int) " + ((int)((1 - (def * (1 - pierce / 100)) / 100) * damage)) );
        //����� 1 = ������ 1% ����
        //���� 1 = ���ݷ��� 1% ����
        //���� �������� �Ҽ����� ����
        if (health <= 0) Die();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }

    protected virtual IEnumerator Attack()
    {
        while(true)
        {
            target.GetComponent<Train>().Attacked();
            SoundPlayer.instance.PlaySound(SoundPlayer.instance.TrainAttacked, transform.position);

            yield return new WaitForSeconds(3f);
        }
    }
}
