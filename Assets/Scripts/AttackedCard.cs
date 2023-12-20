using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		if (!GetComponent<CardMovement>().gameManager.isPlayerTurn)
			return;

		CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>();
		
		if(card && card.selfCard.CanAttack && 
			(transform.parent == GetComponent<CardMovement>().gameManager.enemyMeleeField ||
            transform.parent == GetComponent<CardMovement>().gameManager.enemyRangeField ||
			transform.parent == GetComponent<CardMovement>().gameManager.enemyMediumField))
        {
			card.selfCard.ChangeAttackState(false);

			if(card.isPlayer)
			{
				card.DeHighliteCard();
			}

			GetComponent<CardMovement>().gameManager.CardsFight(card, GetComponent<CardInfo>());
		}
	}
}
