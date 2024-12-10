using UnityEngine;
using UnityEngine.Animations.Rigging;
/// JiggleChain constraint component can be defined given it's job, data and binder
[DisallowMultipleComponent]
public class JiggleChain : RigConstraint<JiggleChainJob, JiggleChainData, JiggleChainBinder>
{
    // No additional implementation required here
}