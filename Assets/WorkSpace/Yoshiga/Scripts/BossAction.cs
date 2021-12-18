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
    [Header("攻撃頻度 : 秒")]
    [SerializeField] private float attackInterval;
    public float interval;   // 攻撃のインターバル
    [HideInInspector]public State bossState;    // ボスのステータス

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myAnim = this.gameObject.GetComponent<Animator>();
        bossState = State.Idle;
        interval = attackInterval;
    }

    public void AttackFinish()
    {
        bossState = State.Idle;
        interval = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(bossState == State.Idle)
        {
            if(interval > 0)
            {
                interval -= Time.deltaTime;
                if(interval <= 0)
                {
                    interval = 0;
                    myAnim.SetTrigger("Attack");
                    bossState = State.Attack;
                }
            }
            this.gameObject.transform.LookAt(player.transform.position);
        }

        
    }
}
