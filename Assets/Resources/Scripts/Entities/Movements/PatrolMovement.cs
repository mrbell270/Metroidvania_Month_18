
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MovementController
{
    [Header("Patrol")]
    [SerializeField]
    List<Vector2> patrolTrail = new List<Vector2>();
    [SerializeField]
    float patrolPointPause = 2f;
    float patrolPointTimer = 0f;
    float patrolPointRadius = 0.02f;
    bool isAtPatrolPoint;
    int curPatrolPointIdx = 0;

    [Header("Navigator options")]
    [SerializeField] float gridRadius = 0.02f;
    Seeker seeker;
    bool isSearchingPath = false;
    float pathSearchTimerConst = 0.5f;
    float pathSearchTimerCur = 0f;
    [SerializeField]
    List<Vector3> pathLeftToGo = new List<Vector3>();


    private void Awake()
    { 
        InitializeMovement();
        seeker = GetComponent<Seeker>();

        if (patrolTrail.Count == 0)
        {
            patrolTrail.Add(Vector2.zero);
        }

        List<Vector2> patrolTrailTmp = new List<Vector2>();
        foreach (Vector3 p in patrolTrail)
        {
            patrolTrailTmp.Add(p + transform.position);
        }
        patrolTrail = patrolTrailTmp;
    }

    public void Patrol()
    {
        if (!isAtPatrolPoint)
        {
            if (Mathf.Abs(Vector2.Distance(transform.position, patrolTrail[curPatrolPointIdx])) < patrolPointRadius)
            {
                ParentActor.movementController.MovementVector = Vector2.zero;
                isAtPatrolPoint = true;
                patrolPointTimer = 0f;
            }
            else
            {
                //move
                if (pathLeftToGo.Count > 0) //if the target is not yet reached
                {
                    MovementVector = (pathLeftToGo[0] - transform.position).normalized;
                    if ((transform.position - pathLeftToGo[0]).sqrMagnitude < gridRadius)
                    {
                        pathLeftToGo.RemoveAt(0);
                    }
                }
                else
                {
                    GetMoveCommand(patrolTrail[curPatrolPointIdx]);
                }
            }
        }
        else
        {
            patrolPointTimer += Time.deltaTime;
            if (patrolPointTimer >= patrolPointPause)
            {
                isAtPatrolPoint = false;
                curPatrolPointIdx = (curPatrolPointIdx + 1) % patrolTrail.Count;
                GetMoveCommand(patrolTrail[curPatrolPointIdx]);
            }
        }

        Move();
    }

    public void Follow(Vector2 position)
    {
        isAtPatrolPoint = false;

        pathSearchTimerCur += Time.deltaTime;
        if (pathSearchTimerCur > pathSearchTimerConst)
        {
            pathSearchTimerCur = 0f;
            GetMoveCommand(position);
        }

        if (pathLeftToGo.Count > 0) //if the target is not yet reached
        {
            MovementVector = (pathLeftToGo[0] - transform.position).normalized;
            if ((transform.position - pathLeftToGo[0]).sqrMagnitude < gridRadius)
            {
                pathLeftToGo.RemoveAt(0);
            }
        }

        Move();
    }

    public override void Stop()
    {
        pathLeftToGo.Clear();
        base.Stop();
    }


    //////////////////
    // Navigation
    void GetMoveCommand(Vector2 target)
    {
        if (!isSearchingPath)
        {
            isSearchingPath = true;
            seeker.StartPath(transform.position, target, OnPathComplete);
        }

        //Path p = seeker.StartPath(transform.position, target);
        //pathLeftToGo = p.vectorPath;
        //p.BlockUntilCalculated();
    }

    public void OnPathComplete(Path p)
    {
        isSearchingPath = false;
        if (p.error)
        {
            isSearchingPath = false;
            Debug.Log("ERROR: No path for " + gameObject.name);
        }
        else
        {
            pathLeftToGo = p.vectorPath;
        }
    }
    // Navigation end
    //////////////////


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GetMoveCommand(patrolTrail[curPatrolPointIdx]);
    }

    private void OnDrawGizmosSelected()
    {
        for(int i = 0; i < patrolTrail.Count; i++)
        {
            int j = (i + 1) % patrolTrail.Count;

            Gizmos.DrawLine((Vector3)patrolTrail[i] + transform.position, (Vector3)patrolTrail[j] + transform.position);
        }
    }
}
