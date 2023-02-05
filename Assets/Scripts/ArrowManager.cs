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

    public void SetDamageArrows(Card cardRepresentation, Player player)
    {
        if (player.orientation == Orientation.Up)
        {
            SetDamageArrow(cardRepresentation.damageUp, arrowUp, player.playerColor);
            SetDamageArrow(0, arrowDown, player.playerColor);
        }
        else
        {
            SetDamageArrow(cardRepresentation.damageUp, arrowDown, player.playerColor);
            SetDamageArrow(0, arrowUp, player.playerColor);

        }
        SetDamageArrow(cardRepresentation.damageLeft, arrowLeft, player.playerColor);
        SetDamageArrow(cardRepresentation.damageRight, arrowRight, player.playerColor);
    }

    public void SetDamageArrow(int damage, Arrow arrow, Color color)
    {
        if (damage > 0)
        {
            arrow.SetActive(true);
            arrow.setText((damage).ToString());
            arrow.setColor(color);
        }
        else
        {
            arrow.SetActive(false);
        }

    }
}