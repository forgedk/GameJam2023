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

    public void SetCards(Player player)
    {
        List<int> indexList = new List<int>();

        foreach (CardSelection cardToSelect in cardSelectList)
        {
            int idSelectd = Random.Range(0, cardList.Length);
            while (indexList.Contains(idSelectd))
                {
                 idSelectd = Random.Range(0, cardList.Length);

            }
            cardToSelect.SetCard(cardList[idSelectd],player);
            indexList.Add(idSelectd);
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
