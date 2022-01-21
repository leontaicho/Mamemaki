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

    [SerializeField] HumanList humanList;

    [Header("死亡後に消えるまでの時間")]
    [SerializeField]
    private float deathTime = 3.0f;

    //攻撃した後のフリーズ時間
    [SerializeField]
    private float freezeTime = 0.5f;

    [Header("プレイヤーの無敵時間 : 秒")]
    [SerializeField] private float IntervalTime;
    private float invincibleTime;    // プレイヤーの無敵時間

    [Header("攻撃開始距離")]
    [SerializeField]
    float Dis = 3f;

    int TargetCount = 0;

    public bool CanAttack = false;

    private float Elapsed = 5.0f;


    void Start()
    {
        humanList = GameObject.FindGameObjectWithTag("Human_List").GetComponent<HumanList>();
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

    public void OnDamage(int Dmg)
    {
        HP -= Dmg;
        myAnim.SetTrigger("Hit");
        invincibleTime = IntervalTime;
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

    private void AttackStart()
    {
        CanAttack = true;
        
    }

    private void FinishAttack()
    {
        CanAttack = false;
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


        if (Human_script[TargetCount] != null)
        {
            if (Human_script[TargetCount].isDaed == true)
            {
                TargetCount++;
            }

            //else if (PL_Obj.isDaed == true)
            //{



            //}

            else
            {
                float dis = Vector3.Distance(this.gameObject.transform.position, Player_obj.transform.position);

                if (dis < Dis)
                {
                    Elapsed += Time.deltaTime;
                    myAnim.SetFloat("Speed", 0);
                    myRB.velocity = Vector3.zero;
                    if (Elapsed > 2)
                    {
                        myAnim.SetTrigger("Attack"); //攻撃開始
                        Elapsed = 0.0f;
                    }
                }
                else
                {
                    this.transform.LookAt(Player_obj.transform.position);
                    myRB.velocity = transform.forward * Speed;
                    myAnim.SetFloat("Speed", Speed);
                    Elapsed = 5.0f;
                }
            }
        }

    }
}
