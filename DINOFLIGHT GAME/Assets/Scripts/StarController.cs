 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    // If player is grappling this star then it will be activated
    public bool isActive;

    // Target will be the player
    private Transform target;

    // Parent is empty game object
    private Transform parent;

    // It's used for previous eulerAngles of parent game object
    private Vector3 prev;
 
    // This method is called after the Awake() method
    private void OnEnable() {
        // Activated the star script
        isActive = true;

        // If parent is not null then do this
        if (parent != null) {
            // Save parent eulerAngles
            prev = transform.parent.eulerAngles;

            // Rotate star to player
            RotateTowards(PlayerController.instance.transform.position);

            // Set star eulerAngles as parent previous eulerAngles
            transform.eulerAngles = prev;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Get the target
        target = PlayerController.instance.transform;

        // Get parent of star
        parent = transform.parent;

        // Set parent eulerAngles
        prev = parent.eulerAngles;

        // Rotate the parent to target position
        RotateTowards(target.position);

        // Set star eulerAngles as parent previous eulerAngles
        transform.eulerAngles = prev;
    }

    private void RotateTowards(Vector2 target) {
        // offset is used for rotating to Z position
        float offset = 90f;

        // It will give direction to target
        Vector2 direction = target - (Vector2)transform.position;

        // Normalize the direction
        direction.Normalize();

        // Get star current angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set parent rotation
        parent.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards(target.position);
    }

    private void OnDisable() {
        // Deactivated the star
        isActive = false;

        // When player left current star then set rotation parent and child star
        parent.rotation = Quaternion.Euler(transform.eulerAngles);
        transform.rotation = Quaternion.Euler(parent.eulerAngles);
    }
}
