using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Seasonal_Control;

public class fruit_generate : MonoBehaviour
{
    public Seasonal_Control controler;
    public GameObject fruit;
    public float minRange = 0.2f; // 最小範圍
    public float maxRange = 0.5f; // 最大範圍
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controler.state == SeasonState.Summer) 
        {
            if (controler.Isgenarate) 
            {
                int i =Random.Range(0, 100);
                Debug.Log(i);
                if (i > 25) 
                {
                    SpawnPrefabOnRandomSide();
                }
            }
        }
    }
    void SpawnPrefabOnRandomSide()
    {
        if (fruit == null)
        {
            Debug.LogError("Prefab or Plane is not assigned.");
            return;
        }

        // 獲取 Plane 的 MeshRenderer
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("Plane does not have a MeshRenderer component."+this.gameObject.name);
            return;
        }

        // 獲取 Plane 的邊界範圍
        Bounds bounds = meshRenderer.bounds;

        // 隨機選擇一個方向
        int randomSide = Random.Range(0, 6); // 0: 上, 1: 下, 2: 左, 3: 右, 4: 前, 5: 後
        Vector3 spawnPosition = Vector3.zero;

        switch (randomSide)
        {
            case 0: // 上
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Mathf.Lerp(bounds.min.y, bounds.max.y, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 1: // 下
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Mathf.Lerp(bounds.min.y, bounds.max.y, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 2: // 左
                spawnPosition = new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 3: // 右
                spawnPosition = new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 4: // 前
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Mathf.Lerp(bounds.min.z, bounds.max.z, Random.Range(minRange, maxRange))
                );
                break;

            case 5: // 後
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Mathf.Lerp(bounds.min.z, bounds.max.z, Random.Range(minRange, maxRange))
                );
                break;
        }

        // 在生成位置生成預制物
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        Instantiate(fruit, spawnPosition, rotation);
        controler.fruit_numbers++;
        Debug.Log("生成!");
    }
}
