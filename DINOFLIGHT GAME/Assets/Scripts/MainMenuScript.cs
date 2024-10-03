using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Button musicOnButton;
    public Button musicOffButton;
    public Button soundOnButton;
    public Button soundOffButton;

    // Start is called before the first frame  
    void Start() 
    {
        if (PlayerPrefs.GetInt("MusicOnOff") == 1)
        {
            musicOnButton.interactable = false;
            musicOffButton.interactable = true;
        }

        if (PlayerPrefs.GetInt("SoundOnOff") == 1)
        {
            soundOnButton.interactable = false;
            soundOffButton.interactable = true;
        }
    }

    //Calls next scene
    public void NextSceneCall(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
    
    //Close the application
    public void Exitcall() {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Music state selection
    public void MusicOnOff(int musicID) {
        SoundManagerScript.instance.uiButtonClickSound.Play();
        SoundManagerScript.instance.SetMusic(musicID);
        SoundManagerScript.instance.TurnMusicOnOff();
    }

    //Sound state selection
    public void SoundOnOff(int soundID) {
        SoundManagerScript.instance.uiButtonClickSound.Play();
        SoundManagerScript.instance.SetSound(soundID);
        SoundManagerScript.instance.TurnSoundOnOff();

    }
}




