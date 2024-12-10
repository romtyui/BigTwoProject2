using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public struct JiggleChainData : IAnimationJobData
{
    [SyncSceneToStream] public Transform root;
    public Transform tip;

    [Header("Properties")]
    [SyncSceneToStream] public float mass;
    [SyncSceneToStream, Range(0f, 1f)] public float stiffness;
    [SyncSceneToStream, Range(0.1f, 100f)] public float dynamicOffset;

    public Vector3 localAimVector;
    public Vector3 localUpVector;
    [SyncSceneToStream] public bool rollEnabled;

    [Header("Forces")]
    [SyncSceneToStream] public Vector3 gravity;
    [SyncSceneToStream] public Vector3 externalForce;

    [Header("Simulation Settings")]
    [SyncSceneToStream, Range(0f, 1f)] public float damping;
    [SyncSceneToStream, Range(0f, 1f)] public float motionDecay;

    public bool IsValid()
    {
        // 檢查 root 和 tip 是否有效
        return root != null && tip != null && root != tip;
    }

    public void SetDefaultValues()
    {
        // 設置默認值
        root = null;
        tip = null;
        mass = 1f;
        stiffness = 0.5f;
        dynamicOffset = 4f;
        localAimVector = new Vector3(1f, 0f, 0f);
        localUpVector = new Vector3(0f, 1f, 0f);
        rollEnabled = false;
        gravity = Vector3.zero;
        externalForce = Vector3.zero;
        damping = 0f;
        motionDecay = 0.1f;
    }
}
