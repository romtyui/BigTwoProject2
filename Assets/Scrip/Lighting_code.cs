using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_code : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnParticleTrigger()
    {
        StartCoroutine(WaitforSeconds(0.3f));
    }
    public IEnumerator WaitforSeconds(float duration) 
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);

    }
}
