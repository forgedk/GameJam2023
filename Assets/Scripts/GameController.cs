using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

                int powerToSend = CalculatePower(cardPrincipal.cardInSlot, initialVector, vectorToInclude);
                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().AddToPower(powerToSend + cardPrincipal.GetPower(col,row), initialVector.x, initialVector.y);

                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().AttackPower = boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().GetPower(col, row);

                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().SetCardInBoard();

              stackinitialNodes.Push(vectorToInclude);

            }
        }
      }

    public int CalculatePower( Card card, Vector2Int inital, Vector2Int include)
    {
        if (inital.x > include.x) 
        {
            return card.damageLeft;


        }
        if (inital.x < include.x)
        {

            return card.damageRight;

        }

        return card.damageUp;




    }





    public void DeclareWin() {
        print("Jugador 1 Gano");
    
    }



}
