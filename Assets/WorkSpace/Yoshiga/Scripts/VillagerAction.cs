using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAction : MonoBehaviour
{
    private List<GameObject> targetList = new List<GameObject>();   // 豆を投げつけるターゲットリスト
    private GameObject target;  // 投げる相手を格納している変数
    private float interval; // 投げるタイミングのインターバル
    private Animator myAnim;    // 自身のアニメーション
    private GameManager manager;    // ゲームマネージャー
    private int beansNum;   // 投げる豆の数
    private GameObject beans;   // 豆オブジェクト

    // Start is called before the first frame update
    void Start()
    {
        myAnim = this.gameObject.GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        interval = Random.Range(1.0f,5.0f);
        beansNum = manager.beansNum;
        beans = (GameObject)Resources.Load("Beans");
        // リストに要素を入れる
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var i in enemys)
        {
            targetList.Add(i);
        }
        targetList.Add(GameObject.FindGameObjectWithTag("Player"));
        FindTarget();
    }

    // 誰が一番近いか判定するための処理
    void FindTarget()
    {
        float distance = 0.0f;
        float saveDistace = 100.0f;
        foreach (var i in targetList)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, i.transform.position);
            if (saveDistace > distance)
            {
                saveDistace = distance;
                target = i;
            }
        }
        transform.LookAt(target.transform.position);
    }

    void ThrowBeans()
    {
        for(int i = 0; i < beansNum; ++i)
        {
            GameObject b;
            b = Instantiate(beans);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // インターバルを減らす
        if(interval > 0)
        {
            interval -= Time.deltaTime;
            if(interval <= 0 && target)
            {
                myAnim.SetTrigger("Attack");
                interval = manager.throwInterval;
                FindTarget();
                Invoke("ThrowBeans", 0.5f);
            }
        }
    }
}
