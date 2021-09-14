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
    private Transform currentDestination;
    private Transform baseTransform;
    private BossState state;

    private List<Transform> defaultOrder = new List<Transform>();

    private float stateTimer = 0;

    private void Awake()
    {
        this.currentDestination = this.defaultLeft;
        this.baseTransform = this.transform;
        this.state = BossState.DefaultState;
        this.stateTimer = this.stateTimerMax;
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
    }

    private void HandleStrafeFireState()
    {
        if (CheckDistance())
        {
            this.TransitionToDefaultState();
        }

        this.MoveTowardsDestination();
    }

    private void TransitionToMovingToPointRotation()
    {
        Debug.Log("Moving to point rotation triggered.");
        this.state = BossState.MovingToPointRotationState;
        this.currentDestination = this.defaultCenter;
    }

    private void TransitionToMovingToPointStrafe()
    {
        Debug.Log("Moving to point strafe triggered.");
        this.state = BossState.MovingToPointStrafeState;
        this.currentDestination = this.strafeFireLeft;
    }

    private void TransitionToRotationOrStrafeFire()
    {
        if (this.state == BossState.MovingToPointRotationState)
        {
            Debug.Log("Rotation fire triggered.");
            this.state = BossState.RotationFireState;
            this.currentDestination = this.rotationFireBack;
        }
        else if (this.state == BossState.MovingToPointStrafeState)
        {
            Debug.Log("Strafe fire triggered.");
            this.state = BossState.StrafeFireState;
            this.currentDestination = this.strafeFireRight;
        }
        else
        {
            Debug.Log("Reset to default.");
            this.state = BossState.DefaultState;
        }
    }

    private void TransitionToDefaultState()
    {
        Debug.Log("Default state triggered.");
        this.currentDestination = this.defaultOrder[0];
        this.state = BossState.DefaultState;
    }

    private void DecideNextMove()
    {
        this.stateTimer -= Time.deltaTime;
        if (stateTimer > 0) return;

        Debug.Log("State timer triggered.");
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
}
