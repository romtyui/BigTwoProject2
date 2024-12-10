using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BearWalkCheck : MonoBehaviour
{
    public static bool bearwalkdonecheck;
    public static bool bearStartWalk;
    public GameObject Walkbear;

    public void OnBearWalkEnd()
    {
        Arduinoreserve.bearwalkdone = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bearStartWalk == true)
        {
            Walkbear.SetActive(true);
        }

        if(Arduinoreserve.bearwalkdone == true)
        {
            Walkbear.SetActive(false);
        }
        else
        {
            Walkbear.SetActive (true);
            Vector3 targetpos = new Vector3(transform.position.x, transform.position.y, -15f);
            this.transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime *0.4f);
        }
    }
}
