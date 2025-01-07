using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using nminhhoangit.SunCalculator;
using static Seasonal_Control;

public class Arduinoreserve : MonoBehaviour
{
    public Camera mainCamera; // 指定攝像機

    public SerialPort sp = new SerialPort("com9", 38400);//com7
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
    public GameObject rain,snow;
    public bool raincheck;
    public GameObject raindot;
    public bool raindotcheck;
    private Material rainmaterial;
    private Seasonal_Control control;
    /*-------------------------下雨-------------------------*/

    public bool triggerLighting = false;  // 用於主執行緒更新狀態的旗標

    /*-----------------搖樹----------------------*/
    [Header("搖樹")]
    public JiggleChain[] jiggleChain;
    public int treechoice;
    private float FVectory;
    public static bool treerechoice = false;
    public List<GameObject> Wavetree; // 所有樹的集合
    public List<GameObject> incameratree = new List<GameObject>(); // 攝像機內的樹集合
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
    /*---------------------------------音效--------------------------------------*/
    [Header("音效")]
    public AudioSource BGM;
    public AudioSource Audio;
    public AudioClip thunderclip;
    public AudioClip rainclip;
    public AudioClip blown_leaves_clip;
    /*---------------------------------音效--------------------------------------*/


    // Start is called before the first frame update
    void Start()
    {
        /*---------------------------------音效--------------------------------------*/
        BGM.Play();
        /*---------------------------------音效--------------------------------------*/

        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        rainmaterial = renderer.material;
        rainmaterial.SetFloat("_Ripple_Strengh", 0);
        targetRotation = Quaternion.Euler(0, -90, -45);
        /*-----------------搖樹----------------------*/

        foreach (GameObject obj in Wavetree)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

            // 檢查物件是否在視野內
            if (viewportPos.z > 0 && // 確保物件在攝像機前方
                viewportPos.x > 0 && viewportPos.x < 1 && // X 軸在視口範圍內
                viewportPos.y > 0 && viewportPos.y < 1)   // Y 軸在視口範圍內
            {
                if (!incameratree.Contains(obj)) // 確保不重複添加
                {
                    incameratree.Add(obj);
                    Debug.Log("Object added to incameratree: " + obj.name);
                }
            }
        }
        try
        {
            sp.Open();
            //sp4.Open();
            Debug.Log("try");
            serialThread = new Thread(ReadSerialData);
            serialThread.Start();
        }
        catch (System.Exception e)
        {
           Debug.Log("第一層");
            Debug.LogError("Failed to open Serial Port: " + e.Message);
        }
        treechoice = Random.Range(0, incameratree.Count);
        Debug.Log("treechoice: " + treechoice);
        treerechoice = true;

        /*-----------------搖樹----------------------*/
        control = this.GetComponent<Seasonal_Control>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    void FixedUpdate()
    {
        if (incameratree.Count == 0)
        {
            return;
        }
        else
        {
            ProcessJiggleChainsInRig(incameratree[treechoice]);

        }
        // 在主執行緒中檢查旗標，並根據需要啟動 lightingcode
        if (triggerLighting)
        {
            StartCoroutine(TriggerLightingEffect());
            triggerLighting = false;  // 重置旗標
            // lightingcode.enabled = false;
        }

        if (raincheck /*|| raindotcheck*/ == true)
        {
            if (control.state == SeasonState.Winter)
            {
                snow.SetActive(true);

            }
            else
            {
                rain.SetActive(true);
            }
            rainmaterial.SetFloat("_Ripple_Strengh", 0.1f);
            Timer = Timer + Time.fixedDeltaTime;
            if (Timer >= 300f)
            {
                if (control.state == SeasonState.Winter)
                {
                    snow.SetActive(false);

                }
                else
                {
                    rain.SetActive(false);
                }
                rainmaterial.SetFloat("_Ripple_Strengh", 0f);
                Audio.clip = null;
                BGM.UnPause();
                Timer = 0;
                raincheck = false;
            }
        }

        if (dropcheck == true)
        {
            //Debug.Log("playing");
            //fruit.GetComponent<Rigidbody>().useGravity = true;
            //if (nextScene == 0)
            //{
            //    mainCamera.transform.position = Abear_cameras[1].transform.position;
            //    mainCamera.transform.rotation = Abear_cameras[1].rotation;
            //}
            //else if (nextScene == 1)
            //{
            //    mainCamera.transform.position = Abear_cameras[2].transform.position;
            //    mainCamera.transform.rotation = Abear_cameras[2].rotation;
            //}
            //else if (nextScene == 2)
            //{
            //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
            //    if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            //    {
            //        transform.rotation = targetRotation; // 修正最终角度
            //        block.SetActive(true);
            //        StartCoroutine(Waittime(1f));
            //    }
            //}

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
            UpdateIncameraTree();
            Debug.Log("TreeReChoice: " + treerechoice);
            treerechoice = false;
            Debug.Log("TreeReChoice: " + treerechoice);
        }

        if (treechoice == 1 && confirm == "T")
        {
            Audio.clip = rainclip;
            Audio.Play();
            BGM.Pause();
        }

        if (treechoice == 0 && confirm == "T")
        {
            Audio.clip = thunderclip;
            Audio.Play();
            
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
                        raincheck = true;
                        raindotcheck = true;
                       
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                         //playing = true;

                    }
                }

