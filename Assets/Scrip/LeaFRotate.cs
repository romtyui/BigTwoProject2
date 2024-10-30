using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaFRotate : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Transform>().LookAt(target);
    }
    Vector3 Target_transform() 
    {
        return new Vector3(0,target.transform.rotation.y +180,0);

    }
}
