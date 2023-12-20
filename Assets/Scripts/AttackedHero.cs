using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedHero : MonoBehaviour, IDropHandler
{
	public enum HeroType
	{
		ENEMY,
		PLAYER
	}

	public HeroType type;
	public GameManager gameManager;	

	public void OnDrop(PointerEventData eventData)
	{
		if(!gameManager.isPlayerTurn)
			return;

		CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>();

		if(card && card.selfCard.CanAttack && type == HeroType.ENEMY)
		{
			card.selfCard.CanAttack = false;

			gameManager.DamageHero(card, true);
		}
	}
}
