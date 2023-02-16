using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public Arrow arrowUp, arrowDown, arrowLeft, arrowRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamageArrows(Card cardRepresentation,int level,int addedPower, Player player)
    {
        if (player.orientation == Orientation.Up)
        {
            SetDamageArrow(cardRepresentation.damageUp * level, addedPower,arrowUp, player.playerColor);
            SetDamageArrow(0, addedPower ,arrowDown, player.playerColor);
        }
        else
        {
            SetDamageArrow(cardRepresentation.damageUp * level, addedPower,arrowDown, player.playerColor);
            SetDamageArrow(0, addedPower, arrowUp , player.playerColor);

        }
        SetDamageArrow(cardRepresentation.damageLeft * level, addedPower, arrowLeft, player.playerColor);
        SetDamageArrow(cardRepresentation.damageRight * level, addedPower, arrowRight, player.playerColor);
    }

    public void HideArrows() {
        arrowUp.SetActive(false);
        arrowDown.SetActive(false);
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
    }

    public void SetDamageArrow(int damage,int power, Arrow arrow, Color color)
    {
        if (damage > 0)
        {
            arrow.SetActive(true);
            arrow.setText((damage+ power).ToString());
            arrow.setColor(color);
        }
        else
        {
            arrow.SetActive(false);
        }

    }
}
