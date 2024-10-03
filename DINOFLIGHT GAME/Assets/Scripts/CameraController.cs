using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float shakeForce = 5f;
    public Camera myCamera;

    // Change size of orthigraphic for zoom in n out effect
    public float minSize;
    public float maxSize;

    // Follow speed of camera
    [Range (1f, 10f)] public float speed; 

    // Is the target followed by camera or not?
    public bool isFollow;
    
    // Who does the camera follow? (player, dino)
    public Transform target;

    // Rigidbody2D for calculating the speed and use that info into zoom effects
    private Rigidbody2D targetRigidbody;
     
    // Get some distance from the target
    public Vector3 padding;

    private void Awake() {
        // Makes sure we only have one camera, not multiple
        instance = this;
    }

    void Start()
    {
        // Fetches component of player
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        isFollow = true;
    }

    private void FixedUpdate() {
        // Check isFollow
        if (isFollow)
        {
            // Change the position of transform
            transform.position = Vector3.Lerp(transform.position, target.position + padding, Time.deltaTime * speed * Vector3.Distance(transform.position, target.position));
        }
    }

    private void LateUpdate() {
        // Check isFollow
        if (isFollow)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Mathf.Clamp(Remap(targetRigidbody.velocity.magnitude, 0, 200, minSize, maxSize), minSize, maxSize), Time.deltaTime * speed);
        }
    }

    private float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private IEnumerator IShake() {
        
        float offset = 0.1f * shakeForce;
       
        // Set max offset
        float max = offset;

        // Set min offset
        float min = offset - 0.1f;

        // Set duration of shaking
        float duration = 0.25f;

        // Time of shaking mechanism
        float t = 0f;

        // Get new position each time
        Vector3 newPos;

        // Get MainCamera transform for changing positions while shaking
        Transform cam = transform.GetChild(0);

        // Set camera rotation to zero
        cam.eulerAngles = Vector3.zero;
 
        // Slow the time
        Time.timeScale = 0.5f;

        // Wait a little bit
        yield return new WaitForSecondsRealtime(0.1f);

        // Loop for shaking
        while (t < duration) {
            t += Time.deltaTime;

            // Set new position in camera
            newPos = new Vector3(Random.Range(min, max), Random.Range(min, max), 0); // x, y and z value

            // Here we change local position of camera (not cameraHandler)
            cam.localPosition = newPos;

            // We change the rotation of camera
            cam.rotation = Quaternion.Euler(Vector3.forward * Random.Range(-2f, 2f));

            // Slow motion to normal motion transaction in each loop
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, t / duration);

            yield return null;
        }

        // Reset cam position
        cam.localPosition = Vector3.zero;

        // Reset rotation to zero
        cam.eulerAngles = Vector3.zero;

        // Reset time scale to 1 (normal speed)
        Time.timeScale = 1f;
    }

    public void CallCameraShakeEffect() {
        // Stop the previous shake
        StopCoroutine(IShake());

        // Start shaking
        StartCoroutine(IShake());
    }
}
