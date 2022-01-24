using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAction : MonoBehaviour
{
    [Header("ブロック配列")]
    [SerializeField] private GameObject[] Boxes = new GameObject[10];
    [Header("敵が箱を攻撃するタイミング")]
    [SerializeField] private float blowAwayDelay;
    [Header("爆発エフェクト : オブジェクト")]
    [SerializeField] private GameObject MineEffect;
    private bool breakFlg = false;  // 破壊されているかのフラグ
    private int id; // 自身が何番目の壁なのかの変数
    [Header("リスポーン時間 : 秒")]
    [SerializeField] private float respawnInterval;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void SetID(int ID)
    {
        id = ID;
    }

    public void OnDamage()
    {
        if(!breakFlg)
        {
            Invoke("BlowBox", blowAwayDelay);
        }
    }

    private void BlowBox()
    {
        Instantiate(MineEffect, transform.position, transform.rotation);
        foreach (var i in Boxes)
        {
            i.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f,1.0f),
                                                                        2.0f,
                                                                        Random.Range(-1.0f, 1.0f)) * 5.0f, ForceMode.Impulse);
            i.layer = 9;
        }
        breakFlg = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(breakFlg)
        {
            respawnInterval -= Time.deltaTime;
            if(respawnInterval <= 0)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>().SetWalls(id);
                foreach (var i in Boxes)
                {
                    Destroy(i);
                }
                Destroy(this);
            }
        }
    }
}
