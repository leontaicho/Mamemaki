using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private PlayerAction PlayerScript;
    private GameManager manager;    // ゲームマネージャー
    [Header("カメラのプレイヤーからの距離")]
    [SerializeField]
    private float CameraPosY;
    [SerializeField]
    private float CameraPosZ;    
    private float degree = 0; // カメラの位置変更用変数
    private GameObject bossObj; // ボス
    private Vector3 LookPos;    // カメラが見る位置

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = Player.GetComponent<PlayerAction>();
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    public void SetBoss()
    {
        bossObj = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(manager.BossBattleFlg)
        {
            LookPos = new Vector3(bossObj.transform.position.x, bossObj.transform.position.y + 2, bossObj.transform.position.z);
            this.transform.position = Player.transform.position 
                                      + new Vector3(Player.transform.position.x - bossObj.transform.position.x, 
                                                    0.0f,
                                                    Player.transform.position.z - bossObj.transform.position.z).normalized * 5.0f
                                      + new Vector3(0.0f,CameraPosY,0.0f);
        }
        else
        {
            LookPos = new Vector3(Player.transform.position.x, Player.transform.position.y + 2, Player.transform.position.z);
            float RstickX = Input.GetAxis("HorizontalR");

            //カメラの回転処理
            if (RstickX > 0.19)
            {
                degree -= RstickX / 10;
            }
            else if (RstickX < -0.19)
            {
                degree -= RstickX / 10;
            }
            float CameraSin = Mathf.Sin(degree);
            float CameraCos = Mathf.Cos(degree);
            this.transform.position = Player.transform.position + new Vector3(CameraPosZ * CameraSin, CameraPosY, CameraPosZ * -CameraCos);
        }

        
        this.transform.LookAt(LookPos);
    }
}
