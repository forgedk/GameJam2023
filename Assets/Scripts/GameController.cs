using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct DamageInput
{
    public DamageInput(int damage, Player player)
    {
        Damage= damage;
        Player= player;


    }
    public int Damage { get; }
    public Player Player { get; }

}

public class GameController : MonoBehaviour
{
    public int row;
    public int col;

    public BoardController boardController;
    public DrawPanel drawController;
    public Player player1, player2;
    // Start is called before the first frame update
    void Start()
    {
        boardController.CreateCardSlots(col, row, 0.5f, 0.5f);
        SetTurn(player1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTurn(Player player)
    {
        drawController.SetActive(true);
        drawController.SetCards(player);
    }

    public void EndTurn(Player player) {
        UpdateDamageCards(player);
        StartFight();
        CheckIfPlayerLose();
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
    boardController.ResetCardPower(col, row);

    bool[,,,] relationMatrix =  boardController.GetGraphMatrix(col,row);
    bool[,] nodesVisited = new bool[col, row];

    List<Vector2Int> initialNodes = boardController.GetInitialNode(relationMatrix, col, row);
    Stack<Vector2Int> stackinitialNodes = new Stack<Vector2Int>(initialNodes);
        while (stackinitialNodes.Count > 0)
        {
          
            Vector2Int initialVector = stackinitialNodes.Pop();
                nodesVisited[initialVector.x,initialVector.y] = true;
            if (boardController.CardsSlot[initialVector.x, initialVector.y].transform.GetComponent<CardSlot>().cardInSlot == null)
            {
                continue;
            }

            Stack<Vector2Int> relations = boardController.GetAllRelationsFromNode(relationMatrix, col, row, initialVector);
            while (relations.Count > 0)
            {
                Vector2Int vectorToInclude = relations.Pop();
                CardSlot cardPrincipal = boardController.CardsSlot[initialVector.x, initialVector.y].transform.GetComponent<CardSlot>();

                int powerToSend = CalculatePower(cardPrincipal.cardInSlot, cardPrincipal.level, initialVector, vectorToInclude);
                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().AddToPower(powerToSend + cardPrincipal.GetPower(col,row), initialVector.x, initialVector.y);

                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().AttackPower = boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().GetPower(col, row);

                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().SetCardInBoard();

              stackinitialNodes.Push(vectorToInclude);

            }
        }
      }

    public int CalculatePower( Card card,int level, Vector2Int inital, Vector2Int include)
    {
        if (inital.x > include.x) 
        {
            return card.damageLeft* level;


        }
        if (inital.x < include.x)
        {

            return card.damageRight * level;

        }

        return card.damageUp * level;




    }

    public void StartFight()
    {
        Stack<DamageInput> damageStack = new Stack<DamageInput> ();

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Vector2Int vectorToSearch = new Vector2Int(i,j);
                DamageInput damageCheck = CheckForDamage(vectorToSearch,row,col);
                print(damageCheck.Damage);
                damageStack.Push(damageCheck);

            }
        }
    }

    public int  GetDirectionalDamage(CardSlot cardSlot, int damage)
    {
        return cardSlot.AttackPower + (damage * cardSlot.level);

    }

    public int GetDamage(int originDamage,int defense)
    {
        if(originDamage > defense)
        {
            return originDamage - defense;
        }
        return 0;
    }


