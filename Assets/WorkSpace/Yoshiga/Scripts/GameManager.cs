using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CameraAction cameraScript;
    [Header("村人のHP")]
    [SerializeField]private int villagerHP;
    public int VillagerHP => villagerHP;
    [Header("村人が豆を投げるインターバル : 秒")]
    [SerializeField] private float throwInterval;
    public float ThrowInterval => throwInterval;
    [Header("村人が投げる豆の数 : 個")]
    [SerializeField] private int beansNum;
    public int BeansNum => beansNum;
    [Header("村人の投げる豆の速さ")]
    [SerializeField] private float beansSpeed;
    public float BeansSpeed => beansSpeed;
    [Header("村人の投げる豆の威力")]
    [SerializeField] private int beansDmg;
    public int BeansDmg => beansDmg;
    private bool bossBattleFlg;    // ボス戦かどうかのフラグ
    public bool BossBattleFlg => bossBattleFlg;
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
        cameraScript = this.gameObject.GetComponent<CameraAction>();
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
        cameraScript.SetBoss();
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
