using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public static MusicManager instance;

    //init audio sources
    public AudioClip[] music; // append all music for use here
    private AudioSource godSource; // main audio player

    // standard singletone practice: use awake instead of start
    void Awake(){
        // need this instance in order to access in other scripts
        //Debug.Log("Made player instance");
        if(instance != this && instance != null){
            //Debug.Log("Destroyed player instance");
            Destroy(this);
        }
        instance = this; //make an instance
        
        godSource = gameObject.AddComponent<AudioSource>();
    }

    public void playTrack1(){
        godSource.Stop();
        godSource.clip = music[0];
        // godSource.volume = 0.35f;
        godSource.Play();
    }
}
