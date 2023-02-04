using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BoardController boardController;
    public DrawPanel drawController;
    public Player player1, player2;
    // Start is called before the first frame update
    void Start()
    {
        boardController.CreateCardSlots(4,4, 0.5f, 0.5f);
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

    public void DeclareWin() {
        print("Jugador 1 Gano");
    
    }



}
