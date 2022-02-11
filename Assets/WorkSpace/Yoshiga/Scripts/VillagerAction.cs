using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAction : MonoBehaviour
{
    private int HP; // 村人のHP
    public int hp => HP;
    private CapsuleCollider myCollider;
    private List<GameObject> targetList = new List<GameObject>();   // 豆を投げつけるターゲットリスト
    private GameObject target;  // 投げる相手を格納している変数
    private float interval; // 投げるタイミングのインターバル
    private Animator myAnim;    // 自身のアニメーション
    private GameManager manager;    // ゲームマネージャー
    private int beansNum;   // 投げる豆の数
    private GameObject beans;   // 豆オブジェクト
    private List<GameObject> myBeansList = new List<GameObject>();  // 自身が投げた豆のリスト
    [Header("豆のスポーンポジション : オブジェクト")]
    [SerializeField]private GameObject spawnPos;    // 豆のスポーンポジション
    
    // Start is called before the first frame update
    void Start()
    { 
        myAnim = this.gameObject.GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        interval = Random.Range(1.0f,5.0f);
        beansNum = manager.BeansNum;
        beans = (GameObject)Resources.Load("Beans");
        myCollider = this.gameObject.GetComponent<CapsuleCollider>();
        // リストに要素を入れる
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var i in enemys)
        {
            targetList.Add(i);
        }
        targetList.Add(GameObject.FindGameObjectWithTag("Player"));
        HP = manager.VillagerHP;
        FindTarget();
    }

    // ダメージ処理
    public void OnDamage(int Dmg)
    {
        HP -= Dmg;
        if(HP <= 0)
        {
            HP = 0;
            myCollider.enabled = false;
            myAnim.SetTrigger("Dead");
        }
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
        // 豆リストをクリア
        foreach(var i in myBeansList)
        {
            if(i != null)
            {
                Destroy(i);
            }
        }
        myBeansList.Clear();

        for(int i = 0; i < beansNum; ++i)
        {
            GameObject b;
            Rigidbody bRB;
            Vector3 throwForce;           
            b = Instantiate(beans,spawnPos.transform.position,spawnPos.transform.rotation);
            bRB = b.GetComponent<Rigidbody>();
            throwForce = this.gameObject.transform.forward 
                       + new Vector3(this.gameObject.transform.forward.x + Random.Range(-0.5f,0.5f),
                                     Random.Range(1.0f, 2.0f), 
                                     this.gameObject.transform.forward.z);
            bRB.AddForce(throwForce * manager.BeansSpeed, ForceMode.Impulse);
            myBeansList.Add(b);
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
                interval = manager.ThrowInterval;
                FindTarget();
                Invoke("ThrowBeans", 0.5f);
            }
        }
    }
}
