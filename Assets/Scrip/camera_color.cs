using nminhhoangit.SunCalculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_color : MonoBehaviour
{
    public SunCalculator calculator;
    [Header("不同時間的燈光")]
    public Color Day_color;
    public Color Night_color;
    public Light[] Lights;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calculator != null)
        {
            if (calculator.GetComponent<SunCalculator>().m_Hour > 18 || calculator.GetComponent<SunCalculator>().m_Hour < 6) 
            {
                for (int i = 0; i < Lights.Length; i++) 
                {
                    Lights[i].color = Night_color;
                } 
            }
            else if(calculator.GetComponent<SunCalculator>().m_Hour < 18 || calculator.GetComponent<SunCalculator>().m_Hour > 6)
            {
                for (int i = 0; i < Lights.Length; i++)
                {
                    Lights[i].color = Day_color;
                }
            }
        }
    }
}
