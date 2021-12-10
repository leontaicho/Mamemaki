using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private PlayerAction PlayerScript;
    [Header("カメラのプレイヤーからの距離")]
    [SerializeField]
    private float CameraPosY;
    [SerializeField]
    private float CameraPosZ;

    //カメラの位置変更用変数
    private float degree = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = Player.GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //カメラが見る場所
        Vector3 LookPos = new Vector3(Player.transform.position.x, Player.transform.position.y + 2, Player.transform.position.z);

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
        this.transform.LookAt(LookPos);

    }
}
