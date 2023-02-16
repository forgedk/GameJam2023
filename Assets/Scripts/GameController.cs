using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public struct DamageInput
{
    public DamageInput(int damage, Player player)
    {
        Damage = damage;
        Player = player;
    }
    public int Damage { get; }
    public Player Player { get; }
}

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int numberOfRows;
    [SerializeField]
    private int numberOfColumns;

    public BoardController boardController;
    public DrawPanel drawController;
    public Player player1, player2;

    public TMP_Text player1HitPoints;
    public TMP_Text player2HitPoints;
    public TMP_Text textInfoMatch;

    public GameObject panelInfoMatch;



    // Start is called before the first frame update
    void Start()
    {
        boardController.CreateCardSlots(numberOfColumns, numberOfRows, 0.5f, 0.5f);
        SetTurn(player1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public void SetTurn(Player player)
    {
        StartCoroutine(ShowNextTurn(string.Format("Turno : {0}",player.name)));
        drawController.SetActive(true);
        drawController.SetCards(player);
    }

    public void EndTurn(Player player) {
        UpdateDamageCards(player);
        StartFight(player);

        player1HitPoints.text = string.Format("Jugador 1: {0}", player1.lifePoints);
        player2HitPoints.text = string.Format("Jugador 2: {0}", player2.lifePoints);


        if (CheckIfPlayerLose()) {
            return;
        }

        if (player == player1)
        {
            SetTurn(player2);
        }
        else
        {
            SetTurn(player1);
        }



    }

    public void UpdateDamageCards(Player player) {
    boardController.ResetCardPower(numberOfColumns, numberOfRows);

    bool[,,,] relationMatrix =  boardController.GetGraphMatrix(numberOfColumns,numberOfRows);
    bool[,] nodesVisited = new bool[numberOfColumns, numberOfRows];

    List<Vector2Int> initialNodes = boardController.GetInitialNode(relationMatrix, numberOfColumns, numberOfRows);
    Stack<Vector2Int> stackNodesToUpdate = new Stack<Vector2Int>(initialNodes);

        while (stackNodesToUpdate.Count > 0)
        {
          
            Vector2Int nodeToSearch = stackNodesToUpdate.Pop();
            nodesVisited[nodeToSearch.x,nodeToSearch.y] = true;

            if (boardController.CardsSlot[nodeToSearch.x, nodeToSearch.y].transform.GetComponent<CardSlot>().CardInSlot == null)
            {
                continue;
            }

            Stack<Vector2Int> relations = boardController.GetAllRelationsFromNode(relationMatrix, numberOfColumns, numberOfRows, nodeToSearch);
            CardSlot cardPrincipal = boardController.CardsSlot[nodeToSearch.x, nodeToSearch.y].transform.GetComponent<CardSlot>();

            while (relations.Count > 0)
            {
                Vector2Int nodeToIncludeInSearch = relations.Pop();
                CardSlot cartRelation = boardController.CardsSlot[nodeToIncludeInSearch.x, nodeToIncludeInSearch.y].transform.GetComponent<CardSlot>();


                int powerToSend = CalculatePower(cardPrincipal,cartRelation);
                boardController.CardsSlot[nodeToIncludeInSearch.x, nodeToIncludeInSearch.y].transform.GetComponent<CardSlot>().AddToPower(powerToSend + cardPrincipal.GetPower(numberOfColumns,numberOfRows), nodeToSearch.x, nodeToSearch.y);

                boardController.CardsSlot[nodeToIncludeInSearch.x, nodeToIncludeInSearch.y].transform.GetComponent<CardSlot>().AttackPower = boardController.CardsSlot[nodeToIncludeInSearch.x, nodeToIncludeInSearch.y].transform.GetComponent<CardSlot>().GetPower(numberOfColumns, numberOfRows);

                boardController.CardsSlot[nodeToIncludeInSearch.x, nodeToIncludeInSearch.y].transform.GetComponent<CardSlot>().SetCardInBoard();

              stackNodesToUpdate.Push(nodeToIncludeInSearch);

            }
        }
      }

    public int CalculatePower( 
        CardSlot cardSlotSelect, CardSlot cardSlotGivingPower)
    {
        if (cardSlotSelect.ColPosition > cardSlotGivingPower.ColPosition) 
        {
            return cardSlotSelect.CardInSlot.damageLeft * cardSlotSelect.Level;


        }
        if (cardSlotSelect.ColPosition < cardSlotGivingPower.ColPosition)
        {

            return cardSlotSelect.CardInSlot.damageRight * cardSlotSelect.Level;

        }

        return   cardSlotSelect.CardInSlot.damageUp * cardSlotSelect.Level;

    }

    public void StartFight(Player actualPlayer)
    {
        Stack<DamageInput> damageStack = new Stack<DamageInput> ();

        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                Vector2Int vectorCardSlot = new Vector2Int(i,j);
                CardSlot cardSlotToSearch = boardController.CardsSlot[vectorCardSlot.y, vectorCardSlot.x].transform.GetComponent<CardSlot>();
                if (cardSlotToSearch.CardInSlot == null) {
                    continue;
                }

                if (cardSlotToSearch.Player != actualPlayer)
                {
                    continue;
                }

                DamageInput damageCheck = CheckForDamage(cardSlotToSearch,numberOfRows,numberOfColumns);
                damageStack.Push(damageCheck);
            }
        }

        while (damageStack.Count > 0) 
        { 
            DamageInput damage = damageStack.Pop();
            if(damage.Damage > 0)
            damage.Player.lifePoints -= damage.Damage;
        }
    }

    public int GetDamage(int originDamage,int defense)
    {
        if(originDamage > defense)
        {
            return originDamage - defense;
        }
        return 0;
    }

