using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.SceneManagement;
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
    [Header("ボス : オブジェクト")]
    [SerializeField] private GameObject bossObj;
    [Header("ボス戦時の壁 : オブジェクト")]
    [SerializeField] private GameObject wallObj;
    [Header("ボス戦時の壁の位置")]
    [SerializeField]private Vector3[] wallPos = new Vector3[8];
    [Header("ボス戦時の壁の角度")]
    [SerializeField] private Vector3[] wallRotation = new Vector3[8];
    private Vector3 bossSpawnPos = new Vector3(-9, 0.5f, 375);
    

    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.FLOOR_BGM);
        cameraScript = this.gameObject.GetComponent<CameraAction>();
        bossBattleFlg = false;
        for (int i = 0; i < 8; ++i)
        {
            SetWalls(i);
        }
    }

    public void StartBossBattle()
    {
        bossBattleFlg = true;
        BGMManager.Instance.Play(BGMPath.BOSS_BATTLE_BGM);
        Instantiate(bossObj, bossSpawnPos, bossObj.transform.rotation);
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

    // タイトルに移行
    private void ChangeTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void SceneChange()
    {
        Invoke("ChangeTitle", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
