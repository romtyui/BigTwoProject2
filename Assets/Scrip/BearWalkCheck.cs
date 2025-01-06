using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BearWalkCheck : MonoBehaviour
{
    public Arduinoreserve ard;
    public float wanderRadius = 10f; // 游荡半径
    public float wanderTimer = 5f;  // 每次改变位置的时间间隔

    public Animator animator;
    private NavMeshAgent agent;     // 导航网格代理
    public float timer;            // 计时器

    private Vector3 newPos;
    public int count = 0;

    public GameObject main_camera;
    public GameObject scare_bear;
    public bool Isplay_bearscare;

    public Bounds movementBounds;  // 限制范围

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        // 设置默认活动范围（可以根据需要调整）
        movementBounds = new Bounds(transform.position, new Vector3(20, 20, 20));
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

                if (movementBounds.Contains(newPos)) // 检查新位置是否在范围内
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

    private IEnumerator Waittime(float x)
    {
        yield return new WaitForSeconds(x);
    }

}
