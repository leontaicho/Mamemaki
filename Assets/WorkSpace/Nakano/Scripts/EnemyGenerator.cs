using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    //敵プレハブ
    public GameObject enemyPrefab;

    [Header("敵の座標")]
    [SerializeField] Vector3 Pos;

    [Header("敵のリスポーン数")]
    [SerializeField] float E_Num;

    bool Hit = false;

    void Start()
    {
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < E_Num; ++i)
            {
                //enemyをインスタンス化する(生成する)
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.transform.position = new Vector3(Pos.x + E_Num, Pos.y, Pos.z);
            }
            Destroy(gameObject);
        }
    }
   

}
