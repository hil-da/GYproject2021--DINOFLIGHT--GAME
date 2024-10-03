using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public static PlayerController instance;
    
    [Space(3)]
    [Header("========== Player Properties ==========")]
    [Space(3)]
   
    // playerBody with sprite
    public Transform playerBody;

    // playerParent rigidbody 2D component
    public Rigidbody2D rigidBody;

    // playerParent SpringJoint 2D
    public SpringJoint2D springJoint;

    [Space(3)]
    [Header("========== Rope Properties ==========")]
    [Space(3)]

    // Showing connected rope
    public LineRenderer line;
    
    // Minimum distance between rope and player
    public float minDistance;

    // Reduce distance speed between star and player
    [Range(0.1f, 5f)] public float grabSpeed = 1;

    [Space(3)]
    [Header("========== Star Properties ==========")]
    [Space(3)]

    // Current star transform
    public Transform currentStar;

    // Star hit will return star border point for grabbing
    private RaycastHit2D starHit;
    private bool isGrappling;

    // Star point will work as rope end star which will stick to the star and rotate with star
    private Transform starPoint;

    // This bool value will stop the functionality of grappling when player dies
    private bool isDead;

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        // Set isGrappling boolean false
        isGrappling = false;

        // Set Line pointscoint to 2, it means that there are 2 points available for rope 1st and 2nd end
        line.positionCount = 2;

        // Create enw game object in scene and assign it to the star point
        starPoint = new GameObject().transform;

        // Change star point name as "starPoint", by default its name was "New GameObject"
        starPoint.name = "starPoint";

    }

    private void Update()
    {
        if (isDead) 
            return;
        if (Input.GetMouseButtonDown(0)) // click
            // Grappling start stuff
            GrapplingStart();
        else if (Input.GetMouseButton(0)) // pressed or held
            // While grappling is running
            Grappling();
        else if (Input.GetMouseButtonUp(0)) // release
            // Grappling end stuff
            GrapplingEnd();
        //Set playerBody animation from speed
        SetPlayerBody();
    }

    private void SetPlayerBody() {
        // Set playerBody local scale according to rigidbody 2D speed
        playerBody.localScale = new Vector3( // z value
            Mathf.Clamp(Remap(rigidBody.velocity.magnitude, 0, 20, 1, 1.5f), 1, 1.5f), // x value 
            Mathf.Clamp(Remap(rigidBody.velocity.magnitude, 0, 20, 1, 0.7f), 0.7f, 1), 1); // y value 

        // To rotate playerBody to direction

        // Get direction of velocity
        Vector3 dir = rigidBody.velocity.normalized;

        // Get angle from direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Set rotation of playerBody with Quaternion.AngleAxis () method
        playerBody.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Remap function for remapping the value of float values
    // in simple example lets take percentage of 250 out of 500, how many precentage it will be
    // so our remap function will be
    // Remap (250, 0 500, 0, 100); it will return float precentage 50
    private float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // It will return the nearest hook from clicked position and with colliders transform
    private Transform GetNearestStar(Collider2D[] colliders, Vector3 clickedPos) {
        // Selected Index
        int index = 0;
        //Debug.Log(clickedPos);

        // Set infinity to determine the minimun distance
        float dist = Mathf.Infinity;
        float minDist = Mathf.Infinity;

        // Finding minimun distance from colliders transform to clicked position
        for (int i = 0; i < colliders.Length; i++) {
            
            dist = Vector3.Distance(colliders[i].transform.position, clickedPos);
            if (dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        if (colliders.Length > 0) {
            // Return transform of hook
            return colliders[index].transform;
        }
        return null;
    }

    private void GrapplingStart() {
        // True to the boolean
        isGrappling = true;

        // Get clicked position with reduce camera distance
        Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);

        // Store colliders which overlaps the area
        Collider2D[] colliders;

        // Get stars within overlap circle area
        colliders = Physics2D.OverlapCircleAll(clickedPos, 25, LayerMask.GetMask("Stars"));

        // Get nearest star
        currentStar = GetNearestStar(colliders, clickedPos);

        // if there's no stars found then no grappling
        if (currentStar == null) {
            isGrappling = false;
            return;
        }

        // If current star is available then raycast to star position and grab the star
        starHit = Physics2D.Raycast(transform.position, (currentStar.position - transform.position).normalized, 100, LayerMask.GetMask("Stars"));

        // After getting raycast data the transform will be available
        // Change the star point parent
        starPoint.parent = starHit.transform;

        // Now change the position od hook point to hit point
        starPoint.position = starHit.point;

        // Now enable the hook controller script
        starHit.transform.GetComponent<StarController>().enabled = true;

        // Set spring joint to starHit point
        springJoint.connectedAnchor = starHit.point;

        // Turn on the spring joint
        springJoint.enabled = true;

        // If the current star is available then enable true line renderer
        line.enabled = true; 
    }

    private void Grappling() {
        // Set 2 points of line
        // 1st is player and 2nd is starHit point
        line.SetPosition(0, transform.position);
        line.SetPosition(1, starPoint.position);

        // Reduce distance between hook and player
        springJoint.distance = Mathf.Lerp(springJoint.distance, minDistance, Time.deltaTime * grabSpeed);

        // Reduce the rigifbody speed to stop spinning the player around the hook
        rigidBody.velocity *= 0.995f;
    }

    private void GrapplingEnd() {
        // False the boolean
        isGrappling = false;

        // Turn off the spring joint
        springJoint.enabled = false;

        // Enable false linear renderer
        line.enabled = false;

        // Reset the line positions
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        // Change the star point parent to null
        starPoint.parent = null;

        // Now disable the hook controller script
        if (starHit) {
            starHit.transform.GetComponent<StarController>().enabled = false;
        }
    }

    public void HitStar(Transform star) {
        // Friendly Star
        if(star.tag == "star") {
            Debug.Log("Friendly star hit ... ");
            GameSceneScript.instance.AddScore(100);
            GameSceneScript.instance.InitPopupText(star.position, 100 * ComboController.instance.comboCounter);
        }

        // Blackhole 
        if(star.tag == "blackhole") {
            Debug.Log("Life Decreased ... ");
        }
    } 
}
 