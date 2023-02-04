using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine;
using TMPro;


public class InfoPanel : MonoBehaviour
{
    public UnityEngine.UI.Image imageCard;
    public TMP_Text description;
    public TMP_Text title;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyInfo(Card cardToRepresent) {
        imageCard.sprite = cardToRepresent.image;
        description.text = cardToRepresent.text;
        title.text = cardToRepresent.title;


    }
}
