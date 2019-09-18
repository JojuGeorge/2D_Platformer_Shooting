using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    // How many times each second we will update our path
    public float updateRate = 1f;

    // Cacheing
    Seeker seeker;
    Rigidbody2D rb;

    // The calculated path
    public Path path;

    // The AI's speed/sec
    public float speed = 300f;
    public ForceMode2D forceMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // Max distance from the AI to a waypoint for it to continue to the next way point
    public float nextWayPointDistance = 3f;

    // The waypoint we are currently moving towards
    int currentWayPoint=0;

    public float playerSearchDelay = .3f;
    bool searchingForPlayer = false;





    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine("SearchingForPlayer");
            }
            return;
        }

        // Start a new path to target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        // Update path if the player moves. if updateing in every frame then it clutter the CPU
        StartCoroutine("UpdatePath");
    }





    IEnumerator SearchingForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");

        if(sResult == null)
        {
            yield return new WaitForSeconds(playerSearchDelay);
            StartCoroutine("SearchingForPlayer");
        }
        else
        {
            searchingForPlayer = false;
            target = sResult.transform;
            StartCoroutine("UpdatePath");
        }
    }




    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine("SearchingForPlayer");
            }
            yield break;
        }

        // Start a new path to target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine("UpdatePath");
    }





    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Did it have an error ? "+p.error);
        if (!p.error) {
            path = p;
            currentWayPoint = 0;
        }
    }




    private void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine("SearchingForPlayer");
            }
            return;
        }

        if (path == null)
        {
            return;
        }

        // path.vectorPath return all of the waypoint as an array
        // Checking if  current way point that we are at is >= total amount of waypoints in the array. if it is then we have reached the end
        if(currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        // Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;    // Gives us the Direction
        dir *= speed * Time.fixedDeltaTime;      // Time.fixedDeltaTime since we are inside FixedUpdate()

        // Move the AI
        rb.AddForce(dir, forceMode);

        // Check if we are close enough to the next waypoint and if we are we preceed to follow the next waypoint
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if (dist < nextWayPointDistance) {
            currentWayPoint++;
            return;
        }
    }


}
