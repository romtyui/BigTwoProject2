using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Linq;

public class Arduinoreserve : MonoBehaviour
{
    public Camera mainCamera; // 指定攝像機

    public SerialPort sp = new SerialPort("com7", 38400);//com7
    //public SerialPort sp4 = new SerialPort("com4", 38400);
    private Thread serialThread;
    public int WaveVector;
    public float Vectory;
    public string wavedate;
    public string confirm;
    public float Timer;
    public int nextScene;
    public bool playing;
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
    public JiggleChain[] jiggleChain;
    public int treechoice;
    private float FVectory;
    public static bool treerechoice = false;
    public GameObject[] Wavetree;
    public GameObject[] incameratree;
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
    public int count;
    public GameObject bearscare;
    public GameObject bearwalk;
    public static bool bearwalkdone;
    public GameObject fruit;
    public bool dropcheck = false;
    public Transform[] Abear_cameras;
    public float triggerTime = 0;
    private Quaternion targetRotation;
    public GameObject block;
    /*----------------丟熊熊----------------------*/

    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        rainmaterial = renderer.material;
        rainmaterial.SetFloat("_Ripple_Strengh", 0);
        targetRotation = Quaternion.Euler(0, -90, -45);
        /*-----------------搖樹----------------------*/
        int k = 0;
        foreach (GameObject obj in Wavetree)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

