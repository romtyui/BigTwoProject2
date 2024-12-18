using CosineKitty;
using nminhhoangit.SunCalculator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seasonal_Control : MonoBehaviour
{
    public string now_time;
    private int now_hour;
    public float timer;
    private float last_timer;

    public SunCalculator sunCalculator;
    public bool timeButton;
    public enum SeasonState { Spring, Summer, Autumn , Winter };
    [Header("®”Ç°¼¾¹")]
    public SeasonState state;
    // Start is called before the first frame update
    void Start()
    {
        last_timer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.time;
        if (timer > (last_timer + 25)) 
        {
            last_timer = timer;
            sunCalculator.m_Hour += 1;
            if (sunCalculator.m_Hour > 23) 
            {
                sunCalculator.m_Hour  = 0;
            }
        }
        now_time = DateTime.Now.ToString();
        now_hour = DateTime.Now.Hour;
        if (now_hour >= 9 && now_hour < 11)
        {
            state = SeasonState.Spring;
        }
        else if (now_hour >= 11 && now_hour < 13)
        {
            state = SeasonState.Summer;
        }
        else if (now_hour >= 13 && now_hour < 14)
        {
            state = SeasonState.Autumn;
        }
        else if (now_hour >= 14 && now_hour <= 15)
        {
            state = SeasonState.Winter;
        }
        seasonalControler();
        if (timeButton) 
        {
            sunCalculator.m_Hour = now_hour;
        }
    }
    void seasonalControler() 
    {
        switch (state) 
        {
            case SeasonState.Spring:
                break;
            case SeasonState.Summer:
                break;
            case SeasonState.Autumn:
                break;
            case SeasonState.Winter:
                break;
        }
    }
}
