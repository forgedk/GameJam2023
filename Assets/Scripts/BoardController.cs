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
        CreateCardSlots(4, 4, 0.5f ,0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCardSlots(int XLength,int YLength,float xOffSet,float yOffSet) {
        CardsSlot = new GameObject[XLength,YLength];

        float LengthMin = this.gameObject.GetComponent<RectTransform>().rect.xMin;
        float LengthMax = this.gameObject.GetComponent<RectTransform>().rect.xMax;

        float HeightMin = this.gameObject.GetComponent<RectTransform>().rect.yMin;
        float HeightMax = this.gameObject.GetComponent<RectTransform>().rect.yMax;

        float intervalLenght = (LengthMax - LengthMin) / XLength;
        float intervalHeight = (HeightMax - HeightMin) / YLength;


        for (int i = 0; i < XLength; i++) {
            for (int j = 0; j < YLength; j++) {

                CardsSlot[i,j] = Instantiate(cardSlotTemplate);
                CardsSlot[i, j].GetComponent<RectTransform>().SetParent(this.gameObject.transform);
                CardsSlot[i, j].transform.position = new Vector3(intervalLenght*(i+ xOffSet) , intervalHeight*(j+ yOffSet), 0);

            }
        
        }


    }
}
