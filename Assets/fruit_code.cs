using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Seasonal_Control;

public class fruit_code : MonoBehaviour
{
    public float life_timer;
    private Rigidbody rb;
    private float Dropout_rate;
    public Seasonal_Control controler;
    private int life;
    public GameObject tree;
    // Start is called before the first frame update
    void Start()
    {
        life = Random.Range(0, 100);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        controler = GameObject.Find("Main Camera").GetComponent<Seasonal_Control>();
    }

    // Update is called once per frame
    void Update()
    {
        life_timer += Time.deltaTime;
        if (!controler.Isgenarate && life_timer > 10.0f) 
        {
            
            if ((life + (life_timer )) > 60f) 
            {
                //Debug.Log(life);
                rb.useGravity = true;
            }
        }
        if (controler.state != SeasonState.Summer) 
        {
            rb.useGravity = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Debug.Log("銷毀");
            StartCoroutine(Waittime(10.0f));
            
        }
    }
    private IEnumerator Waittime(float x)
    {


        // 等待指定的时间（比如 2 秒）
        yield return new WaitForSeconds(x);
        tree.GetComponent<fruit_generate>().tree_fruit_numbers--;
        Destroy(this.gameObject);

    }
}
