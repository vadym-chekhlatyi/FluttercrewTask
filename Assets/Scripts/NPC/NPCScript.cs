using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorStates
{
    None = 0,
    Idle = 1,

    Blinking = 2,
    Walking = 3,

    Count
}

public class NPCScript : MonoBehaviour
{
    [SerializeField] private ConfigNPC config;
    [SerializeField] private NPCAnimation npcAnimation;
    [SerializeField] private Transform spawnPosition;
    private float idleTimer;

    private void Awake()
    {
        idleTimer = config.idleTime;
        StartCoroutine(Idle());
    }

    private void Update()
    {

    }

    private IEnumerator Idle()
    {
        while(idleTimer > 0)
        {
            if(Random.Range(0,2) > 0)
            {
                npcAnimation.setAnimation(BehaviorStates.Idle);
            }
            else
            {
                npcAnimation.setAnimation(BehaviorStates.Blinking);
            }

            idleTimer--;
            yield return new WaitForSeconds(1f);
        }

        idleTimer = config.idleTime;

        StartCoroutine(Walk());
    }

    private int GetRandomWalkRange()
    {
        return Random.Range(config.minWalkRange, config.maxWalkRange + 1);
    }

    private int GetRandomWalkDirection()
    {
        if (Random.value > 0.5f)
        {
            return -1;
        }

        return 1;
    }

    private IEnumerator Walk()
    {
        // Enable Walking animation
        npcAnimation.setAnimation(BehaviorStates.Walking);

        // Get random walk range and direction
        int walkRange = GetRandomWalkRange();
        Vector2 walkDirection = new Vector2(GetRandomWalkDirection(), 0f);

        // Find final walk point
        Vector2 targetPoint = new Vector2(transform.position.x + (walkRange * walkDirection.x), transform.position.y);
        Vector2 spawnPosition = new Vector2(this.spawnPosition.position.x, transform.position.y);

        // If point is too far - get back to spawn
        if(Vector2.Distance(spawnPosition, targetPoint) > config.maxDistanceToSpawn)
        {
            targetPoint = spawnPosition;
            walkDirection = (targetPoint - (Vector2)transform.position).normalized;
        }

        // Find move per frame value
        float movePerFrame = walkDirection.x * config.walkSpeed * Time.deltaTime;

        // Rotate NPC to face the target
        if(movePerFrame > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // While NPC is 0.1 away from target point - move NPC
        while(Vector2.Distance(transform.position, targetPoint) > 0.1f)
        {
            transform.Translate(movePerFrame, 0f, 0f);
            yield return new WaitForEndOfFrame();
        }

        // Start Idling after walk
        StartCoroutine(Idle());
    }
}
