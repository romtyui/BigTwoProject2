using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Runtime.CompilerServices;

public class Arduinoreserve : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com5", 38400);
    public SerialPort sp4 = new SerialPort("com4", 38400);
    private Thread serialThread;
    public int WaveVector;
    public string sp4date;
    public string confirm;

    public LightingCode lightingcode;
    public GameObject rain;
    public bool raincheck;
    public GameObject raindot;
    public bool raindotcheck;
    private Material material;
    //public Behaviour throwrock;

    public bool triggerLighting = false;  // 用於主執行緒更新狀態的旗標
    
    
    /*-----------------搖樹----------------------*/
    public GameObject Wavetree;
    public bool Wavetreecheck;
    private Material treematerial;
    public bool wavetreecheck;
    /*-----------------搖樹----------------------*/

    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        material = renderer.material;
        material.SetFloat("_Ripple_Strengh", 0);
        /*-----------------搖樹----------------------*/
        Renderer treerenderer = Wavetree.GetComponent<Renderer>();
        treematerial = treerenderer.material;

        /*-----------------搖樹----------------------*/
        
        try
        {
            sp.Open();
            sp4.Open();
            serialThread = new Thread(ReadSerialData);
            serialThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open Serial Port: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // 在主執行緒中檢查旗標，並根據需要啟動 lightingcode
        if (triggerLighting)
        {
            StartCoroutine(TriggerLightingEffect());
            triggerLighting = false;  // 重置旗標
            // lightingcode.enabled = false;   
        }

        if (raincheck /*|| raindotcheck*/ == true)
        {
            rain.SetActive(true);
            material.SetFloat("_Ripple_Strengh", 0.1f);
        }

        if(wavetreecheck == true)
        {
            treematerial.SetFloat("_WindDensity", 0.41f);
            treematerial.SetFloat("_WindMovement", 3.4f);
            treematerial.SetFloat("_WindStrength", 2.6f);
            Vector2 offset = treematerial.GetVector("_Direction");
            treematerial.SetVector("_Direction", new Vector4(WaveVector, 0.1f, 0, 0));
            treematerial.SetFloat("_BlendStrength", 5f);
        }
    }

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                confirm = sp.ReadLine();
                sp4date = sp.ReadLine();

                int.TryParse(sp4date, out WaveVector);//把sp4date轉成int放到waveVrctor

                Debug.Log("confirm:" + confirm);
                Debug.Log("Newdata:" + WaveVector);

                // 檢查條件是否滿足，然後設定旗標
                if (confirm == "T")
                {
                    triggerLighting = true;
                }

                if(confirm =="R")
                {
                    raincheck = true;
                    //raindotcheck = true;
                    
                    //WaveTree.Newdate = Newdate;
                }

                if(WaveVector != 0)
                {
                    wavetreecheck = true;
                }
            }
            Thread.Sleep(10); // 控制讀取頻率，避免過度占用CPU
        }
    }

    private IEnumerator TriggerLightingEffect()
    {
        lightingcode.enabled = true;
        yield return new WaitForSeconds(0.5f);  // 例如延遲0.1秒
        lightingcode.enabled = false;
    }
}