// Buscar optimizaci�n
    public DamageInput CheckForDamage(CardSlot cardSlot, int row, int col)
    {
        CardSlot cardObjetiveSlot;

        Card cardObjetive;
        Card cardSelected;

        int CartSelectedDamage;
        int CartObjectivedDamage;

        Player playerSelected;
         

        DamageInput damage = new DamageInput(0, player1);

        if (cardSlot.CardInSlot == null)
        {
            return damage;
        }
        int damageAccumulate = 0;

        playerSelected = cardSlot.Player;
        cardSelected = cardSlot.CardInSlot;

        if (playerSelected.orientation == Orientation.Up)
        {
            if(cardSlot.RowPosition == row - 1)
            {
                damageAccumulate += cardSlot.GetDirectionalDamage(Direction.Up);


            }
            else
            {
                cardObjetiveSlot = boardController.CardsSlot[ cardSlot.ColPosition, cardSlot.RowPosition + 1].transform.GetComponent<CardSlot>();
                if(cardObjetiveSlot.CardInSlot == null || cardObjetiveSlot.Player == playerSelected)
                {
                    
                }
                else { 

                 CartObjectivedDamage = cardObjetiveSlot.GetDirectionalDamage(Direction.Up);
                 CartSelectedDamage = cardSlot.GetDirectionalDamage(Direction.Up);
                 damageAccumulate  += GetDamage(CartSelectedDamage, CartObjectivedDamage);
             }

            }
        }

        if (playerSelected.orientation == Orientation.Down)
            {
                if (cardSlot.RowPosition == 0)
                {
                    damageAccumulate += cardSlot.GetDirectionalDamage(Direction.Up);
                }
                else
                {
                     cardObjetiveSlot = boardController.CardsSlot[cardSlot.ColPosition, cardSlot.RowPosition -1 ].transform.GetComponent<CardSlot>();
                if (cardObjetiveSlot.CardInSlot == null || cardObjetiveSlot.Player == playerSelected)
                {
                    }
                    else
                    {
                    CartObjectivedDamage = cardObjetiveSlot.GetDirectionalDamage(Direction.Up);
                    CartSelectedDamage = cardSlot.GetDirectionalDamage(Direction.Up);
                    damageAccumulate += GetDamage(CartSelectedDamage,CartObjectivedDamage);
                        }
                    }

                }
        

        if ( cardSlot.ColPosition < col-1)
                {
                cardObjetiveSlot = boardController.CardsSlot[cardSlot.ColPosition+1, cardSlot.RowPosition ].transform.GetComponent<CardSlot>();
            if (cardObjetiveSlot.CardInSlot == null || cardObjetiveSlot.Player == playerSelected)
            {

            }

            else
            {
                if (cardSelected.damageRight > 0)
                {
                    CartObjectivedDamage = cardObjetiveSlot.GetDirectionalDamage(Direction.Left);
                    CartSelectedDamage = cardSlot.GetDirectionalDamage(Direction.Right);
                    damageAccumulate += GetDamage(CartSelectedDamage, CartObjectivedDamage);
                }
            }
        }


        if ( cardSlot.ColPosition > 0)
        {
            cardObjetiveSlot = boardController.CardsSlot[cardSlot.ColPosition - 1, cardSlot.RowPosition].transform.GetComponent<CardSlot>();
            if (cardObjetiveSlot.CardInSlot == null || cardObjetiveSlot.Player == playerSelected)
            { }
            else { 
         if (cardSelected.damageLeft > 0)
                {
                    CartObjectivedDamage = cardObjetiveSlot.GetDirectionalDamage(Direction.Right);
                    CartSelectedDamage = cardSlot.GetDirectionalDamage(Direction.Left);
                    damageAccumulate += GetDamage(CartSelectedDamage, CartObjectivedDamage);
            }
            }

        }

        Player playerDamage;
        if (playerSelected == player1)
        {
            playerDamage = player2;
        }
        else
        {
            playerDamage = player1;
        }

        if (damageAccumulate > 0)
        {
            cardSlot.StartFlip();
            damage = new DamageInput(damageAccumulate, playerDamage);
        }
        return damage;
    }


    public bool CheckIfPlayerLose() {
        if(player1.lifePoints <= 0)
        {
            PlayerLose(panelInfoMatch, "Jugador 2 Gana");
            return true;
        }

        if (player2.lifePoints <= 0)
        {
            PlayerLose(panelInfoMatch, "Jugador 1 Gana");
            return true;
        }
        return false;
        }

    private void PlayerLose(GameObject panel, string text)
    {
        panel.SetActive(true);
        textInfoMatch.text =  text;
    }

    IEnumerator ShowNextTurn(string text)
    {
        panelInfoMatch.SetActive(true);
        textInfoMatch.text = text;



        yield return new WaitForSeconds(1);

        panelInfoMatch.SetActive(false);

    }
}
