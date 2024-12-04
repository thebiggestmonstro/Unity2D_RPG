using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private PlayerController _controller => GetComponentInParent<PlayerController>();

    private void AnimationTrigger()
    {
        _controller.AnimationTrigger();
    }
}
