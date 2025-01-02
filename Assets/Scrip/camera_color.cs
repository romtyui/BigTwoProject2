using nminhhoangit.SunCalculator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_color : MonoBehaviour
{
    public SunCalculator calculator;
    [Header("Ÿô¹â?É«")]
    public Color Day_color;
    public Color Night_color;
    public Color camera_colors;

    public Light[] Lights;

    [Header("Ÿô¹âžVçR")]
    public Material plane_material;
    [SerializeField]
    [Range(0, 1)]
    private float m_alpha = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calculator != null && camera_colors != null)
        {
            if (calculator.GetComponent<SunCalculator>().m_Hour > 18 || calculator.GetComponent<SunCalculator>().m_Hour < 6) 
            {
                for (int i = 0; i < Lights.Length; i++) 
                {
                    Lights[i].color = Night_color;
                                
                }
                camera_colors = plane_material.color;
                camera_colors.a = m_alpha;
                plane_material.color = camera_colors;
            }
            else if(calculator.GetComponent<SunCalculator>().m_Hour < 18 || calculator.GetComponent<SunCalculator>().m_Hour > 6)
            {
                for (int i = 0; i < Lights.Length; i++)
                {
                    Lights[i].color = Day_color;
                }
                camera_colors = plane_material.color;
                camera_colors.a = 0;
                plane_material.color = camera_colors;

            }
        }
    }
}
