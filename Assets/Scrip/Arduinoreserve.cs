using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
public class Arduinoreserve : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com5", 38400);
    private Thread serialThread;
    public int WaveVector;
    public string Newdate;
    public string confirm;

    public LightingCode lightingcode;
    public GameObject rain;
    public GameObject raindot;
    private Material material;
    //public Behaviour throwrock;

    public bool triggerLighting = false;  // �Ω�D�������s���A���X��


    // Start is called before the first frame update
    void Start()
    {
        rain.SetActive(false);
        Renderer renderer = raindot.GetComponent<Renderer>();
        material = renderer.material;

        material.SetFloat("_Ripple_Strengh", 0);
        try
        {
            sp.Open();
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
    }

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                confirm = sp.ReadLine();
                Newdate = sp.ReadLine();
                int.TryParse(Newdate, out WaveVector);//��Newdate�নint���waveVrctor

                Debug.Log("confirm:" + confirm);
                Debug.Log("Newdata:" + WaveVector);

                // �ˬd����O�_�����A�M��]�w�X��
                if (confirm == "T")
                {
                    triggerLighting = true;
                }

                if(confirm =="R" && Newdate != null)
                {
                    rain.SetActive(true);
                    material.SetFloat("_Ripple_Strengh", 0.1f);
                    //WaveTree.Newdate = Newdate;
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
