using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAction : MonoBehaviour
{
    [Header("ブロック配列")]
    [SerializeField] private GameObject[] Boxes = new GameObject[10];
    [Header("敵が箱を攻撃するタイミング")]
    [SerializeField] private float blowAwayDelay;
    [Header("爆発エフェクト : オブジェクト")]
    [SerializeField] private GameObject MineEffect;
    private Vector3[] initPos = new Vector3[10];
    private Vector3[] initRotation = new Vector3[10];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Boxes.Length; ++i)
        {
            initPos[i] = Boxes[i].transform.position;
            initRotation[i] = new Vector3(Boxes[i].transform.rotation.x,
                                          Boxes[i].transform.rotation.y,
                                          Boxes[i].transform.rotation.z);
        }
    }

    public void OnDamage()
    {
        Invoke("BlowBox", blowAwayDelay);
        
    }

    private void InitBoxes()
    {
        for(int i = 0;i < Boxes.Length; ++i)
        {
            Boxes[i].transform.position = initPos[i];
            Boxes[i].transform.rotation = Quaternion.Euler(initRotation[i].x,initRotation[i].y,initRotation[i].z);
        }
    }

    private void BlowBox()
    {
        Instantiate(MineEffect, transform.position, transform.rotation);
        foreach (var i in Boxes)
        {
            //i.gameObject.GetComponent<BoxCollider>().enabled = false;
            i.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f,1.0f),
                                                                        2.0f,
                                                                        Random.Range(-1.0f, 1.0f)) * 5.0f, ForceMode.Impulse);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
