using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter SelectAudio;
    public FMODUnity.StudioEventEmitter PlayAudio;
    public Sprite image,imageInGame;
    public int damageLeft, damageRight, damageUp;
    public string title;
    public string text;

    // Start is called before the first frame update
    void Start()
    {
    


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playVoice()
    {
        SelectAudio.Play();
    }

    public void playPuesta()
    {
        PlayAudio.Play();
    }

}
