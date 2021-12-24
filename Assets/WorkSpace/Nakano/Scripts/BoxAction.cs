using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class BoxAction : MonoBehaviour
{
    Rigidbody myRB;
    float Speed = 50;

    public GameObject Target;
    void Start()
    {
        myRB = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        this.transform.LookAt(Target.transform);
        myRB.velocity = transform.forward * Speed;
    }
}