                else if (treechoice == 2)
                {
                    if (confirm == "T")
                    {
                        //dropcheck = true;
                        /*-----重選樹-----*/
                        treerechoice = true;
                        /*-----重選樹-----*/
                        //playing = true;

                    }
                }

//                if (wavedate == "103" || confirm == "103")
//                {
//                    dropcheck = true;
//                    /*-----重選樹-----*/
//                    treerechoice = true;
//                    /*-----重選樹-----*/
//                    playing = true;
//
//                }

                if (wavedate == "102" || confirm == "102")
                {
                    raincheck = true;
                    raindotcheck = true;
                    /*-----重選樹-----*/
                    treerechoice = true;
                    /*-----重選樹-----*/
                    //playing = true;

                }

                if (wavedate == "101" || confirm == "101")
                {
                    
                    triggerLighting = true;
                    /*-----重選樹-----*/
                    treerechoice = true;
                    /*-----重選樹-----*/
                    //playing = true;

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

    void ProcessJiggleChainsInRig(GameObject tree)
    {
        // 找到樹物件下的 "Rig" 子物件
        Transform rig = tree.transform.Find("Rig");

        if (rig != null)
        {
            // 遍歷 "Rig" 下的所有子物件，並處理每個名為 "JiggleChain" 的物件
            foreach (Transform jiggleChain in rig)
            {
                if (jiggleChain.name == "JiggleChain")
                {
                    // 修改 "JiggleChain" 中的 .data.externalForce.y
                    ModifyExternalForce(jiggleChain);
                }
            }
        }
        else
        {
            Debug.Log("No 'Rig' child found in " + tree.name);
        }
    }

    // 修改 JiggleChain 中的 .data.externalForce.y
    void ModifyExternalForce(Transform jiggleChain)
    {
        // 獲取 JiggleChain 組件
        JiggleChain jiggleData = jiggleChain.GetComponent<JiggleChain>();
        if (jiggleData != null)
        {
            // 修改 externalForce.y 的值
            jiggleData.data.externalForce.y = Vectory*2f;
            //jiggleData.data.externalForce.x = Vectory*2f;
            //Audio.clip = blown_leaves_clip;
            //Audio.Play();
            //BGM.Pause();
            // 輸出修改結果

        }
        else
        {
            // 若沒有找到 JiggleChainData 組件，輸出提示
            Debug.Log(jiggleChain.name + " does not have JiggleChainData component.");
        }
    }

    void UpdateIncameraTree()
    {
        incameratree.Clear(); // 清空舊的視野內樹列表

        foreach (GameObject obj in Wavetree)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

            // 檢查物件是否在攝像機的視野內
            if (viewportPos.z > 0 && // 確保物件在攝像機前方
                viewportPos.x > 0 && viewportPos.x < 1 && // X 軸在視口範圍內
                viewportPos.y > 0 && viewportPos.y < 1)   // Y 軸在視口範圍內
            {
                incameratree.Add(obj);
            }
        }

        if (treerechoice)
        {
            treechoice = Random.Range(0, incameratree.Count);
            Debug.Log("Tree chosen: " + incameratree[treechoice].name);
        }
        else
        {
            Debug.LogWarning("No trees found in camera view.");
        }
    }

}