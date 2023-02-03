using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCBehavior : MonoBehaviour
{
    private BehaviorStates currentState;
    public BehaviorStates CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;

            if(currentState == BehaviorStates.None)
            {
                SetRandomBehavior();
            }

            HandleBehaviorChange();
        }
    }

    [SerializeField] private NPCAnimation npcAnimation;
    //[SerializeField] private NPCMovement npcMovement;
    [SerializeField] private ConfigNPC config;
    [SerializeField] private Transform startPosition;

    private void Start()
    {
        SetIdleBehavior();
    }

    private BehaviorStates ChooseNewTask()
    {
        int statesCount = (int)BehaviorStates.Count;
        int random = Random.Range(1, statesCount);

        return (BehaviorStates)random;
    }

    private void SetIdleBehavior()
    {
        CurrentState = BehaviorStates.Idle;
    }

    private void SetNoneBehavior()
    {
        CurrentState = BehaviorStates.None;
    }

    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(1f);

        SetNoneBehavior();
    }

    private void HandleBehaviorChange()
    {
        if(CurrentState == BehaviorStates.None)
        {
            return;
        }

        npcAnimation.setAnimation(CurrentState);

        if (CurrentState == BehaviorStates.Idle) {
            StartCoroutine(Idle());
        }

        if (CurrentState == BehaviorStates.Blinking) {
            StartCoroutine(Blink());
        }

        if (CurrentState == BehaviorStates.Walking)
        {
            StartCoroutine(WalkInDirection());
        }

    }

    private void SetRandomBehavior()
    {
        CurrentState = ChooseNewTask();
    }

    private int GetRandomWalkRange()
    {
        return Random.Range(config.minWalkRange, config.maxWalkRange + 1);
    }

    private int GetRandomWalkDirection()
    {
        if(Random.value > 0.5f)
        {
            return -1;
        }

        return 1;
    }

    private IEnumerator WalkInDirection()
    {
        Debug.LogError("Walking start");
        int walkRange = GetRandomWalkRange();
        int walkDirection = GetRandomWalkDirection();
        if (walkDirection > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        Vector2 destinationPoint = new Vector2(transform.position.x + (walkRange * walkDirection), transform.position.y);

        if(Vector2.Distance(destinationPoint, (Vector2)startPosition.position) > config.maxDistanceToSpawn)
        {
            destinationPoint = startPosition.position;
        }

        Debug.Log("DestinationPoint: " + destinationPoint.x + ":" + destinationPoint.y);

        while(Vector2.Distance(transform.position, destinationPoint) > 0.5f)
        {
            Vector2 movementVector = (destinationPoint - (Vector2)transform.position).normalized;
            float movePerFrame = movementVector.x * config.walkSpeed * Time.deltaTime;

            transform.Translate(movePerFrame, 0f, 0f);
            yield return new WaitForEndOfFrame();
        }
        Debug.LogError("Walking end");

        SetNoneBehavior();
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(config.idleTime);

        SetNoneBehavior();
    }
}
