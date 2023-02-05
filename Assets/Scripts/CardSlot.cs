using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[Serializable]
public class EndTurn : UnityEvent<Player> { }

public class CardSlot : MonoBehaviour, IDropHandler
{

    public int[,] powerMatrix;
    public Card cardInSlot;
    public Player player;
    public UnityEngine.UI.Image imageCard;

    public int AttackPower =0;
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
        arrowManager.SetDamageArrows(cardInSlot,level, AttackPower, player); ;

    }

    public void OnDrop(PointerEventData eventData) 
    {
        Player searchPlayer = eventData.pointerDrag.GetComponent<CardSelection>().ownPlayer;
        Card posibleCard = eventData.pointerDrag.GetComponent<CardSelection>().cardRepresentation;
        if (boardController.CheckValidMove(row,col, posibleCard,searchPlayer)) 
        { 
            if(cardInSlot == posibleCard)
            {
                level= level + 1; ;
            }
            else
            {
                level = 1;
            }
            cardInSlot = posibleCard;
            player = searchPlayer;
            SetCardInBoard();
            endTurn.Invoke(player);

        }
    }


    public void AddToPower(int power,int row,int col) {
        if (powerMatrix[row,col] < power) { 
            powerMatrix[row,col] = power;    
        }
    }

    public void ResetPowerMatrix(int row, int col)
    {
        powerMatrix = new int[row, col];
    }

    public int GetPower(int row, int col)
    {
        int power = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                power += powerMatrix[i, j];
            }
        }
        return power;
    }

    public void ResetPower() {
        AttackPower = 0;
    }

}
