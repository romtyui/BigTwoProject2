using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Arduino : MonoBehaviour
{
    public GameObject remind;
    public GameObject final;
    public SerialPort sp = new SerialPort("com3", 9600);
    private bool remember = false;
    private bool recover = false;
    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
