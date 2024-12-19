using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BearWalkCheck : MonoBehaviour
{
    public Arduinoreserve ard;
    public float wanderRadius = 10f;  // 游荡半径
    public float wanderTimer = 5f;   // 每次改变位置的时间间隔

    public Animator animator;
    private NavMeshAgent agent;      // 导航网格代理
    public float timer;             // 计时器

    private Vector3 newPos;
    public int count = 0;

    public GameObject main_camera;
    public GameObject scare_bear;
    public bool Isplay_bearscare;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

    }

    [System.Obsolete]
    void Update()
    {
        if (ard.playing)
        {
            if (Isplay_bearscare) 
            {
                animator.CrossFade("take_01_Walk_bear_idle", -0.1f, 0);

                // 让物体朝向相机
                this.transform.LookAt(main_camera.transform);

                // 将物体的 x 和 z 轴旋转保持为原来的值，只改变 y 轴
                this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);

                StartCoroutine(Waittime(1.0f));
                this.transform.position = Vector3.Lerp(transform.position, main_camera.transform.position, Time.deltaTime * 1f);
                StartCoroutine(Waittime(0.5f));
                scare_bear.SetActive(true);
                this.gameObject.SetActive(false);
                main_camera.GetComponent<Arduinoreserve>().nextScene = 2;
            }

        }
        else 
        {
            if (timer >= wanderTimer)
            {
                newPos = RandomNavSphere(transform.position, wanderRadius, -1);

                if (this.gameObject.transform.position == newPos)
                {


                }
                else
                {
                    agent.SetDestination(newPos);
                    animator.CrossFade("take_01_Walk_bear_walk", 0.1f, 0);


                    timer = 0;
                }
            }
            else
            {
                if (this.gameObject.transform.position == newPos)
                {
                    var i = Random.Range(0, 10);
                    if (i >= (4 + count))
                    {
                        count++;
                        timer = wanderTimer;
                    }
                    else
                    {
                        animator.CrossFade("take_01_Walk_bear_idle", -0.1f, 0);
                        if (timer >= (wanderTimer - 1))
                        {
                            count = 0;
                        }
                    }
                }
                timer += Time.deltaTime;
            }

        }

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    public void OnBearWalkEnd()
    {
        //Arduinoreserve.bearwalkdone = true;
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    if(Arduinoreserve.bearwalkdone == true)
    //    {
    //        //Walkbear.SetActive(false);
    //    }
    //    else
    //    {
    //        //Walkbear.SetActive (true);
    //        //Vector3 targetpos = new Vector3(transform.position.x, transform.position.y, -15f);
    //        //this.transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime *0.4f);
    //    }
    //}
    private IEnumerator Waittime(float x)
    {


        // 等待指定的时间（比如 2 秒）
        yield return new WaitForSeconds(x);

    }

}
