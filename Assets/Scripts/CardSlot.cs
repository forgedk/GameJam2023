using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public Card cardInSlot;
    public UnityEngine.UI.Image imageCard;
    public int AttackPower;
    public int level = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardInBoard()
    {
        imageCard.sprite = cardInSlot.image;

    }

    public void OnDrop(PointerEventData eventData) 
    {
        cardInSlot = eventData.pointerDrag.GetComponent<CardSelection>().cardRepresentation;
        SetCardInBoard();

    }

}
