using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class EndTurn : UnityEvent<Player> { }

public class CardSlot : MonoBehaviour, IDropHandler
{
    public Card cardInSlot;
    public Player player;
    public UnityEngine.UI.Image imageCard;

    public int AttackPower;
    public int level = 1;
    public ArrowManager arrowManager;
    public BoardController boardController;

    public int row;
    public int col;

    public EndTurn endTurn;

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
        arrowManager.SetDamageArrows(cardInSlot, player);




    }

    public void OnDrop(PointerEventData eventData) 
    {
        Player searchPlayer = eventData.pointerDrag.GetComponent<CardSelection>().ownPlayer;
        Card posibleCard = eventData.pointerDrag.GetComponent<CardSelection>().cardRepresentation;
        if (boardController.CheckValidMove(row,col, posibleCard,searchPlayer)) 
        { 
            cardInSlot = posibleCard;
            player = searchPlayer;
            SetCardInBoard();
            endTurn.Invoke(player);

        }
    }


    public void AddToPower(ref int power) {
        AttackPower += power;
    }

    public void ResetPower() {
        AttackPower = 0;
    }

}
