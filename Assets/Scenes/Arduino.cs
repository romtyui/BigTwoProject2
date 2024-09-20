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
    public float time;
    public string[] WORD ,newdata;//1.¨¤«×2.x¶b3.Y¶b4.Z¶b
    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    string date = sp.ReadLine();
                    //time += Time.deltaTime;
                    Debug.Log(date);

                   // int uCount = sp.BytesToRead;
                    //if (uCount != 0)
                    //{
                    //    byte[] btAryBuffer = new byte[uCount];
                    //    sp.Read(btAryBuffer, 0, uCount);

                    //}
                }
            }
            catch
            {

            }
        }
        
            /*
            newdata[i] = null;
            newdata[i] += date;
            num[i] = float.Parse(newdata[i]);
            */
           
        }

        /*
        if (WORD[1] != newdata[1] || WORD[2] != newdata[2])
        {
            this.transform.position = new Vector3(num[1] / 10, num[2] / 10);
        }
        // num[i] = float.Parse(WORD[i]);*/
    }
    

