using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaFRotate : MonoBehaviour
{
    public Transform target;
    public Arduinoreserve ard;
    private Quaternion orignal_transform;
    // Start is called before the first frame update
    void Start()
    {
        orignal_transform = this.GetComponent<Transform>().rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (ard != null) 
        {
            if (!ard.playing)
            {
                GetComponent<Transform>().LookAt(GetSymmetryPoint());
            }
            else 
            {
                this.transform.rotation = orignal_transform;
            }
        }

    }
    Vector3 GetSymmetryPoint()
    {
        return new Vector3(
            transform.position.x * 2 - target.position.x,
            transform.position.y * 2 - target.position.y,
            transform.position.z * 2 - target.position.z);
    }
}
