using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruit_drop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "bearwalk") 
        {
            other.GetComponent<BearWalkCheck>().Isplay_bearscare = true;
            Debug.Log(other.gameObject.name);

        }
    }
}
