using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Runtime.CompilerServices;

public class Arduinoreserve : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com7", 38400);
    //public SerialPort sp4 = new SerialPort("com4", 38400);
    private Thread serialThread;
    public int WaveVector;
    public float Vectory;
    public string wavedate;
    public string confirm;

    /*-------------------------下雨-------------------------*/
    [Header("下雨")]
    public LightingCode lightingcode;
    public GameObject rain;
    public bool raincheck;
    public GameObject raindot;
    public bool raindotcheck;
    private Material rainmaterial;
    /*-------------------------下雨-------------------------*/

    public bool triggerLighting = false;  // 用於主執行緒更新狀態的旗標

    /*-----------------搖樹----------------------*/
    [Header("搖樹")]
    public int treechoice;
    private float FVectory;
    public bool treerechoice = false;
    public GameObject[] Wavetree;
    public Material[] treematerial;
    public Material leavesmaterial;
    public Material trunkmaterial;
    public bool wavetreecheck = false;
    public bool Zerowavetreecheck = false;
    /*-----------------搖樹----------------------*/

    /*----------------丟熊熊----------------------*/
    [Header("丟熊熊")]
    //public GameObject Rock;
    //public float RockX;
    //public float RockY;
    //public float RockZ;
    //public bool throwcheck = false;
    //public float throwrocktotalTime = 3;
    //public float gravty = 9.8f;
    //public float time;
    //private double powT;
    public GameObject bear;
    public GameObject fruit;
    public bool dropcheck = false;
    public float triggerTime=0;
    /*----------------丟熊熊----------------------*/

    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        rainmaterial = renderer.material;
        rainmaterial.SetFloat("_Ripple_Strengh", 0);
        /*-----------------搖樹----------------------*/
        treechoice = Random.Range(0, 3);
        Renderer treerenderer = Wavetree[treechoice].GetComponent<Renderer>();
        treematerial = treerenderer.materials;
        trunkmaterial = treematerial[0];
        leavesmaterial = treematerial[1];
        /*-----------------搖樹----------------------*/
        
        try
        {
            sp.Open();
            //sp4.Open();
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
            rainmaterial.SetFloat("_Ripple_Strengh", 0.1f);
        }
        
        if (dropcheck == true)
        {
            //RockX = Rock.transform.position.x;
            //RockY = Rock.transform.position.y;
            //RockZ = Rock.transform.position.z;
            //time = Time.deltaTime;
            //powT = (float)throwrocktotalTime * throwrocktotalTime;
            //Rock.transform.localPosition = new Vector3((RockX * time) / throwrocktotalTime, (RockY * time) / throwrocktotalTime, ((RockZ + (float)(0.5 * gravty * powT))/throwrocktotalTime) * (time - (float)(0.5 * gravty * time*time)));
            fruit.SetActive(true);
            //triggerTime = Time.deltaTime;
            //if(T>2)
            //{
                bear.SetActive(true);

            //}
            dropcheck = false;
        }
        if(Zerowavetreecheck == true)
        {
            leavesmaterial.SetFloat("_WindDensity", FVectory);
            leavesmaterial.SetFloat("_WindMovement", FVectory);
            leavesmaterial.SetFloat("_WindStrength", FVectory);
            Vector2 offset = trunkmaterial.GetVector("_Direction");
            trunkmaterial.SetVector("_Direction", new Vector4(FVectory, 0.1f, 0, 0));
            trunkmaterial.SetFloat("_BlendStrength", 5f);
            Zerowavetreecheck = false;
        }

        else if(wavetreecheck == true)
        {
            leavesmaterial.SetFloat("_WindDensity", WaveVector);
            leavesmaterial.SetFloat("_WindMovement", WaveVector);
            leavesmaterial.SetFloat("_WindStrength", WaveVector);
            Vector2 offset = trunkmaterial.GetVector("_Direction");
            trunkmaterial.SetVector("_Direction", new Vector4(WaveVector, 0.1f, 0, 0));
            trunkmaterial.SetFloat("_BlendStrength", 5f);
            wavetreecheck = false;
        }
        

        if(treerechoice == true)
        {
            treechoice = Random.Range(0, 3);
            Renderer treerenderer = Wavetree[treechoice].GetComponent<Renderer>();
            treematerial = treerenderer.materials;

            trunkmaterial = treematerial[0];
            leavesmaterial = treematerial[1];
            treerechoice = false;
        }
    }

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                confirm = sp.ReadLine();
                wavedate = sp.ReadLine();
                if(wavedate != "T")
                {
                    Vectory = float.Parse(wavedate);
                    WaveVector = (int)Vectory;
                }
                //int.TryParse(wavedate, out WaveVector);//把sp4date轉成int放到waveVrctor
                //int.TryParse(confirm, out WaveVector);
                Debug.Log("Vectory:" + Vectory);
                Debug.Log("WaveVectory:" + WaveVector);
                //Debug.Log("Newdata:" + WaveVector);
                if (WaveVector == 0)
                {
                    Zerowavetreecheck = true;
                    FVectory = 0.5f;
                }
                
                if(WaveVector == 2 || WaveVector == -2)
                {
                    wavetreecheck = true;
                    WaveVector = 2;
                }
                
                if(WaveVector == 1 || WaveVector == -1)
                {
                    wavetreecheck = true;
                    WaveVector = 1;
                }

                // 檢查條件是否滿足，然後設定旗標
                if(treechoice == 0)
                {
                    if (confirm == "T")
                    {
                        triggerLighting = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                    }
                }
                else if (treechoice == 1)
                {
                    if(confirm =="T")
                    {
                        raincheck = true;
                        raindotcheck = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                    }
                }
                
                else if (treechoice == 2)
                {
                    if(confirm =="T")
                    {
                        dropcheck = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                    }
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
