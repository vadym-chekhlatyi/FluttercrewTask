using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private NPCBehavior behaviorController;
    [SerializeField] private Seeker seeker;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float minDistanceToWaypoint;

    private Path path;
    private int currentWaypoint = 0;
    private bool finishedMovement;
    private float movementSpeed;

    public bool FinishedMovement()
    {
        return finishedMovement;
    }

    public void WalkTo(Vector2 target, float speed)
    {
        movementSpeed = speed;
        CalculatePath(target);
    }

    private void CalculatePath(Vector2 target)
    {
        path = seeker.StartPath(rigidBody.position, target, onMovementEnd);
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            finishedMovement = true;
            return;
        }
        else
        {
            finishedMovement = false;
        }

        Vector2 movementDirection = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 movementVector = movementDirection * movementSpeed * Time.deltaTime;

        Debug.LogWarning("Force: " + movementVector);

        //rigidBody.AddForce(movementForce);
        transform.Translate(movementVector);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < minDistanceToWaypoint)
        {
            currentWaypoint++;
        }

        if (movementDirection.x > 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (movementDirection.x < 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void onMovementEnd(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }
}
