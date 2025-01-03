using UnityEngine;

public class Agent : MonoBehaviour
{
    public float moveSpeed = 1.0f;     
    public float rotateSpeed = 0.5f;            
    private Vector3 goal;                          // NEXT WAYPOINT POSITION
    public Vector3 finalGoal;                     // FINAL GOAL POSITION
    private PathManager pathManager;               // REFERENCE TO PATHMANAGER
    public OrientationManager orientationManager;  // ORIENTATION HANDLER

    private bool isGoalReached = false;            // TRACK IF GOAL IS REACHED

    private void Start()
    {
        // FIND PATHMANAGER AND REGISTER AGENT
        pathManager = FindObjectOfType<PathManager>();
        if (pathManager != null)
        {
            pathManager.RegisterAgent(this);
            goal = pathManager.GetNextWaypoint(this);  // INITIALIZE NEXT WAYPOINT
            finalGoal = pathManager.GetFinalGoal(this); // INITIALIZE FINAL GOAL
        }
        else
        {
            Debug.LogError("PathManager not found for agent: " + gameObject.name);
        }
    }

    private void Update()
{
    if (isGoalReached) return;

    Vector3 direction = (goal - transform.position);
    float distanceToWaypoint = direction.magnitude;

   
    if (distanceToWaypoint < 0.5f)
    {
        pathManager.AdvanceToNextWaypoint(this);
        goal = pathManager.GetNextWaypoint(this); 
    }

    direction = direction.normalized;
    UpdateAgent(Time.deltaTime, direction);

   
    Debug.DrawLine(transform.position, goal, Color.blue);      
    Debug.DrawLine(transform.position, finalGoal, Color.red);   
}

    public void UpdateAgent(float deltaTime, Vector3 direction)
    {
        Vector3 velocity = direction * moveSpeed * deltaTime;
        transform.Translate(velocity, Space.World);

        orientationManager?.Rotate(direction, rotateSpeed);
    }
    public void UpdateFinalGoal(Vector3 newGoal)
    {
        finalGoal = newGoal;
    }

}
