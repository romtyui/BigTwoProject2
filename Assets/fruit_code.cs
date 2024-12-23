using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruit_code : MonoBehaviour
{
    private float life_timer;
    private Rigidbody rb;
    private float Dropout_rate;
    public Seasonal_Control controler;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        controler = GameObject.Find("Main Camera").GetComponent<Seasonal_Control>();
    }

    // Update is called once per frame
    void Update()
    {
        life_timer += Time.time;
        if (controler.now_minute != controler.last_minute) 
        {
            int i =Random.Range(0, 100);
            if ((i + (life_timer / 100f)) > 30f) 
            {
                rb.useGravity = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "енн▒") 
        {
            Destroy(this.gameObject);
        }
    }
}
