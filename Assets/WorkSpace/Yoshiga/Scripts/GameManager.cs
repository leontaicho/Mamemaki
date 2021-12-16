using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("村人のHP")]
    public int villagerHP;
    [Header("村人が豆を投げるインターバル : 秒")]
    public float throwInterval;
    [Header("村人が投げる豆の数 : 個")]
    public int beansNum;
    [Header("村人の投げる豆の速さ")]
    public float beansSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
