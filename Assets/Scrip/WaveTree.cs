using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class WaveTree : MonoBehaviour
{
    // Start is called before the first frame update
    public SerialPort sp = new SerialPort("com5", 115200);
    //public float[] num;

    public float WaveVector,Vector,timer,oringnaltimer;
    private string Newdate;
    //public string[] WORD ,newdata;//1.¨¤«×2.x¶b3.Y¶b4.Z¶b
    // Start is called before the first frame update

    void Start()
    {
        sp.Open();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (sp.IsOpen)
            {
                Newdate = sp.ReadLine();
                WaveVector = float.Parse(Newdate);
            }
        }
        catch
        {

        }
        finally
        {
            timer += Time.deltaTime;
        }
       if(oringnaltimer != timer) 
        {
            if(WaveVector != null) 
            {
                this.transform.Rotate(Vector*WaveVector, 0, Vector * WaveVector, Space.Self);
            }
            oringnaltimer = timer;
        }
    }
}
