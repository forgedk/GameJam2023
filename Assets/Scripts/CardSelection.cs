using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardSelection : MonoBehaviour
{
    private UnityEngine.UI.Image imageCard;
    private Card cardRepresentation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(Card cardInSystem) 
    {
        cardRepresentation = cardInSystem;
        setImage(cardInSystem.image);

    }

    public void setImage(Sprite texture)
    {
        imageCard = GetComponent<UnityEngine.UI.Image>();
        imageCard.sprite = texture;
    }

}
