using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com3", 115200);
    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        string date = sp.ReadLine();
        
    }

}
