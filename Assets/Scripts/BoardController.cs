using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Up, Down, Left, Right
}
public class BoardController : MonoBehaviour
{
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


}
