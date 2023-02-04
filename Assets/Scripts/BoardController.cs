using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameObject cardSlotTemplate;
    public GameObject[] CardList;
    public GameObject[,] CardsSlot;
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


        float intervalLenght = this.gameObject.GetComponent<RectTransform>().rect.width / XLength;
        float intervalHeight = this.gameObject.GetComponent<RectTransform>().rect.height / YLength;


        for (int i = 0; i < XLength; i++) {
            for (int j = 0; j < YLength; j++) {

                CardsSlot[i,j] = Instantiate(cardSlotTemplate);
                CardsSlot[i, j].GetComponent<RectTransform>().SetParent(this.gameObject.transform);
                CardsSlot[i, j].transform.localPosition = new Vector3((intervalLenght*(i+ xOffSet)) , intervalHeight*(j+ yOffSet), 0);


            }
        
        }
    }
}
