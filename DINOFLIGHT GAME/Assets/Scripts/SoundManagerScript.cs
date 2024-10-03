using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource uiButtonClickSound;
    public AudioSource playerHitSound;
    public AudioSource playerDieSound;
    public AudioSource playerSwingSound;
    public AudioSource coinCollectSound;
    public AudioSource enemyBlastSound;
    public static SoundManagerScript instance;
    
    private void Start() {
        //Checking if its the first time gameplay or not
        if (!PlayerPrefs.HasKey("FirstTimeSoundCheck")) {
            PlayerPrefs.SetInt("MusicOnOff", 1);
            PlayerPrefs.SetInt("SoundOnOff", 1);
            PlayerPrefs.SetInt("FirtsTimeSoundCheck", 0);
        }
        TurnMusicOnOff();
        TurnSoundOnOff();
        MakeSingleton();

    }

    public void TurnMusicOnOff() {
        //Checks if the music is turned on
        if(GetMusic() == 1) {
            backgroundMusic.enabled = true;
        } else {
            backgroundMusic.enabled = false;
        }
    }

    public void TurnSoundOnOff() {
        // Activates sounds
        if (GetSound() == 1) {
            uiButtonClickSound.enabled = true;
            playerHitSound.enabled = true;
            playerDieSound.enabled = true;
            playerSwingSound.enabled = true;
            coinCollectSound.enabled = true;
            enemyBlastSound.enabled = true;
        } else {
            uiButtonClickSound.enabled = false;
            playerHitSound.enabled = false;
            playerDieSound.enabled = false;
            playerSwingSound.enabled = false;
            coinCollectSound.enabled = false;
            enemyBlastSound.enabled = false;
        }
    }

    //Getters and setters to turn on and off sound
    public void SetMusic(int isOn) { 
        PlayerPrefs.SetInt("MusicOnOff", isOn);
    }
    public int GetMusic() {
        return PlayerPrefs.GetInt("MusicOnOff");
    }
    public void SetSound(int isOn) {
        PlayerPrefs.SetInt("SoundOnOff", isOn);
    }
    public int GetSound() {
        return PlayerPrefs.GetInt("SoundOnOff");
    }

    void MakeSingleton() {
        // checking if intance is null
        if(instance != null){     
            // Destroy the current gameobject(the new sound manager script) if there's is one  
            Destroy(gameObject);        
        } else {
            instance = this;
            // Don't destroy this gameobject when you load a new scene or move to another
            DontDestroyOnLoad(gameObject);      
        }
    }
}
