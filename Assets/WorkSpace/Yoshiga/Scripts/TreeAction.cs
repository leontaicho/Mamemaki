using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAction : MonoBehaviour
{
    [Header("木が何回殴られたら倒れるか : 回数")]
    [SerializeField] private int HP;
    [Header("ヒットエフェクト : オブジェクト")]
    [SerializeField] private GameObject HitEffect;
    private GameObject player;
    private PlayerAction playerScript;
    private bool fallDownFlg;   // 倒れているかのフラグ
    private float acceleration; // 加速度
    private float rotationAngle; // 回転角
    private CapsuleCollider myCollider; // 自身の当たり判定

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerAction>();
        fallDownFlg = false;
        acceleration = 0;
        myCollider = this.gameObject.GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon" && playerScript.CanAttack)
        {
            --HP;
            // ヒットエフェクトを生成
            Instantiate(HitEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);
            if(HP <= 0)
            {
                fallDownFlg = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // 倒れる処理
        if(fallDownFlg)
        {
            acceleration += Time.deltaTime / 5;
            rotationAngle += acceleration;
          
            // 回転処理
            transform.Rotate(new Vector3(acceleration, 0, 0));
            
            if (rotationAngle >= 90.0f)
            {
                fallDownFlg = false;
                myCollider.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            fallDownFlg = true;
        }
    }
}
