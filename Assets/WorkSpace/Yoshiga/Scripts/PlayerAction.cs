using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Hit,
        Dead,
    }

    private GameManager manager;    // GameManager
    [Header("プレイヤーのステータス")]
    [SerializeField]
    private float Speed = 0;
    [SerializeField]
    private float HP;
    [Header("プレイヤーの無敵時間 : 秒")]
    [SerializeField] private float IntervalTime;
    private float moveX = 0;
    private float moveZ = 0;
    private Rigidbody myRB;
    private Animator myAnim; // 自身のアニメーション
    private GameObject MainCamera;
    private Vector3 cameraForward; // カメラの向き   
    private Vector3 moveForward; // プレイヤーの向き
    private State state;    // 自身のステータス
    public State playerState => state;
    [Header("土煙 : エフェクト")]
    [SerializeField] private GameObject PatSmoke;
    ParticleSystem.MainModule SmokeMain; //砂煙の本体  
    private float invincibleTime;    // プレイヤーの無敵時間
    [Header("自身の体のメッシュ")]
    [SerializeField] private SkinnedMeshRenderer[] myMesh = new SkinnedMeshRenderer[3];
    private bool canAttack;    // 攻撃判定がオンかどうかのフラグ
    public bool CanAttack => canAttack;
    private Vector3 bossBattleIniPos = new Vector3(10, 0.5f, 375);
    private float bossDistance = 0.0f;
    private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        //自身のRigidbodyを取得
        myRB = this.gameObject.GetComponent<Rigidbody>();
        //自身のアニメーションを取得
        myAnim = this.gameObject.GetComponent<Animator>();
        //カメラを取得
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        manager = MainCamera.GetComponent<GameManager>();
        SmokeMain = PatSmoke.GetComponent<ParticleSystem>().main;
        canAttack = false;
    }

    public void OnDamage(int Dmg)
    {
        HP -= Dmg;
        if(HP <= 0)
        {
            myAnim.SetTrigger("Death");
            myRB.velocity = Vector3.zero;
            state = State.Dead;
        }
        else
        {
            myAnim.SetTrigger("Hit");
            invincibleTime = IntervalTime;
            myRB.velocity = Vector3.zero;
            state = State.Hit;
        }       
    }

    private void AttackStart()
    {
        canAttack = true;
    }

    private void FinishAttack()
    {
        state = State.Idle;
        canAttack = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Beans")
        {
            Destroy(other.gameObject);
            if(invincibleTime <= 0)
            {
                OnDamage(manager.BeansDmg);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BattleZone" && !manager.BossBattleFlg)
        {
            manager.StartBossBattle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.BossBattleFlg)
        {
            if(boss == null)
            {
                boss = GameObject.FindGameObjectWithTag("Boss");
            }
            bossDistance = Vector3.Distance(this.transform.position, boss.transform.position);
            // 離れすぎると瞬間移動
            if(bossDistance > 30)
            {
                this.gameObject.transform.position = bossBattleIniPos;
            }
        }

        // 点滅処理
        if(invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
            if(invincibleTime <= 0)
            {
                invincibleTime = 0;
                state = State.Idle;
                foreach (var i in myMesh)
                {
                    i.enabled = true;
                }
            }

            if(invincibleTime % 0.3f < 0.2)
            {
                foreach(var i in myMesh)
                {
                    i.enabled = true;
                }
            }
            else
            {
                foreach (var i in myMesh)
                {
                    i.enabled = false;
                }
            }
        }

        moveX = 0;
        moveZ = 0;       
        if (state != State.Dead && state != State.Hit)
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");                   
        }
        Vector3 dir = new Vector3(moveX, 0, moveZ);
        myAnim.SetFloat("Speed", dir.magnitude);
        // 移動方向への量に応じて砂ぼこりを制御する。
        SmokeMain.startSize = dir.sqrMagnitude * 1.5f;

        // 攻撃処理
        if (Input.GetButtonDown("BumperR") && state != State.Hit && state != State.Attack && state != State.Dead)
        {
            myAnim.SetTrigger("Attack");
            myRB.velocity = Vector3.zero;
            state = State.Attack;
        }
    }

    void FixedUpdate()
    {
        if (myAnim.GetFloat("Speed") > 0 && state == State.Idle)
        {
            // 移動している
            state = State.Walk;
        }
        else if(myAnim.GetFloat("Speed") <= 0 && state == State.Walk)
        {
            // 待機状態
            state = State.Idle;
            myRB.velocity = Vector3.zero;
        }

        // 移動していたら
        if (state == State.Walk)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            moveForward = cameraForward * moveZ + Camera.main.transform.right * moveX;
            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            myRB.velocity = moveForward * Speed + new Vector3(0, myRB.velocity.y, 0);
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
             
    }
}
