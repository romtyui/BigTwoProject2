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
        GetComponent<Transform>().LookAt(GetSymmetryPoint());
    }
    Vector3 GetSymmetryPoint()
    {
        return new Vector3(
            transform.position.x * 2 - target.position.x,
            transform.position.y * 2 - target.position.y,
            transform.position.z * 2 - target.position.z);
    }
}
