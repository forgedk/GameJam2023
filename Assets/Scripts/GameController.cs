using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    List<Vector2Int> initialNodes = boardController.GetInitialNode(relationMatrix, col, row);
 

    }



    public void DeclareWin() {
        print("Jugador 1 Gano");
    
    }



}
