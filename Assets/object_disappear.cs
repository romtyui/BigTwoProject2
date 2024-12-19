using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_disappear : MonoBehaviour
{
    public Arduinoreserve ard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null) 
        {
            //other.gameObject.SetActive(false);
            ard.nextScene = 1;
            Debug.Log(other.gameObject.name);
        }
    }
}
