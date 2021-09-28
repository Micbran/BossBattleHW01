using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossStates;

public class BossMovement : MonoBehaviour
{
    private readonly float minDistance = 1e-12f;

    [SerializeField] private Transform defaultLeft;
    [SerializeField] private Transform defaultRight;
    [SerializeField] private Transform defaultCenter;
    [Space(10)]
    [SerializeField] private Transform rotationFireBack;
    [Space(10)]
    [SerializeField] private Transform strafeFireLeft;
    [SerializeField] private Transform strafeFireRight;
    [Space(10)]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float movingToPointSpeed;
    [SerializeField] private float rotationFireSpeed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float rotationRotateSpeed;
    [SerializeField] private float strafeRotateSpeed;
    [Space(10)]
    [SerializeField] private float rotationFireWeight = 0.5f;
    [SerializeField] private float rotationFireStep = 0.05f;
    [Space(10)]
    [SerializeField] private float strafeFireWeight = 0.5f;
    [SerializeField] private float strafeFireStep = 0.05f;
    [Space(10)]
    [SerializeField] private float specialStateWeight = 0.5f;
    [SerializeField] private float specialStateStep = 0.05f;
    [Space(10)]
    [SerializeField] private float stateTimerMax = 5.0f;
    [Space(10)]
    [SerializeField] private MeshRenderer stateIndicator;
    [SerializeField] private Color rotationColor;
    [SerializeField] private Color strafeColor;
    private Color defaultColor;

    private Transform currentDestination;
    private bool isOneHalf;
    private int rotationSign;
    private Transform baseTransform;
    private BossState state;

    private List<Transform> defaultOrder = new List<Transform>();

    private float stateTimer = 0;

    public BossState CurrentState
    {
        get
        {
            return this.state;
        }
    }

    private void Awake()
    {
        this.currentDestination = this.defaultLeft;
        this.baseTransform = this.transform;
        this.state = BossState.DefaultState;
        this.stateTimer = this.stateTimerMax;
        this.isOneHalf = false;
        this.rotationSign = 1;
        this.defaultColor = this.stateIndicator.material.color;
        defaultOrder.Add(this.defaultLeft);
        defaultOrder.Add(this.defaultRight);
        defaultOrder.Add(this.defaultCenter);
    }

    private void FixedUpdate()
    {
        switch(this.state)
        {
            case BossState.DefaultState:
                this.HandleDefaultState();
                this.DecideNextMove();
                break;
            case BossState.MovingToPointStrafeState:
            case BossState.MovingToPointRotationState:
                this.HandleMovingToPointState();
                break;
            case BossState.RotationFireState:
                this.HandleRotationFireState();
                break;
            case BossState.StrafeFireState:
                this.HandleStrafeFireState();
                break;
            default:
                break;
        }

    }

    private void HandleDefaultState()
    {
        if (CheckDistance())
        {
            this.currentDestination = this.defaultOrder[0];
            this.defaultOrder.RemoveAt(0);
            this.defaultOrder.Add(currentDestination);
        }

        this.MoveTowardsDestination();
    }

    private void HandleMovingToPointState()
    {
        if (CheckDistance())
        {
            this.TransitionToRotationOrStrafeFire();
        }

        this.MoveTowardsDestination();
    }

    private void HandleRotationFireState()
    {
        if (CheckDistance())
        {
            this.TransitionToDefaultState();
        }

        this.MoveTowardsDestination();
        this.Rotate();
    }

    private void HandleStrafeFireState()
    {
        if (CheckDistance())
        {
            this.TransitionToDefaultState();
        }

        this.MoveTowardsDestination();
        this.RotateBounce();
    }

    private void TransitionToMovingToPointRotation()
    {
        this.state = BossState.MovingToPointRotationState;
        this.currentDestination = this.defaultCenter;
        this.stateIndicator.material.color = this.rotationColor;
        this.ResetRotation();
    }

    private void TransitionToMovingToPointStrafe()
    {
        this.state = BossState.MovingToPointStrafeState;
        this.currentDestination = this.strafeFireLeft;
        this.stateIndicator.material.color = this.strafeColor;
        this.ResetRotation();
    }

    private void TransitionToRotationOrStrafeFire()
    {
        if (this.state == BossState.MovingToPointRotationState)
        {
            this.state = BossState.RotationFireState;
            this.currentDestination = this.rotationFireBack;
            this.stateIndicator.material.color = this.rotationColor;
            this.ResetRotation();
        }
        else if (this.state == BossState.MovingToPointStrafeState)
        {
            this.state = BossState.StrafeFireState;
            this.currentDestination = this.strafeFireRight;
            this.stateIndicator.material.color = this.strafeColor;
            this.ResetRotation();
        }
        else
        {
            this.state = BossState.DefaultState;
            this.stateIndicator.material.color = this.defaultColor;
            this.ResetRotation();
        }
    }

    private void TransitionToDefaultState()
    {
        this.currentDestination = this.defaultOrder[0];
        this.state = BossState.DefaultState;
        this.stateIndicator.material.color = this.defaultColor;
        this.ResetRotation();
    }

    private void DecideNextMove()
    {
        this.stateTimer -= Time.deltaTime;
        if (stateTimer > 0) return;

        stateTimer = this.stateTimerMax;
        // janky "not quite" pnrg, because I'm too lazy to have a starting value
        if (Random.value <= this.specialStateWeight)
        {
            this.specialStateWeight -= this.specialStateStep;
            if (Random.value <= this.rotationFireWeight)
            {
                this.rotationFireWeight -= this.rotationFireStep;
                this.TransitionToMovingToPointRotation();
            }
            else if (Random.value <= this.strafeFireWeight)
            {
                this.rotationFireWeight += this.rotationFireStep;
                this.strafeFireWeight -= this.strafeFireStep;
                this.TransitionToMovingToPointStrafe();
            }
            else
            {
                this.rotationFireWeight += this.rotationFireStep;
                this.strafeFireWeight += this.strafeFireStep;
            }
        }
        else
        {
            this.specialStateWeight += this.specialStateStep;
        }
    }

    private void MoveTowardsDestination()
    {
        this.baseTransform.position = 
            Vector3.MoveTowards(this.baseTransform.position, this.currentDestination.position, this.DetermineSpeed());
    }

    private void Rotate()
    {
        this.baseTransform.RotateAround(this.baseTransform.position, Vector3.up, this.rotationRotateSpeed * Time.deltaTime);
    }

    private void RotateBounce()
    {
        if (!this.isOneHalf && this.CheckRotation())
        {
            this.isOneHalf = true;
            this.rotationSign *= -1;
        }
        this.baseTransform.RotateAround(this.baseTransform.position, Vector3.up, this.strafeRotateSpeed * Time.deltaTime * -this.rotationSign);
    }

    private void ResetRotation()
    {
        this.baseTransform.rotation = Quaternion.Euler(0, 0, 0);
        this.isOneHalf = false;
        this.rotationSign = 1;
    }

    private float DetermineSpeed()
    {
        return this.state switch
        {
            BossState.DefaultState => this.defaultSpeed,
            BossState.MovingToPointStrafeState => this.movingToPointSpeed,
            BossState.MovingToPointRotationState => this.movingToPointSpeed,
            BossState.RotationFireState => this.rotationFireSpeed,
            BossState.StrafeFireState => this.strafeSpeed,
            _ => this.defaultSpeed
        };
    }

    private bool CheckDistance()
    {
        return Vector3.Distance(this.baseTransform.position, this.currentDestination.position) <= this.minDistance;
    }

    private bool CheckRotation()
    {
        return this.baseTransform.position.z >= 0;
    }
}
