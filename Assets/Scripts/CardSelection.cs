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


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(Card cardInSystem) 
    {
        cardRepresentation = cardInSystem;
        setImage(cardInSystem.image);
        SetDamageArrows();
    }

    public void SetDamageArrows() 
    {
        SetDamageArrow(cardRepresentation.damageUp, arrowUp);
        SetDamageArrow(cardRepresentation.damageLeft, arrowLeft);
        SetDamageArrow(cardRepresentation.damageRight, arrowRight);
    }

    public void SetDamageArrow(int damage, Arrow arrow)
    {
        if(damage > 0)
        {
            arrow.SetActive(true);
            arrow.setText((damage).ToString());
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

        gameObject.transform.position = initialPosition;
    }
    

}
