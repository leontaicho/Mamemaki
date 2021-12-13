﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Attack,
        Hit,
    }

    [Header("プレイヤーのステータス")]
    [SerializeField]
    private float Speed = 0;
    [SerializeField]
    private float HP;
    private float moveX = 0;
    private float moveZ = 0;
    private Rigidbody myRB;
    private Animator myAnim; // 自身のアニメーション
    private GameObject MainCamera;
    private Vector3 cameraForward; // カメラの向き   
    private Vector3 moveForward; // プレイヤーの向き
    private State state;    // 自身のステータス
    [Header("土煙 : エフェクト")]
    [SerializeField] private GameObject PatSmoke;
    ParticleSystem.MainModule SmokeMain; //砂煙の本体

    // Start is called before the first frame update
    void Start()
    {
        //自身のRigidbodyを取得
        myRB = this.gameObject.GetComponent<Rigidbody>();
        //自身のアニメーションを取得
        myAnim = this.gameObject.GetComponent<Animator>();
        //カメラを取得
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        SmokeMain = PatSmoke.GetComponent<ParticleSystem>().main;
    }

    public void OnDamage(int Dmg)
    {
        HP -= Dmg;
        myAnim.SetTrigger("Hit");
    }

    private void FinishAttack()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(moveX, 0, moveZ);
        myAnim.SetFloat("Speed", dir.magnitude);

        // 移動方向への量に応じて砂ぼこりを制御する。
        SmokeMain.startSize = dir.sqrMagnitude * 1.5f;

        // 攻撃処理
        if (Input.GetButtonDown("BumperR"))
        {
            myAnim.SetTrigger("Attack");
            myRB.velocity = Vector3.zero;
            state = State.Attack;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            OnDamage(10);
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
