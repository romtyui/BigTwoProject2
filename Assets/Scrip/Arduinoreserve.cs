using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Linq;

public class Arduinoreserve : MonoBehaviour
{
    public Camera mainCamera; // ���w�ṳ��

    public SerialPort sp = new SerialPort("com3", 38400);//com7
    //public SerialPort sp4 = new SerialPort("com4", 38400);
    private Thread serialThread;
    public int WaveVector;
    public float Vectory;
    public string wavedate;
    public string confirm;
    public float Timer;
    /*-------------------------�U�B-------------------------*/
    [Header("�U�B")]
    public LightingCode lightingcode;
    public GameObject rain;
    public bool raincheck;
    public GameObject raindot;
    public bool raindotcheck;
    private Material rainmaterial;
    /*-------------------------�U�B-------------------------*/

    public bool triggerLighting = false;  // �Ω�D�������s���A���X��

    /*-----------------�n��----------------------*/
    [Header("�n��")]
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
    /*-----------------�n��----------------------*/

    /*----------------�ẵ��----------------------*/
    [Header("�ẵ��")]
    //public GameObject Rock;
    //public float RockX;
    //public float RockY;
    //public float RockZ;
    //public bool throwcheck = false;
    //public float throwrocktotalTime = 3;
    //public float gravty = 9.8f;
    //public float time;
    //private double powT;
    public GameObject bearscare;
    public GameObject bearwalk;
    public static bool bearwalkdone;
    public GameObject fruit;
    public bool dropcheck = false;
    public float triggerTime=0;
    /*----------------�ẵ��----------------------*/

    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        rainmaterial = renderer.material;
        rainmaterial.SetFloat("_Ripple_Strengh", 0);
        /*-----------------�n��----------------------*/
        foreach (GameObject obj in Wavetree)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

            // �ˬd����O�_�b������
            if (viewportPos.z > 0 && // �T�O����b�ṳ���e��
                viewportPos.x > 0 && viewportPos.x < 1 && // X �b�b���f�d��
                viewportPos.y > 0 && viewportPos.y < 1)   // Y �b�b���f�d��
            {
                incameratree.Append(obj);
                Debug.Log($"{obj.name} is in view.");
            }
        }
        treechoice = Random.Range(0, 3);
        Renderer treerenderer = incameratree[treechoice].GetComponent<Renderer>();
        treematerial = treerenderer.materials;
        trunkmaterial = treematerial[0];
        leavesmaterial = treematerial[1];
        /*-----------------�n��----------------------*/
        
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
        // �b�D��������ˬd�X�СA�îھڻݭn�Ұ� lightingcode
        if (triggerLighting)
        {
            StartCoroutine(TriggerLightingEffect());
            triggerLighting = false;  // ���m�X��
            // lightingcode.enabled = false;
        }

        if (raincheck /*|| raindotcheck*/ == true)
        {
            rain.SetActive(true);
            rainmaterial.SetFloat("_Ripple_Strengh", 0.1f);
            Timer = Timer+Time.fixedDeltaTime;
            if(Timer>=300f)
            {
                rain.SetActive(false);
                rainmaterial.SetFloat("_Ripple_Strengh", 0f);
                Timer = 0;
                raincheck = false;
            }
        }
        
        if (dropcheck == true)
        {
            //RockX = Rock.transform.position.x;
            //RockY = Rock.transform.position.y;
            //RockZ = Rock.transform.position.z;
            //time = Time.deltaTime;
            //powT = (float)throwrocktotalTime * throwrocktotalTime;
            //Rock.transform.localPosition = new Vector3((RockX * time) / throwrocktotalTime, (RockY * time) / throwrocktotalTime, ((RockZ + (float)(0.5 * gravty * powT))/throwrocktotalTime) * (time - (float)(0.5 * gravty * time*time)));
    //        fruit.SetActive(true);
            //triggerTime = Time.deltaTime;
            //if(T>2)
            //{

           
            BearWalkCheck.bearStartWalk = true;
            Debug.Log("256482314586");
            bearwalk.SetActive(true);
            if (bearwalkdone == true)
            {
                bearscare.SetActive(true);
                Debug.Log("�Ұ�");
                dropcheck = false;
            }

            //}
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
            foreach (GameObject obj in Wavetree)
            {
                Vector3 viewportPos = mainCamera.WorldToViewportPoint(obj.transform.position);

                // �ˬd����O�_�b������
                if (viewportPos.z > 0 && // �T�O����b�ṳ���e��
                    viewportPos.x > 0 && viewportPos.x < 1 && // X �b�b���f�d��
                    viewportPos.y > 0 && viewportPos.y < 1)   // Y �b�b���f�d��
                {
                    incameratree.Append(obj);
                    Debug.Log($"{obj.name} is in view.");
                }
            }

            treechoice = Random.Range(0, 3);
            Renderer treerenderer = incameratree[treechoice].GetComponent<Renderer>();
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
                //int.TryParse(wavedate, out WaveVector);//��sp4date�নint���waveVrctor
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

                // �ˬd����O�_�����A�M��]�w�X��
                if(treechoice == 0)
                {
                    if (confirm == "T")
                    {
                        triggerLighting = true;
                        /*-----�����-----*/
                        treerechoice = true;
                        /*-----�����-----*/
                    }
                }
                else if (treechoice == 1)
                {
                    if(confirm =="T")
                    {
                        raincheck = true;
                        raindotcheck = true;
                        /*-----�����-----*/
                        treerechoice = true;
                        /*-----�����-----*/
                    }
                }
                
                else if (treechoice == 2)
                {
                    if(confirm =="T")
                    {
                        dropcheck = true;
                        /*-----�����-----*/
                        treerechoice = true;
                        /*-----�����-----*/
                    }
                }

                if (wavedate == "103" || confirm == "103")
                {
                    
                    dropcheck = true;
                    /*-----�����-----*/
                    treerechoice = true;
                    /*-----�����-----*/
                }

                if (wavedate == "102" || confirm == "102")
                {

                    raincheck = true;
                    raindotcheck = true;
                    /*-----�����-----*/
                    treerechoice = true;
                    /*-----�����-----*/
                }

                if (wavedate == "101" || confirm == "101")
                {

                    triggerLighting = true;
                    /*-----�����-----*/
                    treerechoice = true;
                    /*-----�����-----*/
                }
            }
            Thread.Sleep(10); // ����Ū���W�v�A�קK�L�ץe��CPU
        }
    }

    private IEnumerator TriggerLightingEffect()
    {
        lightingcode.enabled = true;
        yield return new WaitForSeconds(0.5f);  // �Ҧp����0.1��
        lightingcode.enabled = false;
    }
}
