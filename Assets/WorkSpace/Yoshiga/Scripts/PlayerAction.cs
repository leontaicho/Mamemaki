using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    [SerializeField]
    private float Speed = 0;
    [SerializeField]
    private float HP;
    private float MoveX = 0;
    private float MoveZ = 0;
    private Rigidbody MyRB;
    private GameObject MainCamera;
    private Vector3 cameraForward; // カメラの向き
    //プレイヤーの向き
    private Vector3 moveForward;

    // Start is called before the first frame update
    void Start()
    {
        //自身のRigidbodyを取得
        MyRB = this.gameObject.GetComponent<Rigidbody>();
        //カメラを取得
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(MoveX, 0, MoveZ);
    }

    void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        moveForward = cameraForward * MoveZ + Camera.main.transform.right * MoveX;
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        MyRB.velocity = moveForward * Speed + new Vector3(0, MyRB.velocity.y, 0);
        transform.rotation = Quaternion.LookRotation(moveForward);
    }
}
