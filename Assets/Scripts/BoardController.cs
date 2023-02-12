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

    public GameController gameController;

    public GameObject cardSlotTemplate;
    public GameObject[,] CardsSlot;
    public int numberRow, numberCol;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetCardPower(int XLength, int YLength)
    {
        for (int i = 0; i < XLength; i++)
        {
            for (int j = 0; j < YLength; j++)
            {
                CardsSlot[i, j].GetComponent<CardSlot>().ResetPowerMatrix(XLength,YLength);

            }

        }
    }

    public void CreateCardSlots(int XLength, int YLength, float xOffSet, float yOffSet)
    {
        CardsSlot = new GameObject[XLength, YLength];

        numberRow = XLength;
        numberCol = YLength;

        float intervalLenght = this.gameObject.GetComponent<RectTransform>().rect.width / XLength;
        float intervalHeight = this.gameObject.GetComponent<RectTransform>().rect.height / YLength;


        for (int i = 0; i < XLength; i++)
        {
            for (int j = 0; j < YLength; j++)
            {

                CardsSlot[i, j] = Instantiate(cardSlotTemplate);
                CardsSlot[i, j].GetComponent<RectTransform>().SetParent(this.gameObject.transform);
                CardsSlot[i, j].transform.localPosition = new Vector3((intervalLenght * (i + xOffSet)), intervalHeight * (j + yOffSet), 0);

                CardsSlot[i, j].transform.GetComponent<CardSlot>().RowPosition = j;
                CardsSlot[i, j].transform.GetComponent<CardSlot>().ColPosition = i;
                CardsSlot[i, j].transform.GetComponent<CardSlot>().BoardController = this;

                CardsSlot[i, j].transform.GetComponent<CardSlot>().endTurn.AddListener(gameController.EndTurn);

            }

        }
    }

    public bool CheckValidMove(int row, int column, Card card, Player player)
    {
        if (CardsSlot[column, row].transform.GetComponent<CardSlot>().CardInSlot != null)
        {
            if (CardsSlot[column, row].transform.GetComponent<CardSlot>().Player == player) { return true; }
            return false;
        }

        if (player.orientation == Orientation.Up && row == 0)
        {
            return true;
        }

        if (player.orientation == Orientation.Down && row == (numberRow - 1))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row - 1, column, player, Direction.Up))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row + 1, column, player, Direction.Down))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row, column - 1, player, Direction.Right))
        {
            return true;
        }

        if (TestIfAdjacentToAlly(row, column + 1, player, Direction.Left))
        {
            return true;
        }
        return false;
    }

    public bool TestIfAdjacentToAlly(int rowAllyPosition, int columnAllyPosition, Player player, Direction dir)
    {
        if (columnAllyPosition >= numberCol | columnAllyPosition < 0)
        {
            return false;
        }

        if (rowAllyPosition >= numberRow | rowAllyPosition < 0)
        {
            return false;
        }

        if (CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().CardInSlot == null)
        {
            return false;
        }

        if (CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().Player != player)
        {
            return false;
        }

        if (dir == Direction.Up)
        {
            if (CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().Player.orientation == Orientation.Up || CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().CardInSlot.damageUp > 0)
            {
                return true;

            }
        }

        if (dir == Direction.Down)
        {
            if (CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().Player.orientation == Orientation.Down || CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().CardInSlot.damageUp > 0)
            {
                return true;

            }
        }

        if (dir == Direction.Left)
        {
            if (CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().CardInSlot.damageLeft > 0)
            {
                return true;

            }
        }

        if (dir == Direction.Right)
        {
            if (CardsSlot[columnAllyPosition, rowAllyPosition].transform.GetComponent<CardSlot>().CardInSlot.damageRight > 0)
            {
                return true;

            }
        }

        return false;
    }

    public bool[,,,] GetGraphMatrix(int XLength, int YLength)
    {
        bool[,,,] graphRelations = new bool[XLength, YLength, XLength, YLength];
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
                                graphRelations[i, j, a, b] = TestIfRelationExist(i, j, a, b);

                            }
                        }
                    }
                }
            }
        }

        return graphRelations;
    }

    public List<Vector2Int> GetInitialNode(bool[,,,] graphRelations, int XLength, int YLength)
    {
        List<Vector2Int> vectorNodes = new List<Vector2Int>();

        for (int i = 0; i < XLength; i++)
        {
            for (int j = 0; j < YLength; j++)
            {
                int numberOfRelatiosWithOtherCards = 0;
                for (int a = 0; a < XLength; a++)
                {
                    for (int b = 0; b < YLength; b++)
                    {
                        if (graphRelations[a, b, i, j])
                        {
                            numberOfRelatiosWithOtherCards++;

                        }

                    }
                }
                if (numberOfRelatiosWithOtherCards == 0)
                {
                    vectorNodes.Add(new Vector2Int(i, j));

                }
            }
        }

        return vectorNodes;
    }

    public bool TestIfRelationExist(int columnSelector, int rowSelector, int columnToInspect, int rowToInspect)
    {
        if (CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().CardInSlot == null)
        {
            return false;
        }

        if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().CardInSlot == null)
        {
            return false;
        }

        if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().Player != CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().Player)
        {
            return false;
        }

        Player player = CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().Player;

        if (columnSelector == columnToInspect)
        {
            if (Math.Abs(rowSelector - rowToInspect) == 1)
            {
                if (rowSelector > rowToInspect && player.orientation == Orientation.Down)
                {
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
                    if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().CardInSlot.damageRight > 0 && CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().CardInSlot.damageLeft == 0)
                    {
                        return true;

                    }

                }

                if (columnSelector > columnToInspect)
                {
                    if (CardsSlot[columnSelector, rowSelector].transform.GetComponent<CardSlot>().CardInSlot.damageLeft > 0 && CardsSlot[columnToInspect, rowToInspect].transform.GetComponent<CardSlot>().CardInSlot.damageRight == 0)
                    {
                        return true;

                    }

                }

            }
        }



        return false;


    }

    public Stack<Vector2Int> GetAllRelationsFromNode(bool[,,,] graphRelations, int XLength, int YLength, Vector2Int relation)
    {
        Stack<Vector2Int> result = new Stack<Vector2Int>();
        for (int i = 0; i < XLength; i++)
        {
            for (int j = 0; j < YLength; j++)
            {
                if (graphRelations[relation.x, relation.y, i, j]) {
                    result.Push(new Vector2Int(i, j));
                }
            }
        }
        return result;
    }
}