            // 檢查物件是否在視野內
            if (viewportPos.z > 0 && // 確保物件在攝像機前方
                viewportPos.x > 0 && viewportPos.x < 1 && // X 軸在視口範圍內
                viewportPos.y > 0 && viewportPos.y < 1)   // Y 軸在視口範圍內
            {
                incameratree[k] = obj;
                k++;
                if(k>4)
                {
                    k = 0;
                }
            }
        }
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
        treechoice = Random.Range(0, 4);
        
        Debug.Log(incameratree[treechoice].transform.GetChild(8).transform.childCount);
        for (count = 0; count < incameratree[treechoice].transform.GetChild(8).transform.childCount; count++)
        {
            JiggleChain eachChain = incameratree[treechoice].transform.GetChild(8).transform.GetChild(count).GetComponent<JiggleChain>();
            jiggleChain[count] = eachChain;
            jiggleChain[count].data.externalForce.y = 3;
        }
        
        treerechoice = true;
        /*-----------------搖樹----------------------*/

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
            Timer = Timer + Time.fixedDeltaTime;
            if (Timer >= 300f)
            {
                rain.SetActive(false);
                rainmaterial.SetFloat("_Ripple_Strengh", 0f);
                Timer = 0;
                raincheck = false;
            }
        }

        if (dropcheck == true)
        {
            fruit.GetComponent<Rigidbody>().useGravity = true;
            if (nextScene == 0)
            {
                mainCamera.transform.position = Abear_cameras[1].transform.position;
                mainCamera.transform.rotation = Abear_cameras[1].rotation;
            }
            else if (nextScene == 1)
            {
                mainCamera.transform.position = Abear_cameras[2].transform.position;
                mainCamera.transform.rotation = Abear_cameras[2].rotation;
            }
            else if (nextScene == 2)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    transform.rotation = targetRotation; // 修正最终角度
                    block.SetActive(true);
                    StartCoroutine(Waittime(1f));
                }
            }

            //BearWalkCheck.bearStartWalk = true;
            //bearwalk.SetActive(true);
            //if (bearwalkdone == true)
            //{
            //    bearscare.SetActive(true);
            //    Debug.Log("啟動");
            //    dropcheck = false;
            //}

            //}
        }
        if (Zerowavetreecheck == true)
        {
            leavesmaterial.SetFloat("_WindDensity", FVectory);
            leavesmaterial.SetFloat("_WindMovement", FVectory);
            leavesmaterial.SetFloat("_WindStrength", FVectory);
            Vector2 offset = trunkmaterial.GetVector("_Direction");
            trunkmaterial.SetVector("_Direction", new Vector4(FVectory, 0.1f, 0, 0));
            trunkmaterial.SetFloat("_BlendStrength", 5f);
            Zerowavetreecheck = false;
        }

        else if (wavetreecheck == true)
        {
            leavesmaterial.SetFloat("_WindDensity", WaveVector);
            leavesmaterial.SetFloat("_WindMovement", WaveVector);
            leavesmaterial.SetFloat("_WindStrength", WaveVector);
            Vector2 offset = trunkmaterial.GetVector("_Direction");
            trunkmaterial.SetVector("_Direction", new Vector4(WaveVector, 0.1f, 0, 0));
            trunkmaterial.SetFloat("_BlendStrength", 5f);
            wavetreecheck = false;
        }


        if (treerechoice == true)
        {
            foreach (GameObject obj in Wavetree)
            {
                Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);
                int l = 0;
                // 檢查物件是否在視野內
                if (viewportPos.z > 0 && // 確保物件在攝像機前方
                    viewportPos.x > 0 && viewportPos.x < 1 && // X 軸在視口範圍內
                    viewportPos.y > 0 && viewportPos.y < 1)   // Y 軸在視口範圍內
                {
                    incameratree[l] = obj;
                    l++;
                    if (l > 4)
                    {
                        l = 0;
                    }
                }
            }

            treechoice = Random.Range(0, 4);
            treerechoice = false;
        }
            for (count = 0; count < incameratree[treechoice].transform.GetChild(8).transform.childCount; count++)
            {
                JiggleChain eachchain = incameratree[treechoice].transform.GetChild(8).transform.GetChild(count).GetComponent<JiggleChain>();
                jiggleChain[count] = eachchain;
                jiggleChain[count].data.externalForce.y = Vectory * 3;
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
                if (wavedate != "T")
                {
                    Vectory = float.Parse(wavedate);
                    //WaveVector = (int)Vectory;
                }
                //int.TryParse(wavedate, out WaveVector);//把sp4date轉成int放到waveVrctor
                //int.TryParse(confirm, out WaveVector);
                //    Debug.Log("Vectory:" + Vectory);
                //    Debug.Log("WaveVectory:" + WaveVector);
                //Debug.Log("Newdata:" + WaveVector);
                //    if (WaveVector == 0)
                //    {
                //        Zerowavetreecheck = true;
                //        FVectory = 0.5f;
                //    }

                //    if(WaveVector == 2 || WaveVector == -2)
                //    {
                //        wavetreecheck = true;
                //        WaveVector = 2;
                //    }

                //    if(WaveVector == 1 || WaveVector == -1)
                //    {
                //        wavetreecheck = true;
                //        WaveVector = 1;
                //    }

                // 檢查條件是否滿足，然後設定旗標
                if (treechoice == 0)
                {
                    if (confirm == "T")
                    {
                        Debug.Log("256482314586");
                        triggerLighting = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        playing = true;
                        /*-----重選樹-----*/
                    }
                }
                else if (treechoice == 1)
                {
                    if (confirm == "T")
                    {
                        Debug.Log("256482314586");
                        raincheck = true;
                        raindotcheck = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                        playing = true;

                    }
                }

                else if (treechoice == 2)
                {
                    if (confirm == "T")
                    {
                        Debug.Log("256482314586");
                        dropcheck = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                        playing = true;

                    }
                }

                if (wavedate == "103" || confirm == "103")
                {
                    Debug.Log("256482314586");
                    dropcheck = true;
                    /*-----重選樹-----*/
                    treerechoice = true;
                    /*-----重選樹-----*/
                    playing = true;

                }

                if (wavedate == "102" || confirm == "102")
                {
                    Debug.Log("256482314586");
                    raincheck = true;
                    raindotcheck = true;
                    /*-----重選樹-----*/
                    treerechoice = true;
                    /*-----重選樹-----*/
                    playing = true;

                }

                if (wavedate == "101" || confirm == "101")
                {
                    Debug.Log("256482314586");
                    triggerLighting = true;
                    /*-----重選樹-----*/
                    treerechoice = true;
                    /*-----重選樹-----*/
                    playing = true;

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
    private IEnumerator Waittime(float x)
    {

        bearscare.SetActive(false);

        // 等待指定的时间（比如 2 秒）
        yield return new WaitForSeconds(x);
        block.SetActive(false);
        mainCamera.transform.position = Abear_cameras[0].transform.position;
        mainCamera.transform.rotation = Abear_cameras[0].rotation;
        dropcheck = false;
    }
}
