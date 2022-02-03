using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAction : MonoBehaviour
{
    //== HP ==//
    public Image imgHPbar;
    public Text txtHP;
    public GameObject HP;
    Animator HpAnim;

    //== MiniMap ==//
    public Image imgPlayer;
    public Image[] imgTarget = new Image[5];
    public Text txtHuman;

    GameObject Player;
    GameObject[] Enemy;
    public float radoarLength = 20.0f;
    RectTransform[] rt = new RectTransform[5];
    Vector2 offset;
    float r = 0.4f;

    //== Icon ==//
    public Image imgBoss;
    public Image[] imgEnemy = new Image[5];
    public Image[] imgDamage = new Image[5];
    public Sprite imgDeadEnemy;
    Animator[] AnimDamage = new Animator[5];
    int MaxEnemy;
    int enemyRemain;
    int oldEnemyRemain;

    //== デバッグ用 ==//
    int CurrentHP = 100;
    int maxHP = 100;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        for(int i = 0; i < 5; i++)
        {
            AnimDamage[i] = imgDamage[i].gameObject.GetComponent<Animator>();
            rt[i] = imgTarget[i].GetComponent<RectTransform>();
            offset = imgPlayer.GetComponent<RectTransform>().anchoredPosition;
            imgDamage[i].gameObject.SetActive(false);
            imgEnemy[i].gameObject.SetActive(false);
        }

        HpAnim = HP.gameObject.GetComponent<Animator>();

        HpChange(CurrentHP, maxHP);
        EnemyRemainSet();
    }

    //HP表示
    /*
        @param HP       現在のHP
        @param MaxHP    最大HP
    */
    void HpChange(int HP, int MaxHP)
    {
        float fill = (float)HP / (float)MaxHP;
        imgHPbar.fillAmount = fill;
        if (fill <= 0.2f)
        {
            imgHPbar.color = new Color(1.0f, 0.3f, 0.4f, 1.0f);
        }
        else if (fill <= 0.5f)
        {
            imgHPbar.color = new Color(1.0f, 0.95f, 0.1f, 1.0f);
        }
        else
        {
            imgHPbar.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        txtHP.text = "HP   " + HP.ToString().PadLeft(3, '0') + " / " + MaxHP.ToString().PadLeft(3, '0');
        if (HP < 0.0f)
        {
            txtHP.text = "HP   000 / " + MaxHP.ToString().PadLeft(3,'0');
        }

        HpAnim.SetTrigger("Damage");
    }

    //敵の残数表示
    /*
        @param EnemyRemain  残りの敵の数
    */
    void EnemyRemain(int EnemyRemain)
    {
        for(int i = 0; i < MaxEnemy; i++)
        {
            if (i < EnemyRemain)
            {
                imgEnemy[i].gameObject.SetActive(true);
                imgTarget[i].gameObject.SetActive(true);
            }
            else
            {
                imgEnemy[i].sprite = imgDeadEnemy;
                imgDamage[i].gameObject.SetActive(true);
                imgTarget[i].gameObject.SetActive(false);
                AnimDamage[i].SetTrigger("Damage");
                Destroy(imgDamage[i].gameObject, 1.0f);
            }
        }
    }


    //敵の残数表示セット
    void EnemyRemainSet()
    {
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        MaxEnemy = Enemy.Length;
        enemyRemain = MaxEnemy;

        for (int i = 0; i < MaxEnemy; i++)
        {
            imgEnemy[i].gameObject.SetActive(true);
            imgTarget[i].gameObject.SetActive(true);
        }
    }


    //ミニマップ表示
    void MiniMap()
    {
        Vector3[] enemyDir = new Vector3[enemyRemain];
        for(int i = 0; i < enemyRemain; i++)
        {
            enemyDir[i] = Enemy[i].transform.position;
            enemyDir[i].y = Player.transform.position.y;
            enemyDir[i] = Enemy[i].transform.position - Player.transform.position;

            enemyDir[i] = Quaternion.Inverse(Player.transform.rotation) * enemyDir[i];
            enemyDir[i] = Vector3.ClampMagnitude(enemyDir[i], radoarLength);

            rt[i].anchoredPosition = new Vector2((enemyDir[i].x * r) + offset.x, enemyDir[i].z * r + offset.y);
        }
    }

    //ボスエンカウント
    public void BossEncount()
    {
        imgBoss.color = new Color(1.0f, 0.4f, 0.4f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //== HP確認用 ==//
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentHP -= 10;
            HpChange(CurrentHP, maxHP);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentHP += 10;
            HpChange(CurrentHP, maxHP);
        }

        //== ボスエンカウント ==//
        if(Input.GetKeyDown(KeyCode.B))
        {
            BossEncount();
        }

        //敵の残数確認用
        oldEnemyRemain = enemyRemain;
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        enemyRemain = Enemy.Length;
        if (enemyRemain != oldEnemyRemain)
        {
            EnemyRemain(enemyRemain);
        }

        //ミニマップ表示
        MiniMap();
    }
}
