using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{
    public enum State
    {
        Idle,
        Attack,
        Hit,
        angry,
    }

    private GameObject player;
    private Animator myAnim;
    [Header("敵のHP")]
    [SerializeField] private int HP;
    [Header("攻撃頻度 : 秒")]
    [SerializeField] private float attackInterval;
    [Header("混乱時間 : 秒")]
    [SerializeField] private float stunInterval;
    private float interval;   // 攻撃のインターバル
    private float hitInterval;    // 混乱インターバル
    [HideInInspector] public State bossState;    // ボスのステータス
    [Header("混乱エフェクト : オブジェクト")]
    [SerializeField] private GameObject stunEffect;
    [Header("前方向のRayの長さ")]
    [SerializeField] private float rayLength;
    private bool blowFlg;   // 壁が吹き飛んだかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myAnim = this.gameObject.GetComponent<Animator>();
        bossState = State.Idle;
        interval = attackInterval;
        blowFlg = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            --HP;
            bossState = State.Hit;
            myAnim.SetTrigger("Damage");
        }
    }

    // 混乱処理
    public void StartStun()
    {
        hitInterval = stunInterval;
        stunEffect.SetActive(true);
    }

    public void AttackFinish()
    {
        if (bossState != State.Hit)
        {
            blowFlg = false;
            bossState = State.Idle;
            interval = attackInterval;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (bossState)
        {
            case State.Idle:
                if (interval > 0)
                {
                    interval -= Time.deltaTime;
                    if (interval <= 0)
                    {
                        interval = 0;
                        myAnim.SetTrigger("Attack");
                        bossState = State.Attack;
                    }
                }
                this.gameObject.transform.LookAt(player.transform.position);
                break;
            case State.Attack:
                RaycastHit hit;
                Debug.DrawRay(gameObject.transform.position, this.gameObject.transform.forward * rayLength, Color.blue, 0.1f);
                if (Physics.Raycast(gameObject.transform.position, this.gameObject.transform.forward, out hit, rayLength))
                {
                    if (hit.collider.gameObject.tag == "Wall" && !blowFlg)
                    {
                        hit.collider.gameObject.GetComponent<WallAction>().OnDamage();
                        blowFlg = true;
                    }
                }
                break;
            case State.Hit:
                if (hitInterval > 0)
                {
                    hitInterval -= Time.deltaTime;
                    if (hitInterval <= 0)
                    {
                        myAnim.SetTrigger("FinishStun");
                        stunEffect.SetActive(false);
                        bossState = State.Idle;
                        // 攻撃頻度のリセット
                        interval = attackInterval;
                    }
                }
                break;
        }
    }
}
