using System.Collections;
using System.Threading;
using UnityEngine;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{
    public SerialPort sp = new SerialPort("com3", 38400);
    private Thread serialThread;
    private float Pos;
    private int Olddate;
    private int transpos;
    private bool triggerCamera;
    private bool isRotating = false; // �O�_���b����

    void Start()
    {
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

    void FixedUpdate()
    {
        if (triggerCamera && !isRotating) // �p�GĲ�o����B���b���त
        {
            StartCoroutine(roundCamera());
            triggerCamera = false; // ���mĲ�o�лx
        }
    }

    private void ReadSerialData()
    {
        while (true)
        {
            if (sp.IsOpen)
            {
                try
                {
                    string rawData = sp.ReadLine();
                    if (float.TryParse(rawData, out float newAngle))
                    {
                        Pos = newAngle;
                        transpos = Mathf.RoundToInt(Pos);
                        triggerCamera = true; // �]�mĲ�o�лx
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Serial Read Error: " + ex.Message);
                }
            }
            Thread.Sleep(10);
        }
    }

    private IEnumerator roundCamera()
    {
        if (transpos != Olddate && !isRotating) // �p�G�����ܤƥB���b���त
        {
            // �p��ؼШ���
            int newQuadrant = Mathf.FloorToInt((transpos + 180f) / 90f) % 4;
            int targetAngle = newQuadrant * 90;

            // �}�l���Ʊ���
            yield return SmoothRotateToAngle(targetAngle);

            Olddate = transpos; // ��s�W�@�Ө���
        }
    }

    private IEnumerator SmoothRotateToAngle(float targetAngle)
    {
        isRotating = true; // �}�l����
        float duration = 1.0f; // ���Ʊ���һݮɶ��]��^
        float elapsedTime = 0f; // �w�g�L���ɶ�
        Quaternion startRotation = transform.rotation; // ��l���ਤ��
        Quaternion endRotation = Quaternion.Euler(0, targetAngle, 0); // �ؼб��ਤ��

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // ��s�w�g�L���ɶ�
            float t = elapsedTime / duration; // �ɶ����
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t); // ���Ʊ���
            yield return null; // ���ݤU�@�V
        }

        transform.rotation = endRotation; // �T�O�̲ױ����ؼШ���
        isRotating = false; // ��������
    }

    private void OnDestroy()
    {
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Abort();
        }
        if (sp.IsOpen)
        {
            sp.Close();
        }
    }
}
