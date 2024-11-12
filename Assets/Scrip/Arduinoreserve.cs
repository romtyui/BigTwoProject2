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

    public bool triggerLighting = false;  // �Ω�D�������s���A���X��
    
    
    /*-----------------�n��----------------------*/
    public GameObject Wavetree;
    public bool Wavetreecheck;
    private Material treematerial;
    public bool wavetreecheck;
    /*-----------------�n��----------------------*/

    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        material = renderer.material;
        material.SetFloat("_Ripple_Strengh", 0);
        /*-----------------�n��----------------------*/
        Renderer treerenderer = Wavetree.GetComponent<Renderer>();
        treematerial = treerenderer.material;

        /*-----------------�n��----------------------*/
        
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

                int.TryParse(sp4date, out WaveVector);//��sp4date�নint���waveVrctor

                Debug.Log("confirm:" + confirm);
                Debug.Log("Newdata:" + WaveVector);

                // �ˬd����O�_�����A�M��]�w�X��
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
