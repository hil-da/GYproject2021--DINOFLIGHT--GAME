using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboController : MonoBehaviour
{
    // To acces this script from anywhere in game scene
    public static ComboController instance;
    public bool isComboActive;

    // Combo deactive time
    [Range(0f, 4f)] public float comboTime;

    // Combo counter x1, x2, x3 etc
    public int comboCounter;

    // Limit the combo
    public int maxCombo;

    // Display the comboCounter
    public Text textCombo;

    // Animation of combo beat
    public Animator comboAnim;

    private void Start() {
        //First combo will deactivate
        isComboActive = false;
        // And multiplayer will be only 1
        comboCounter = 1;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public int AddCombo(int score, int addedScore) {
        // Check if combo is activated or not
        if (isComboActive) {
            // Increase the counter
            comboCounter++;

            // Cancel the previous iteration of StopCombo method
            CancelInvoke("StopCombo");

            // Recall call iteration of StopCombo method after given time
            // example : if comboTime = 3 then after 3 second Invoke "StopCombo"
            Invoke("StopCombo", comboTime);
        } else {
            // OR else start the combo
            StartCombo();
        }

        // Play animation clip from start
        comboAnim.Play("Combo Beat", 0, 0);

        // Clamp the value between 1 and max combo variable
        comboCounter = Mathf.Clamp(comboCounter, 1, maxCombo);

        // Display comboCounter value
        textCombo.text = "x" + comboCounter;

        // Return the calculated score to display
        return score + (addedScore * comboCounter);
    }
    
    public void StartCombo() {
        // Active combo
        isComboActive = true;

        // If in combo time no kill then this method will invoke and deactivate combo
        Invoke("StopCombo", comboTime);
    }

    public void StopCombo() {
        // Deactivate combo
        isComboActive = false;
        
        // Counter reset
        comboCounter = 1;

        // Display on counter into text
        textCombo.text = "x" + comboCounter;
    } 
}
