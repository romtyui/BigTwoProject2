using UnityEngine;
using UnityEngine.Animations.Rigging;
using Unity.Collections;
using Unity.Mathematics;
using System.Collections.Generic;

public class JiggleChainBinder : AnimationJobBinder<JiggleChainJob, JiggleChainData>
{
    public override JiggleChainJob Create(Animator animator, ref JiggleChainData data, Component component)
    {
        // 初始化鏈條節點
        var chain = new List<Transform>();
        Transform tmp = data.tip;
        while (tmp != null && tmp != data.root)
        {
            chain.Add(tmp);
            tmp = tmp.parent;
        }
        chain.Reverse();

        // 創建 JiggleChainJob
        var job = new JiggleChainJob
        {
            chain = new NativeArray<ReadWriteTransformHandle>(chain.Count, Allocator.Persistent),
            dynamicTargetAim = new NativeArray<JiggleChainJob.DynamicTarget>(chain.Count, Allocator.Persistent),
            dynamicTargetRoll = new NativeArray<JiggleChainJob.DynamicTarget>(chain.Count, Allocator.Persistent),
            localAimDir = math.normalize(data.localAimVector),
            localUpDir = math.normalize(data.localUpVector)
        };

        // 初始化 NativeArray
        for (int i = 0; i < chain.Count; ++i)
        {
            job.chain[i] = ReadWriteTransformHandle.Bind(animator, chain[i]);
            float4x4 tx = float4x4.TRS(chain[i].position, chain[i].rotation, new float3(1f));
            job.dynamicTargetAim[i] = new JiggleChainJob.DynamicTarget
            {
                position = math.transform(tx, job.localAimDir * data.dynamicOffset),
                velocity = float3.zero
            };
            job.dynamicTargetRoll[i] = new JiggleChainJob.DynamicTarget
            {
                position = math.transform(tx, job.localUpDir * data.dynamicOffset),
                velocity = float3.zero
            };
        }

        // 綁定參數屬性
        job.massProperty = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.mass)));
        job.stiffnessProperty = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.stiffness)));
        job.dynamicOffsetProperty = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.dynamicOffset)));
        job.rollEnabledProperty = BoolProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.rollEnabled)));
        job.gravityProperty = Vector3Property.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.gravity)));
        job.externalForceProperty = Vector3Property.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.externalForce)));
        job.motionDecayProperty = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.motionDecay)));
        job.dampingProperty = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.damping)));

        return job;
    }

    public override void Destroy(JiggleChainJob job)
    {
        if (job.chain.IsCreated)
            job.chain.Dispose();
        if (job.dynamicTargetAim.IsCreated)
            job.dynamicTargetAim.Dispose();
        if (job.dynamicTargetRoll.IsCreated)
            job.dynamicTargetRoll.Dispose();
    }
}
