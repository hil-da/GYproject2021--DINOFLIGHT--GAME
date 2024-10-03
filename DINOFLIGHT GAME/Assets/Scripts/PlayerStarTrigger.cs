using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStarTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c) {
        // Detect star and it need to be blackhole
        if(c.gameObject.layer == LayerMask.NameToLayer("Stars")) {
            // Hit the star
            PlayerController.instance.HitStar(c.transform);
            CameraController.instance.CallCameraShakeEffect();
        }

        // Detect lava
        if(c.tag == "lava") {
            Debug.Log("Player Died !! Game over ... ");
            GameSceneScript.instance.GameOver();
        }
    }
}
