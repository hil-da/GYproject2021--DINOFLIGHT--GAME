using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneScript : MonoBehaviour
{
    public static GameSceneScript instance;

    // Use gameOverPanel to view and hide
    public GameObject gameOverPanel;

    // All texts that are used in gameOverPanel
    
    // Show kill count of star
    public Text killText;

    // Show distance from starting point
    public Text distanceText;

    // Show score which is the calculated score value - used for the money calculation
    public Text totalScoreText;

    // In game current score
    public Text inGameScoreText;

    // HighScore
    public Text inGameHighScoreText;

    // Show current money
    public Text currentMoneyText;

    // Show total money value
    public Text totalMoneyText;

    // Show current score value
    public Text currentScoreDisplayText;

    // Text counters
    // In game score value
    public int inGameScoreCounter;

    //In game highscore value
    public int inGameHighscoreCounter;

    // Calculate the score money
    public int totalScore;

    // Count kills
    public int killCounter;

    // Count the distance from player to platform
    public int distanceCounter;

    // Current money value
    public int currentMoneyCounter;
    
    // Calculated total money value ( final money value after all calculation)
    public int totalMoneyCounter;

    //================ Pop up text ================//
    // Here we assign popUpText prefab and change the properties of its components according to our need 
    public Transform refpopUpText;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // We are foing to stop the time when the player dies
        // So we need to reset it when the scene change or reloads
        Time.timeScale = 1;

        gameOverPanel.SetActive(false);

        // PlayerPrefs will use for saving something on memory and retrive that value
        // You can use string, int and float values to save and retrive
        // PlayerPrefs will save data as key and value pair
        // Check if highscore is available

        if (!PlayerPrefs.HasKey("HighScore")) {
            // SetInt (string, int) will save int value
            // if not then set it
            PlayerPrefs.SetInt("HighScore", 0);
        }

        // GetInt(string) will retuirn int from the key value you pass
        // Get highscore value from preference
        inGameHighscoreCounter = PlayerPrefs.GetInt("HighScore");

        // Reset current score and other values in game
        inGameHighscoreCounter = 0;
        distanceCounter = 0;
        killCounter = 0;
        totalScore = 0;
        totalMoneyCounter = 0;
    }

    // Go to main meny when the game is over or retry
    public void SceneCall(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void GameOver() {
        gameOverPanel.SetActive(true);

        // Calculate total score money by current score / 100
        totalScore = inGameScoreCounter / 100;

        // Current money is total score + total stars killed + distance between player starting position and the end position
        currentMoneyCounter = totalScore + killCounter + distanceCounter;

        // Add current money to total monet
        totalMoneyCounter = totalMoneyCounter + currentMoneyCounter;

        // Save total money
        PlayerPrefs.SetInt("totalMoney", totalMoneyCounter);

        // Set new highscore
        if (inGameScoreCounter > inGameHighscoreCounter) {
            inGameHighscoreCounter = inGameScoreCounter;
            PlayerPrefs.SetInt("HighScore", inGameHighscoreCounter);
        }

        // Show all values in text field of gameOverPanel
        inGameScoreText.text = inGameScoreCounter.ToString();
        inGameHighScoreText.text = inGameHighscoreCounter.ToString();
        killText.text = killCounter.ToString();
        distanceText.text = distanceCounter.ToString();
        totalScoreText.text = totalScore.ToString();
        currentMoneyText.text = currentMoneyCounter.ToString();
        totalMoneyText.text = totalMoneyCounter.ToString(); 
        
        // Freeze the game
        Time.timeScale = 0;

    }

    public void InitPopupText(Vector3 initPos, int value, bool isScore = true) {
        // Make new gameObject by instantiate popUpText prefab to position and set default rotation
        GameObject text = Instantiate(refpopUpText, initPos + new Vector3(0f, 1f, 0f), Quaternion.identity).gameObject;

        // Make an empty string variable that will use to store sign and value
        string str = "";

        // Assign sign before string value
        if (isScore) {
            // if value is score then "+ sign will add"
            str += "+";
        } else {
            // if value is not score (but money) then "$" sign will add
            str += "$";
        }

        // Add value which will change in string
        str += value;

        // Change popUpText of the text mesh component to our string
        text.GetComponent<TextMesh>().text = str;

        // Destroy popUpText after 2s (Text animation completes within 2s)
        Destroy(text, 2f);
    }

    // This function is used to call add score
    public void AddScore(int score) {
        totalScore += score; 
        totalScore = ComboController.instance.AddCombo(totalScore, score); 
        totalScoreText.text = totalScore.ToString();
        currentScoreDisplayText.text = totalScoreText.text;
    }
}
