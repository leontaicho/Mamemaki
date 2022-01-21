using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    //敵プレハブ
    public GameObject enemyPrefab;
    //人間プレハブ
    public GameObject humanPrefab;

    [Header("敵の座標")]
    [SerializeField] GameObject[] enemySpawnPointObjs;
    [Header("人間の座標")]
    [SerializeField] GameObject[] humanSpawnPointObjs;

    [SerializeField] HumanList humanList;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //humanListに前の情報が残っていたら消す
            if (humanList.Humans.Count != 0)
            {
                foreach (var human in humanList.Humans)
                {
                    if (human != null)
                    {
                        Destroy(human);
                    }
                }
                humanList.Humans.Clear();
            }
            //人間スポーン
            for (int i = 0; i < humanSpawnPointObjs.Length; ++i)
            {
                //humanをインスタンス化する(生成する)
                 humanList.Humans.Add(Instantiate(humanPrefab, humanSpawnPointObjs[i].transform.position, Quaternion.identity));
            }
            //敵スポーン
            for (int i = 0; i < enemySpawnPointObjs.Length; ++i)
            {
                //enemyをインスタンス化する(生成する)
                Instantiate(enemyPrefab, enemySpawnPointObjs[i].transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
   

}
