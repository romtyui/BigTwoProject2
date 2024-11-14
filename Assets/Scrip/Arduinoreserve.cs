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
    public bool treerechoice = false;
    public GameObject[] Wavetree;
    [SerializeField]private Material treematerial;
    [SerializeField] private Material leavesmaterial;
    public bool wavetreecheck = false;
    /*-----------------搖樹----------------------*/

    /*----------------丟熊熊----------------------*/
    [Header("丟熊熊")]
    public GameObject Rock;
    public float RockX;
    public float RockY;
    public float RockZ;
    public bool throwcheck = false;
    public float throwrocktotalTime = 3;
    public float gravty = 9.8f;
    public float time;
    private double powT;
    /*----------------丟熊熊----------------------*/

    // Start is called before the first frame update
    void Start()
    {
        treechoice = Random.Range(0, 3);
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        rainmaterial = renderer.material;
        rainmaterial.SetFloat("_Ripple_Strengh", 0);
        /*-----------------搖樹----------------------*/
//        Renderer treerenderer = Wavetree[treechoice].GetComponent<Renderer>();
//        treematerial = treerenderer.material;

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
        
        if (throwcheck /*|| raindotcheck*/ == true)
        {
            RockX = Rock.transform.position.x;
            RockY = Rock.transform.position.y;
            RockZ = Rock.transform.position.z;
            time = Time.deltaTime;

            powT = (float)throwrocktotalTime * throwrocktotalTime;
            Rock.transform.localPosition = new Vector3((RockX * time) / throwrocktotalTime, (RockY * time) / throwrocktotalTime, ((RockZ + (float)(0.5 * gravty * powT))/throwrocktotalTime) * (time - (float)(0.5 * gravty * time*time)));
        }

        if(wavetreecheck == true)
        {
            leavesmaterial.SetFloat("_WindDensity", Vectory * 5);
            leavesmaterial.SetFloat("_WindMovement", Vectory*5);
            leavesmaterial.SetFloat("_WindStrength", Vectory * 5);
            Vector2 offset = treematerial.GetVector("_Direction");
            treematerial.SetVector("_Direction", new Vector4(Vectory*5, 0.1f, 0, 0));
            treematerial.SetFloat("_BlendStrength", 5f);
        }

        if(treerechoice == true)
        {
            treechoice = Random.Range(0, 3);
            Renderer treerenderer = Wavetree[treechoice].GetComponent<Renderer>();
            treematerial = treerenderer.material;
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
                }
                //int.TryParse(wavedate, out WaveVector);//把sp4date轉成int放到waveVrctor
                //int.TryParse(confirm, out WaveVector);
                Debug.Log("Vectory:" + Vectory);
                //Debug.Log("Newdata:" + WaveVector);
                if(Vectory > 0 || Vectory < 0)
                {
                    wavetreecheck = true;
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
                        throwcheck = true;
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
