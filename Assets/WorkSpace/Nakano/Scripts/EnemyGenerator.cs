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
    [SerializeReference] float x, y, z;

    [Header("敵のリスポーン数")]
    [SerializeReference] float E_Num;

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
                enemy.transform.position = new Vector3(x + E_Num, y, z);

            }

            Destroy(this.gameObject);
        }
    }
   

    void Update()
    {

        for (int i = 0; i < E_Num; ++i)
        {
            //enemyをインスタンス化する(生成する)
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = new Vector3(x + E_Num, y, z);

        }
        Destroy(this.gameObject);
    }
}
