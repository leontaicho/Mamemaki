using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class EnemAction : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Attack,
        Hit,
    }

    [Header("敵のステータス")]
    [SerializeField]
    private float Speed = 0;
    [SerializeField]
    private float HP;

    private Rigidbody myRB;
    private Animator myAnim; // 自身のアニメーション

    private State state;    // 自身のステータス
    [Header("土煙 : エフェクト")]
    [SerializeField] private GameObject PatSmoke;
    ParticleSystem.MainModule SmokeMain; //砂煙の本体
    Human_Action[] Human_script = new Human_Action[10];
    public GameObject[] Human_obj;
    GameObject Player_obj; //プレイヤーを取得
    Human_Action Player;

    Transform Target;
    int TargetCount = 0;


    void Start()
    {
        for(int i = 0; i < Human_obj.Length; ++i)
        {
            Human_script[i] = Human_obj[i].GetComponent<Human_Action>();
        }

        //プレイヤーを取得
        Player_obj = GameObject.FindGameObjectWithTag("Player");
        Player = Player_obj.GetComponent<Human_Action>();

        //自身のRigidbodyを取得
        myRB = this.gameObject.GetComponent<Rigidbody>();
        //自身のアニメーションを取得
        myAnim = this.gameObject.GetComponent<Animator>();

        SmokeMain = PatSmoke.GetComponent<ParticleSystem>().main;
    }


    void EnemyController()
    {

        if(Human_script[TargetCount].isDaed == true)
        {
            TargetCount++;
        }

        else if(Player.isDaed == true)
        {
            Target = Player.gameObject.transform;
            transform.LookAt(Target, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z));
            myRB.velocity = new Vector3() + transform.forward * Speed;
        }

        else 
        {
            Target = Human_script[TargetCount].gameObject.transform;
            transform.LookAt(Target, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z));
            myRB.velocity = new Vector3() + transform.forward * Speed;
        }

    }



    void FIxedUpdate()
    {

        EnemyController();

    }
}
