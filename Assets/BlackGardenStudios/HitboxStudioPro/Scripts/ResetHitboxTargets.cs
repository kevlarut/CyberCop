using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackGardenStudios.HitboxStudioPro
{
    /// <summary>
    /// Use this behavior on attack animator states, specifically attacks that combo into themselves.
    /// Reports usually clear themselves when animations change, but an attack that repeats or combos
    /// into itself will not clear reports.
    /// </summary>
    public class ResetHitboxTargets : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<HitboxManager>().ResetReports();
        }
    }
}