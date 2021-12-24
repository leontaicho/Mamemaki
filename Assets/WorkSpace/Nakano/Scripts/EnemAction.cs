using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class EnemAction : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        Hit,
    }

    [Header("敵のステータス")]
    [SerializeField]
    private float Speed = 5;
    [Header("自身の体力")]
    [SerializeField]
    private float HP;

    private Rigidbody myRB;
    private Animator myAnim; // 自身のアニメーション

    private EnemyState state;    // 自身のステータス
    [Header("土煙 : エフェクト")]
    [SerializeField] private GameObject PatSmoke;
    ParticleSystem.MainModule SmokeMain; //砂煙の本体
    Human_Action[] Human_script = new Human_Action[10];
    public GameObject[] Human_obj;
    Human_Action PL_Obj;
    GameObject Player_obj; //プレイヤーを取得
    bool isDead = false;

    [Header("死亡後に消えるまでの時間")]
    [SerializeField]
    private float deathTime = 3.0f;

    //攻撃した後のフリーズ時間
    [SerializeField]
    private float freezeTime = 0.5f;

    [Header("攻撃開始距離")]
    [SerializeField]
    float Dis = 3f;

    int TargetCount = 0;


    void Start()
    {

        state = EnemyState.Walk;
        for (int i = 0; i < Human_obj.Length; ++i)
        {
            Human_script[i] = Human_obj[i].GetComponent<Human_Action>();
        }

        TargetCount = 0;

        //プレイヤーを取得
        Player_obj = GameObject.FindGameObjectWithTag("Player");
        PL_Obj = Player_obj.GetComponent<Human_Action>();
        //自身のRigidbodyを取得
        myRB = this.gameObject.GetComponent<Rigidbody>();
        //自身のアニメーションを取得
        myAnim = this.gameObject.GetComponent<Animator>();

        SmokeMain = PatSmoke.GetComponent<ParticleSystem>().main;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "club" && !isDead)
        {
            if (HP < 0)
            {
                Destroy(gameObject, deathTime);
            }
            else
            {
                HP--;
            }

        }
    }

    void Attack(Transform Pos)
    {
        float dis = Vector3.Distance(this.gameObject.transform.position, Pos.transform.position);
        Pos.transform.LookAt(PL_Obj.transform.position);
        myRB.velocity = transform.forward * Speed;
        myAnim.SetFloat("Speed", Speed);
        if (Dis > dis)
        {
            myAnim.SetFloat("Speed", 0);
            myRB.velocity = Vector3.zero;
            myAnim.SetBool("Attack", true); //攻撃開始
        }

        
      
    }



    void Update()
    {

        //switch (state)
        //{
        //    case EnemyState.Walk:

        //        break;

        //    case EnemyState.Hit:
        //        break;

        //    case EnemyState.Attack:
        //        break;

        //    default:
        //        break;
        //}



        if (Human_script[TargetCount].isDaed == true)
        {
            TargetCount++;
        }

        else if (PL_Obj.isDaed == true)
        {
            //回転値を取得
            this.transform.LookAt(PL_Obj.transform.position);
            myRB.velocity = transform.forward * Speed;
            //    myRB.velocity = new Vector3(transform.position.x, 0, transform.position.z) + transform.forward * Speed;
        }

        else
        {
            //回転値を取得
            this.transform.LookAt(Human_obj[TargetCount].gameObject.transform.position);
            myRB.velocity = transform.forward * Speed;
            myAnim.SetFloat("Speed", Speed);
        }

    }
}
