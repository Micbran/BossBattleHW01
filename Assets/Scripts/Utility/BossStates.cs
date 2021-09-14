using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BossStates
{
    public enum BossState
    {
        DefaultState = 1,
        MovingToPointRotationState = 2,
        RotationFireState = 3,
        MovingToPointStrafeState = 4,
        StrafeFireState = 5,
    }
}
