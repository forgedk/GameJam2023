using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine;

[Serializable]
public class ChangeCard : UnityEvent<Card> { }

public class CardSelection : MonoBehaviour, IDragHandler , IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ChangeCard ChangePanel;

    private Vector3 initialPosition;
    private UnityEngine.UI.Image imageCard;
    public Arrow arrowUp, arrowDown, arrowLeft, arrowRight;
    public Card cardRepresentation;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gameObject.transform.position;


    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ChangePanel.Invoke(cardRepresentation);
        imageCard.raycastTarget = false;



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(Card cardInSystem, Player player) 
    {
        cardRepresentation = cardInSystem;
        setImage(cardInSystem.image);
        SetDamageArrows(player);
    }

    public void SetDamageArrows(Player player) 
    {
        if (player.orientation == Orientation.Up) {
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
        if(damage > 0)
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

    public void setImage(Sprite texture)
    {
        imageCard = GetComponent<UnityEngine.UI.Image>();
        imageCard.sprite = texture;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        imageCard.raycastTarget = true;
        gameObject.transform.position = initialPosition;
    }
    

}
