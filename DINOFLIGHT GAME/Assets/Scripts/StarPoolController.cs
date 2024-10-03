using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPoolController : MonoBehaviour
{
    // To know player position we have to add player transform
    public Transform player;

    [Space(3)]
    [Header("========== Star Counter ==========")]
    [Space(3)]

    // Counter for active stars
    public int activeStarCounter;

    // Total stars we want in our scene
    public int totalStarCount; 

    [Space(3)]
    [Header("========== Star#1 ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int normalStarCount;

    // Prefab of normal stars
    public Transform refNormalStar;

    [Space(3)]
    [Header("========== Star#2 ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int normalStarCount2;

    // Prefab of normal stars
    public Transform refNormalStar2;

    [Space(3)]
    [Header("========== Star#3 ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int normalStarCount3;

    // Prefab of normal stars
    public Transform refNormalStar3;
    
    [Space(3)]
    [Header("========== Star#4 ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int normalStarCount4;

    // Prefab of normal stars
    public Transform refNormalStar4;

    [Space(3)]
    [Header("========== Star#5 ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int normalStarCount5;

    // Prefab of normal stars
    public Transform refNormalStar5;

    [Space(3)]
    [Header("========== Star#6 ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int normalStarCount6;

    // Prefab of normal stars
    public Transform refNormalStar6;
    [Space(3)]
    [Header("========== Blackhole ==========")]
    [Space(3)]

    // Set value how many stars you want to instantiate
    public int blackholeCount;

    // Prefab of normal stars
    public Transform refBlackhole;


    [Space(3)]
    [Header("========== Star Instantiate Values ==========")]
    [Space(3)]

    // Rate of star instantiate per frame
    public int ratePerFrame;

    [Space(3)]
    [Header("========== Star Storage ==========")]
    [Space(3)]

    // List of active stars
    public List<Transform> starList;
    
    [Space(3)]
    [Header("========== Star Distance Conditional Values ==========")]
    [Space(3)]

    // Minimun distance from player
    public int minDist;

    // Maximum distance from player
    public int maxDist;

    // Far distance from player
    public int farDist;


    private void Awake() {
        // Calculate total stars before start method
        totalStarCount = normalStarCount + normalStarCount2 + normalStarCount3 + normalStarCount4 + normalStarCount5 + normalStarCount6 + blackholeCount;
    }

    // Instantiate stars
    private IEnumerator InitStars(Transform refStar, int starCount) {
        // Run the loop until star count will be greater than 0;
        while(starCount > 0) {
            // For loop for rate per frame variable
            // This loop will run until i is less than ratePerFrame and hook count greater than 0
            for (int i = 0; i < ratePerFrame && starCount > 0; i++) {
                // Instantiate the star
                Transform star = Instantiate(refStar);

                // Change the star parent to StarPoolerController game object
                star.parent = transform;

                // Add star to list
                starList.Add(star);

                // Change star position
                ChangeStarPos(star.GetChild(0));

                // Increase the active star counter
                activeStarCounter++;

                // Decrease star count
                starCount--;
            }
            // Wait for frame to end
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the stars in scene
        StartCoroutine(InitStars(refNormalStar, normalStarCount));
        StartCoroutine(InitStars(refNormalStar2, normalStarCount2));
        StartCoroutine(InitStars(refNormalStar3, normalStarCount3));
        StartCoroutine(InitStars(refNormalStar4, normalStarCount4));
        StartCoroutine(InitStars(refNormalStar5, normalStarCount5));
        StartCoroutine(InitStars(refNormalStar6, normalStarCount6));
        StartCoroutine(InitStars(refBlackhole, blackholeCount));

        StartCoroutine(StarManager());

    }

    //  Chanfe the position of star
    private void ChangeStarPos(Transform starChild, bool isFar = false) {
        // Get random direction
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

        // Define dist variable
        float dist;

        // Check if star will be far or near
        // According to that set the distance
        if (isFar) {
            dist = Random.Range(farDist, maxDist);
        } else {
            dist = Random.Range(minDist, maxDist);
        }

        // Create new Vector3 variable for new position from player
        Vector3 newPos = player.position + (dir * dist);

        // First we will check if new position has far distance if isFar is true
        if (isFar && Vector3.Distance(newPos, player.position) < farDist) {
            ChangeStarPos(starChild, isFar);
            return;
        }
        // If first condition will be false, then we check the hoos is not near from other hook and ground
        // Be sure that "Lava" and "planetMars" has ground layer assigned. 
        else if (Physics2D.OverlapCircle(newPos, 1f, LayerMask.GetMask("Stars", "Ground"))) {
            ChangeStarPos(starChild, isFar);
            return;
        }

        // After all check up change the star parent position
        starChild.parent.position = newPos;

        // Change the rotation of child and parent of star
        starChild.eulerAngles = Vector3.zero;
        starChild.parent.eulerAngles = Vector3.zero;
    }

    // Checking star positions and change star if its far from player
    private IEnumerator StarManager() {
        // Wait until all stars are in the scene
        yield return new WaitUntil(() => activeStarCounter == totalStarCount);

        // Storing distance of stars from player in each loop iteration
        float dist;

        // Infinite loop for checking distance
        while (true) {
            // Get all the stars from list
            foreach (Transform star in starList)
            {
                // Calculate the distance between the star and player position
                dist = Vector3.Distance(star.position, player.position);

                // Check if distance is greater than max distance
                if (dist > maxDist) {
                    // Change position of star and we want it far away from player
                    ChangeStarPos(star.GetChild(0), true);
                }
            }
            // Wait for end frame
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
