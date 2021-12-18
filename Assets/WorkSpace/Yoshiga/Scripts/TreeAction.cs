using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAction : MonoBehaviour
{
    [Header("木が何回殴られたら倒れるか : 回数")]
    [SerializeField] private int HP;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
