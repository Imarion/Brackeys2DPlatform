using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{

    public Transform target; // what to chase
    public float updateRate = 2f; // How many times a second we will update the path

    private Seeker seeker;
    private Rigidbody2D rb;

    // The calculated path
    public Path path;

    //The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to next waypoint.
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int curentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null) {
            Debug.LogError("No player found !");
            return;
        }

        // Start a new path to the target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPathComplete(Path p) {
        Debug.Log("We got a path, result/error: " + p.error);
        if (!p.error) {
            path = p;
            curentWaypoint = 0;
        }
    }
}
