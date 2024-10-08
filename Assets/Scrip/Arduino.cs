using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com5", 115200);
    //public float[] num;
    private float Pos;
    public float time;
    private string Newdate;
    private int Olddate;
    //public string[] WORD ,newdata;//1.¨¤«×2.x¶b3.Y¶b4.Z¶b
    // Start is called before the first frame update

    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        time= Time.time;
        
        if (sp.IsOpen)
            {
                try
                {
                    // for (int i = 0; i < 4; i++)
                    // {

                    Newdate = sp.ReadLine();
                    
               
                //if(transpos == Olddate)
                //{
                //    transpos = 0;
                //  }
                // int uCount = sp.BytesToRead;
                //if (uCount != 0)
                //{
                //    byte[] btAryBuffer = new byte[uCount];
                //    sp.Read(btAryBuffer, 0, uCount);

                //}
                //}
                //if (transpos != Olddate)
                  //  {
                       
                    //}
                //else
                //{
                //    this.transform.Rotate(0, 0, 0, Space.Self);
                //}
                    
                }
                catch
                {

                }
            float Pos = float.Parse(Newdate);

            int transpos = (int)Pos;
            //Olddate = transpos;
            Debug.Log(transpos);
            if(transpos != null)
            {
                if(transpos != Olddate)
                {
                    if (transpos > 0 || transpos < 0)
                    {
                        int gap=transpos - Olddate;
                        //this.transform.position = new Vector3(Pos, 0, 0);
                        // this.transform.Rotate(Vector3.right * this.transform.rotation.x * Pos);
                        //                    this.transform.rotation = this.transform.rotation +  Quaternion.Angle;
                        //if (time % 1.0f ==0)
                        //{
                        this.transform.Rotate(0,-gap/3.5f,0 , Space.Self);
                        //}
                    }
                }
                Olddate = transpos;
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
    

