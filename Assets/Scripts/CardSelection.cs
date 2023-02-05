using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[Serializable]
public class ChangeCard : UnityEvent<Card> { }

public class CardSelection : MonoBehaviour, IDragHandler , IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ChangeCard ChangePanel;

    private Vector3 initialPosition;
    private UnityEngine.UI.Image imageCard;
    public Card cardRepresentation;
    public ArrowManager arrowManager;
    public Player ownPlayer;


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
        cardRepresentation.playVoice();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(Card cardInSystem, Player player) 
    {
        cardRepresentation = cardInSystem;
        setImage(cardInSystem.image);
        ownPlayer = player;
        arrowManager.SetDamageArrows(cardRepresentation,1,0, player);
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
