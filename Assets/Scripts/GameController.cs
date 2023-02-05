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
    bool[,,,] relationMatrix =  boardController.GetGraphMatrix(col,row);
    bool[,] nodesVisited = new bool[col, row];
    List<Vector2Int> initialNodes = boardController.GetInitialNode(relationMatrix, col, row);
    Stack<Vector2Int> stackinitialNodes = new Stack<Vector2Int>(initialNodes);
        while (stackinitialNodes.Count > 0)
        {
            Vector2Int initialVector = stackinitialNodes.Pop();
            if (boardController.CardsSlot[initialVector.x, initialVector.y].transform.GetComponent<CardSlot>().cardInSlot == null)
            {
                continue;
            }

            Stack<Vector2Int> relations = boardController.GetAllRelationsFromNode(relationMatrix, col, row, initialVector);
            while (relations.Count > 0)
            {
                Vector2Int vectorToInclude = relations.Pop();
                boardController.CardsSlot[vectorToInclude.x, vectorToInclude.y].transform.GetComponent<CardSlot>().AttackPower = 100;
                stackinitialNodes.Push(vectorToInclude);

            }
        }



            }





    public void DeclareWin() {
        print("Jugador 1 Gano");
    
    }



}
