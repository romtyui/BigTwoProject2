using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using Palmmedia.ReportGenerator.Core.Common;

public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com3", 115200);
    public float[] num;
    public string[]newdata;//1.¨¤«×2.x¶b3.Y¶b4.Z¶b
    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++) 
        {
            string date = sp.ReadLine();
            Debug.Log(date);
            
            newdata[i] = null;
            newdata[i] += date;
            num[i] = float.Parse(newdata[i]);
            /*if (WORD[i] != newdata[i])i
                {
                          WORD[i] = newdata[i];
                      }
                      // num[i] = float.Parse(WORD[i]);
                      */
        }
        this.transform.position = new Vector3(num[1]/10, num[2]/10);
    }

}
