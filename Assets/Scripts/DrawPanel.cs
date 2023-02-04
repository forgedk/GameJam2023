using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPanel : MonoBehaviour
{
    public Card[] cardList;
    public CardSelection[] cardSelectList;  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCards()
    {
        foreach (CardSelection cardToSelect in cardSelectList)
        {
            cardToSelect.SetCard(cardList[0]);

        }
    }
}
