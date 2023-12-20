using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_MELEE_FIELD,
    SELF_MEDIUM_FIELD,
    SELF_RANGE_FIELD,
    ENEMY_HAND,
    ENEMY_MELEE_FIELD,
    ENEMY_MEDIUM_FIELD,
    ENEMY_RANGE_FIELD
}


public class DropPlace : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;

    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_MELEE_FIELD && Type != FieldType.SELF_RANGE_FIELD && Type != FieldType.SELF_MEDIUM_FIELD)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card && card.gameManager.PlayerFieldCards.Count < 6 &&
          card.gameManager.isPlayerTurn && card.gameManager.playerMana >= card.GetComponent<CardInfo>().selfCard.Manacost)
        {
            card.gameManager.PlayerHandCards.Remove(card.GetComponent<CardInfo>());
            card.gameManager.PlayerFieldCards.Add(card.GetComponent<CardInfo>());
            card.defaultParent = transform;

            // Вызов боевого клича при появлении карты на столе
            card.GetComponent<CardInfo>().TriggerBattlecry();
            card.GetComponent<CardInfo>().ChangeIsInHandState();

            card.gameManager.ReduceMana(true, card.GetComponent<CardInfo>().selfCard.Manacost);
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null ||
            Type == FieldType.ENEMY_MELEE_FIELD ||
              Type == FieldType.ENEMY_RANGE_FIELD ||
              Type == FieldType.ENEMY_MEDIUM_FIELD ||
              Type == FieldType.ENEMY_HAND ||
      Type == FieldType.SELF_HAND)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card)
            card.defaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card && card.defaultTempCardParent == transform)
            card.defaultTempCardParent = card.defaultParent;
    }


}