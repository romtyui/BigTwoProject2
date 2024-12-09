using UnityEngine;

public class TreeShake : MonoBehaviour
{
    public Transform trunk;  // 樹幹的Transform
    public Transform[] branches;  // 所有樹枝的Transform
    public float shakeStrength = 0.1f;  // 搖晃強度
    public float shakeDuration = 1f;  // 搖晃持續時間
    public float dampingFactor = 0.1f;  // 阻尼因子
    public float reboundSpeed = 0.5f;  // 回彈速度

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float shakeTime;

    void Start()
    {
        initialPosition = trunk.position;
        initialRotation = trunk.rotation;
    }

    void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;  // 更新搖晃時間
            ApplyShake();  // 應用搖晃效果
        }
        else
        {
            // 停止搖晃後，慢慢恢復
            trunk.position = Vector3.Lerp(trunk.position, initialPosition, Time.deltaTime * dampingFactor);
            trunk.rotation = Quaternion.Lerp(trunk.rotation, initialRotation, Time.deltaTime * dampingFactor);
        }
    }

    public void StartShake()
    {
        shakeTime = shakeDuration;  // 啟動搖晃
    }

    void ApplyShake()
    {
        // 基於正弦波生成搖晃的 x 和 y 坐標
        float shakeX = Mathf.Sin(Time.time * 5) * shakeStrength;
        float shakeY = Mathf.Cos(Time.time * 5) * shakeStrength;

        // 應用搖晃到樹幹
        trunk.position += new Vector3(shakeX, shakeY, 0);

        // 對每個樹枝應用擴大搖晃幅度的效果
        for (int i = 0; i < branches.Length; i++)
        {
            // 計算樹枝距離樹幹的相對距離
            float distanceFromTrunk = Vector3.Distance(trunk.position, branches[i].position);

            // 根據距離來調整搖晃的強度
            float branchShakeStrength = shakeStrength * (distanceFromTrunk / 10f);  // 距離越遠，搖晃強度越大
            float branchDamping = 1f - (i / (float)branches.Length);  // 隨著樹枝遠離根部，減少阻尼

            // 應用搖晃到樹枝
            branches[i].position += new Vector3(shakeX * branchShakeStrength, shakeY * branchShakeStrength, 0);

            // 讓樹枝的回復速度（阻尼）變慢，隨著樹枝距離樹幹越遠，回復越慢
            branches[i].position = Vector3.Lerp(branches[i].position, initialPosition, Time.deltaTime * branchDamping);

            // 引入反彈效果：越遠的樹枝回彈越慢
            branches[i].position = Vector3.Lerp(branches[i].position, initialPosition, Time.deltaTime * reboundSpeed * (distanceFromTrunk / 10f));
        }
    }
}