    public DamageInput CheckForDamage(Vector2Int vectorToSearch,int row, int col)
    {
        CardSlot cardSlot = boardController.CardsSlot[vectorToSearch.y, vectorToSearch.x].transform.GetComponent<CardSlot>();
        CardSlot cardObjetiveSlot;

        Card cardObjetive;
        Card cardSelected;

        int CartSelectedDamage;
        int CartObjectivedDamage;

        Player playerSelected;
         



        DamageInput damage = new DamageInput(0, player1);

        if (cardSlot.cardInSlot == null)
        {
            return damage;
        }
        int damageAccumulate = 0;

        playerSelected = cardSlot.player;
        cardSelected = cardSlot.cardInSlot;

        if (playerSelected.orientation == Orientation.Up)
        {
            if(cardSlot.row == row - 1)
            {
                damageAccumulate = damageAccumulate + GetDirectionalDamage(cardSlot, cardSelected.damageUp);
            }
            else
            {
                cardObjetiveSlot = boardController.CardsSlot[ cardSlot.col, cardSlot.row + 1].transform.GetComponent<CardSlot>();
                if(cardObjetiveSlot.cardInSlot == null || cardObjetiveSlot.player == playerSelected)
                {
                    
                }
                else { 

                 cardObjetive= cardObjetiveSlot.cardInSlot;
                 CartObjectivedDamage = GetDirectionalDamage(cardObjetiveSlot, cardObjetive.damageUp);
                 CartSelectedDamage = GetDirectionalDamage(cardSlot, cardSelected.damageUp);
                    damageAccumulate  = damageAccumulate + GetDamage(CartSelectedDamage, CartObjectivedDamage);
             }

            }
        }

        if (playerSelected.orientation == Orientation.Down)
            {
                if (cardSlot.row == 0)
                {
                    damageAccumulate = damageAccumulate + GetDirectionalDamage(cardSlot, cardSelected.damageUp);
                }
                else
                {
                     cardObjetiveSlot = boardController.CardsSlot[cardSlot.col, cardSlot.row -1 ].transform.GetComponent<CardSlot>();
                if (cardObjetiveSlot.cardInSlot == null || cardObjetiveSlot.player == playerSelected)
                {
                    }
                    else
                    {

                         cardObjetive = cardObjetiveSlot.cardInSlot;
                         CartObjectivedDamage = GetDirectionalDamage(cardObjetiveSlot, cardObjetive.damageUp);
                         CartSelectedDamage = GetDirectionalDamage(cardSlot, cardSelected.damageUp);
                        damageAccumulate = damageAccumulate + GetDamage(CartSelectedDamage,CartObjectivedDamage);
                        }
                    }

                }
        

        if ( cardSlot.col < col-1)
                {
                cardObjetiveSlot = boardController.CardsSlot[cardSlot.col+1, cardSlot.row ].transform.GetComponent<CardSlot>();
            if (cardObjetiveSlot.cardInSlot == null || cardObjetiveSlot.player == playerSelected)
            {

            }

            else
            {
                if (cardSelected.damageRight > 0)
                {
                    cardObjetive = cardObjetiveSlot.cardInSlot;
                    CartObjectivedDamage = GetDirectionalDamage(cardObjetiveSlot, cardObjetive.damageLeft);
                    CartSelectedDamage = GetDirectionalDamage(cardSlot, cardSelected.damageRight);
                    damageAccumulate = damageAccumulate + GetDamage(CartSelectedDamage, CartObjectivedDamage);
                }
            }
        }


        if ( cardSlot.col > 0)
        {
            cardObjetiveSlot = boardController.CardsSlot[cardSlot.col - 1, cardSlot.row].transform.GetComponent<CardSlot>();
            if (cardObjetiveSlot.cardInSlot == null || cardObjetiveSlot.player == playerSelected)
            { }
            else { 
         if (cardSelected.damageLeft > 0)
                {
                    cardObjetive = cardObjetiveSlot.cardInSlot;
                    CartObjectivedDamage = GetDirectionalDamage(cardObjetiveSlot, cardObjetive.damageRight);
                    CartSelectedDamage = GetDirectionalDamage(cardSlot, cardSelected.damageLeft);
                    damageAccumulate = damageAccumulate + GetDamage(CartSelectedDamage, CartObjectivedDamage);
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
        damage = new DamageInput(damageAccumulate, playerDamage);
        return damage;
    }


    public void CheckIfPlayerLose() {
        if(player1.lifePoints <= 0)
            {
                print("Player 2 win");
            }

            if (player2.lifePoints <= 0)
            {
                print("Player 1 win");
            }
        }
}
