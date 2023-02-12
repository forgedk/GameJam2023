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

    private int[,] powerMatrix;
    private Card cardInSlot;
    private Player player;
    public UnityEngine.UI.Image imageCard;

    private int attackPower = 0;
    private int level = 1;

    [SerializeField]
    private ArrowManager arrowManager;

    [SerializeField]
    private BoardController boardController;

    private int rowPosition;
    private int colPosition;

    public EndTurn endTurn;

    public int[,] PowerMatrix { get => powerMatrix; set => powerMatrix = value; }
    public Card CardInSlot { get => cardInSlot; set => cardInSlot = value; }
    public Player Player { get => player; set => player = value; }
    public int AttackPower { get => attackPower; set => attackPower = value; }
    public int Level { get => level; set => level = value; }
    public int ColPosition { get => colPosition; set => colPosition = value; }
    public int RowPosition { get => rowPosition; set => rowPosition = value; }
    public BoardController BoardController { get => boardController; set => boardController = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData) 
    {
        Player actualPlayer = eventData.pointerDrag.GetComponent<CardSelection>().ownPlayer;
        Card posibleCard = eventData.pointerDrag.GetComponent<CardSelection>().cardRepresentation;

        if (BoardController.CheckValidMove(RowPosition,ColPosition, posibleCard,actualPlayer)) 
        { 
            if(CardInSlot == posibleCard)
            {
                Level= Level + 1;
            }
            else
            {
                Level = 1;
            }

            CardInSlot = posibleCard;
            Player = actualPlayer;

            CardInSlot.playPuesta();
            SetCardInBoard();
            endTurn.Invoke(Player);
        
        }
    }

    public void SetCardInBoard()
    {
        imageCard.sprite = CardInSlot.imageInGame;
        arrowManager.SetDamageArrows(CardInSlot, Level, AttackPower, Player); ;
    }


    public void AddToPower(int power,int row,int col) {
        if (PowerMatrix[row,col] < power) { 
            PowerMatrix[row,col] = power;    
        }
    }

    public void ResetPowerMatrix(int row, int col)
    {
        PowerMatrix = new int[row, col];
    }

    public int GetPower(int row, int col)
    {
        int power = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                power += PowerMatrix[i, j];
            }
        }
        return power;
    }

    public void ResetPower() {
        AttackPower = 0;
    }

}
