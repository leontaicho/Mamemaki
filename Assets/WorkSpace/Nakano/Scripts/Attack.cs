using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
    EnemAction EA;
    public GameObject enemy;

    void Start()
    {
        enemy = transform.root.gameObject;
        EA = enemy.GetComponent<EnemAction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (EA.CanAttack)
        {
            if(other.tag == "Player" || other.tag == "Villager")
            {
                other.SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    void Update()
    {
        
    }
}
