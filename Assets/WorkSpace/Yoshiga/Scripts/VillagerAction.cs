using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAction : MonoBehaviour
{
    private List<GameObject> targetList;   // 豆を投げつけるターゲットリスト
    private GameObject target;  // 

    // Start is called before the first frame update
    void Start()
    {
        // リストに要素を入れる
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var i in enemys)
        {
            targetList.Add(i);
        }
        targetList.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    void FinishThrow()
    {
        foreach (var i in targetList)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
