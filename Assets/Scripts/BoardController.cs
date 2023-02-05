using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum Direction
{
    Up, Down, Left, Right
}
public class BoardController : MonoBehaviour
{

    public  GameController gameController;

    public GameObject cardSlotTemplate;
    public GameObject[,] CardsSlot;
    public int numberRow, numberCol;
    // Start is called before the first frame update
    void Start()
    {
// Create a 4X4 Board
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCardSlots(int XLength,int YLength,float xOffSet,float yOffSet) {
        CardsSlot = new GameObject[XLength,YLength];

        numberRow =  XLength;
        numberCol = YLength;

        float intervalLenght = this.gameObject.GetComponent<RectTransform>().rect.width / XLength;
        float intervalHeight = this.gameObject.GetComponent<RectTransform>().rect.height / YLength;


        for (int i = 0; i < XLength; i++) {
            for (int j = 0; j < YLength; j++) {

                CardsSlot[i,j] = Instantiate(cardSlotTemplate);
                CardsSlot[i, j].GetComponent<RectTransform>().SetParent(this.gameObject.transform);
                CardsSlot[i, j].transform.localPosition = new Vector3((intervalLenght*(i+ xOffSet)) , intervalHeight*(j+ yOffSet), 0);

                CardsSlot[i, j].transform.GetComponent<CardSlot>().row = j;
                CardsSlot[i, j].transform.GetComponent<CardSlot>().col = i;
                CardsSlot[i, j].transform.GetComponent<CardSlot>().boardController = this;

                CardsSlot[i, j].transform.GetComponent<CardSlot>().endTurn.AddListener(gameController.EndTurn);

            }
        
        }
    }

    public bool CheckValidMove(int row,int column,Card card,Player player)
    {
        if (CardsSlot[column, row].transform.GetComponent<CardSlot>().cardInSlot != null)
        {
            if(CardsSlot[column, row].transform.GetComponent<CardSlot>().player == player) { return true; }
            return false;
        }

        if(player.orientation == Orientation.Up && row == 0) {
            return true;
        }

        if (player.orientation == Orientation.Down && row == (numberRow -1))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row - 1,column,card,player,Direction.Up))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row + 1, column, card, player, Direction.Down))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row , column-1, card, player, Direction.Right))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row, column + 1, card, player, Direction.Left))
        {
            return true;
        }
        return false;
    }

    public bool TestIfAdjacentToAlly(int row, int column, Card card, Player player, Direction dir)
    {
        if (column >= numberCol | column <0) {
            return false; 
        }
  
        if (row >= numberRow | row < 0) {
            return false; 
        }

        if (CardsSlot[column, row].transform.GetComponent<CardSlot>().cardInSlot == null)
        {
            return false;
        }

        if (CardsSlot[column, row].transform.GetComponent<CardSlot>().player != player) {
            return false;
        }

        if(dir == Direction.Up)
        {
            if(CardsSlot[column, row].transform.GetComponent<CardSlot>().player.orientation == Orientation.Up || CardsSlot[column, row].transform.GetComponent<CardSlot>().cardInSlot.damageUp > 0)
            {
                return true;

            }
        }

        if (dir == Direction.Down)
        {
            if (CardsSlot[column, row].transform.GetComponent<CardSlot>().player.orientation == Orientation.Down || CardsSlot[column, row].transform.GetComponent<CardSlot>().cardInSlot.damageUp > 0)
            {
                return true;

            }
        }

        if (dir == Direction.Left)
        {
            if ( CardsSlot[column, row].transform.GetComponent<CardSlot>().cardInSlot.damageLeft > 0)
            {
                return true;

            }
        }

        if (dir == Direction.Right)
        {
            if (CardsSlot[column, row].transform.GetComponent<CardSlot>().cardInSlot.damageRight > 0)
            {
                return true;

            }
        }

        return false;
    }

    public bool[,,,] GetGraphMatrix(int XLength, int YLength) {
        bool[,,,] graphRelations =  new bool[XLength, YLength, XLength, YLength];
        for (int i = 0; i < XLength; i++)
        {
            for (int j = 0; j < YLength; j++)
            {
                for (int a = 0; a < XLength; a++)
                {
                    for (int b = 0; b < YLength; b++)
                    {

                        {
                            {
                                graphRelations[i, j, a, b] = TestIfRelationExist(i,j,a,b);

                            }
                        }
                    }
                }
            }
        }

                        return graphRelations;
    }

    public bool TestIfRelationExist(int rowSelector, int columnSelector, int rowToInspect, int columnToInspect) {
        if (CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().cardInSlot == null)
        {
            return false;
        }

        if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().cardInSlot == null)
        {
            return false;
        }

        if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().player != CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().player)
        {
            return false;
        }
        // Sprint(String.Format("Objeto de buscada Columna : {0},Fila : {1}",columnSelector,rowSelector));
        // Sprint(String.Format("Objeto de inteacción Columna : {0},Fila : {1}", columnToInspect, rowToInspect));

        Player player = CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().player;

        if (columnSelector == columnToInspect) {
            if (Math.Abs(rowSelector - rowToInspect) == 1) {
                if (rowSelector > rowToInspect && player.orientation == Orientation.Down)  {
                    return true;
                }
                if (rowSelector < rowToInspect && player.orientation == Orientation.Up) 
                { 
                    return true;
                }
            }
        }

        if (rowSelector == rowToInspect)
        {
            if (Math.Abs(columnSelector - columnToInspect) == 1)
            {
                if (columnSelector < columnToInspect)
                {
                    if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().cardInSlot.damageRight > 0 && CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().cardInSlot.damageLeft == 0)
                    {
                        return true;

                    }

                }

                if (columnSelector > columnToInspect)
                {
                    if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().cardInSlot.damageLeft > 0 && CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().cardInSlot.damageRight == 0)
                    {
                        return true;

                    }

                }

            }
        }



        return false;

    
    }




}
