using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Seasonal_Control;

public class fruit_generate : MonoBehaviour
{
    public Seasonal_Control controler;
    public GameObject fruit;
    public float minRange = 0.2f; // �̤p�d��
    public float maxRange = 0.5f; // �̤j�d��
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

        // ��� Plane �� MeshRenderer
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("Plane does not have a MeshRenderer component."+this.gameObject.name);
            return;
        }

        // ��� Plane ����ɽd��
        Bounds bounds = meshRenderer.bounds;

        // �H����ܤ@�Ӥ�V
        int randomSide = Random.Range(0, 6); // 0: �W, 1: �U, 2: ��, 3: �k, 4: �e, 5: ��
        Vector3 spawnPosition = Vector3.zero;

        switch (randomSide)
        {
            case 0: // �W
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Mathf.Lerp(bounds.min.y, bounds.max.y, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 1: // �U
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Mathf.Lerp(bounds.min.y, bounds.max.y, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 2: // ��
                spawnPosition = new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 3: // �k
                spawnPosition = new Vector3(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, Random.Range(minRange, maxRange)),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
                break;

            case 4: // �e
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Mathf.Lerp(bounds.min.z, bounds.max.z, Random.Range(minRange, maxRange))
                );
                break;

            case 5: // ��
                spawnPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Mathf.Lerp(bounds.min.z, bounds.max.z, Random.Range(minRange, maxRange))
                );
                break;
        }

        // �b�ͦ���m�ͦ��w�
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        Instantiate(fruit, spawnPosition, rotation);
        controler.fruit_numbers++;
        Debug.Log("�ͦ�!");
    }
}
