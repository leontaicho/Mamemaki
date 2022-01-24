using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("村人のHP")]
    public int villagerHP;
    [Header("村人が豆を投げるインターバル : 秒")]
    public float throwInterval;
    [Header("村人が投げる豆の数 : 個")]
    public int beansNum;
    [Header("村人の投げる豆の速さ")]
    public float beansSpeed;
    [Header("村人の投げる豆の威力")]
    public int beansDmg;
    [HideInInspector] public bool bossBattleFlg;    // ボス戦かどうかのフラグ
    [Header("ボス戦時の壁 : オブジェクト")]
    [SerializeField] private GameObject wallObj;
    [Header("ボス戦時の壁の数")]
    [SerializeField] private int wallNum;
    [Header("ボス戦時の壁の位置")]
    private Vector3[] wallPos;
    [Header("ボス戦時の壁の角度")]
    [SerializeField] private Vector3[] wallRotation;


    // Start is called before the first frame update
    void Start()
    {
        bossBattleFlg = false;
        wallPos = new Vector3[wallNum];
        wallRotation = new Vector3[wallNum];

        for (int i = 0;i < wallNum; ++i)
        {
            wallPos[i] = new Vector3(0, 1, i * 5);
            SetWalls(i);
        }
    }

    public void StartBossBattle()
    {
        bossBattleFlg = true;
    }

    // 壁生成処理
    public void SetWalls(int ID)
    {
        GameObject Walls;
        Walls = Instantiate(wallObj, wallPos[ID], wallObj.transform.rotation);
        Walls.transform.rotation = Quaternion.Euler(wallRotation[ID]);
        Walls.GetComponent<WallAction>().SetID(ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
