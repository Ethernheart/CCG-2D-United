using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game
{
	public List<Card> PlayerDeck, EnemyDeck;

	public Game()
	{
		PlayerDeck = GiveMeDeckCard();
		EnemyDeck = GiveEnemyDeckCard();
	}

	List<Card> GiveMeDeckCard()
	{
		List<Card> list = new List<Card>();
		for (int i = 0; i < CardList.allPlayerCards.Count; i++)
			list.Add(CardList.allPlayerCards[i]);
		Shuffle(list);
		return list;
	}

	List<Card> GiveEnemyDeckCard()
	{
		List<Card> list = new List<Card>();
		for (int i = 0; i < CardList.allEnemyCards.Count; i++)
			list.Add(CardList.allEnemyCards[i]);
		Shuffle(list);
		return list;
	}

	void Shuffle<T>(List<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0, n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}

public class GameManager : MonoBehaviour
{
	public Transform enemyMeleeField;
	public Transform enemyRangeField;
	public Transform enemyMediumField;
	public Transform selfRangeField;

	public int playerMana = 10, enemyMana = 10;
	public TextMeshProUGUI playerManaTxt, enemyManaTxt;
	public TextMeshProUGUI playerHPTxt, enemyHPTxt;

	public int playerHP, enemyHP;

	public GameObject resultGO;
	public TextMeshProUGUI resultTxt;

	[SerializeField] private Game _currentGame;
	[SerializeField] private Transform _enemyHand;
	[SerializeField] private Transform _playerHand;
	[SerializeField] private GameObject _cardPref;
	[SerializeField] private TextMeshProUGUI _turnTimeText;
	[SerializeField] private Button _endTurnBtn;

	public List<CardInfo> PlayerHandCards = new List<CardInfo>(),
		PlayerFieldCards = new List<CardInfo>(),
		EnemyHandCards = new List<CardInfo>(),
		EnemyFieldCards = new List<CardInfo>();

	private int _turn = 0;
	private int _turnTime = 30;

	public bool isPlayerTurn
	{
		get => _turn % 2 == 0;
	}

	private void Start()
	{
		_currentGame = new Game();

		GiveHandCards(_currentGame.EnemyDeck, _enemyHand);
		GiveHandCards(_currentGame.PlayerDeck, _playerHand);

		playerHP = enemyHP = 30;

		ShowMana();

		StartCoroutine(TurnFunc());
	}

	private void GiveHandCards(List<Card> deck, Transform hand)
	{
		int i = 0;
		while (i++ < 4)
			GiveCardToHand(deck, hand);
	}

	private void GiveCardToHand(List<Card> deck, Transform hand)
	{
		if (deck.Count == 0)
			return;

		Card card = deck[0];

		GameObject CardGO = Instantiate(_cardPref, hand, false);

		if (hand == _enemyHand)
		{
			CardGO.GetComponent<CardInfo>().HideCardInfo(card);
			EnemyHandCards.Add(CardGO.GetComponent<CardInfo>());
		}
		else
		{
			CardGO.GetComponent<CardInfo>().ShowCardInfo(card, true);
			PlayerHandCards.Add(CardGO.GetComponent<CardInfo>());
			CardGO.GetComponent<AttackedCard>().enabled = false;
		}

		deck.RemoveAt(0);
	}

	private void GiveNewCards()
	{
		GiveCardToHand(_currentGame.EnemyDeck, _enemyHand);
		GiveCardToHand(_currentGame.PlayerDeck, _playerHand);
	}

	IEnumerator TurnFunc()
	{
		_turnTime = 30;
		_turnTimeText.text = _turnTime.ToString();

		foreach (var card in PlayerFieldCards)
			card.DeHighliteCard();

		if (isPlayerTurn)
		{
			foreach (var card in PlayerFieldCards)
			{
				card.selfCard.ChangeAttackState(true);
				card.HighliteCard();
			}

			while (_turnTime-- > 0)
			{
				_turnTimeText.text = _turnTime.ToString();
				yield return new WaitForSeconds(1);
			}
		}
		else
		{
			foreach (var card in EnemyFieldCards)
				card.selfCard.ChangeAttackState(true);

			while (_turnTime-- > 27)
			{
				_turnTimeText.text = _turnTime.ToString();
				yield return new WaitForSeconds(1);
			}

			if (EnemyHandCards.Count > 0)
				EnemyTurn(EnemyHandCards);
		}

		ChangeTurn();
	}

	private void EnemyTurn(List<CardInfo> cards)
	{
		int count = Random.Range(1, cards.Count);

		for (int i = 0; i < count; i++)
		{
			if (EnemyFieldCards.Count > 5 || enemyMana == 0)
				return;

			List<CardInfo> cardsList = cards.FindAll(x => enemyMana >= x.selfCard.Manacost);

			if (cardsList.Count == 0)
				break;
			int typeField = Random.Range(0, 3);
			if (enemyRangeField.childCount <= 1)
			{
				typeField = 1;
			}


			if (typeField == 0)
			{
				cardsList[0].ShowCardInfo(cardsList[0].selfCard, false);
				cardsList[0].transform.SetParent(enemyMeleeField);

				EnemyFieldCards.Add(cardsList[0]);
				EnemyHandCards.Remove(cardsList[0]);
			}
			else if (typeField == 1)
			{
				cardsList[0].ShowCardInfo(cardsList[0].selfCard, false);
				cardsList[0].transform.SetParent(enemyRangeField);

				EnemyFieldCards.Add(cardsList[0]);
				EnemyHandCards.Remove(cardsList[0]);
			}
			else if (typeField == 2)
			{
				cardsList[0].ShowCardInfo(cardsList[0].selfCard, false);
				cardsList[0].transform.SetParent(enemyMediumField);

				EnemyFieldCards.Add(cardsList[0]);
				EnemyHandCards.Remove(cardsList[0]);
			}
		}


		foreach (var activeCard in EnemyFieldCards.FindAll(x => x.selfCard.CanAttack))
		{
			for (activeCard.selfCard.attack = 0; activeCard.selfCard.attack < 1; activeCard.selfCard.attack++)
			{
				if (selfRangeField.childCount != 0)
				{
					List<Transform> enemyTaunts = new List<Transform>();
					for (int i = 0; i < selfRangeField.childCount; i++)
					{
						enemyTaunts.Add(selfRangeField.GetChild(i));
					}

					var attackedTaunt = enemyTaunts[Random.Range(0, enemyTaunts.Count)].GetComponent<CardInfo>();
					CardsFight(attackedTaunt, activeCard);
				}
				else
				{
					if (PlayerFieldCards.Count != 0)
					{
						var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

						Debug.Log(activeCard.selfCard.Name + " (" + activeCard.selfCard.Attack + ";" +
								  activeCard.selfCard.HP + ") ===> " + enemy.selfCard.Name +
								  " (" + enemy.selfCard.Attack + ";" + enemy.selfCard.HP + ")");

						activeCard.selfCard.ChangeAttackState(false);
						CardsFight(enemy, activeCard);
					}
					else
					{
						Debug.Log(activeCard.selfCard.Name + " (" + activeCard.selfCard.Attack + ") Attacked hero");

						activeCard.selfCard.ChangeAttackState(true);
						DamageHero(activeCard, false);
					}
				}
			}

			if (selfRangeField.childCount != 0)
			{
				List<Transform> enemyTaunts = new List<Transform>();
				for (int i = 0; i < selfRangeField.childCount; i++)
				{
					enemyTaunts.Add(selfRangeField.GetChild(i));
				}

				var attackedTaunt = enemyTaunts[Random.Range(0, enemyTaunts.Count)].GetComponent<CardInfo>();
				CardsFight(attackedTaunt, activeCard);
			}
			else
			{
				if (PlayerFieldCards.Count != 0)
				{
					var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

					Debug.Log(activeCard.selfCard.Name + " (" + activeCard.selfCard.Attack + ";" +
							  activeCard.selfCard.HP + ") ===> " + enemy.selfCard.Name +
							  " (" + enemy.selfCard.Attack + ";" + enemy.selfCard.HP + ")");

					activeCard.selfCard.ChangeAttackState(false);
					CardsFight(enemy, activeCard);
				}
				else
				{
					Debug.Log(activeCard.selfCard.Name + " (" + activeCard.selfCard.Attack + ") Attacked hero");

					activeCard.selfCard.ChangeAttackState(false);
					DamageHero(activeCard, false);
				}

				activeCard.selfCard.attack = 0;
			}
		}
	}

	public void ChangeTurn()
	{
		StopAllCoroutines();

		_turn++;

		_endTurnBtn.interactable = isPlayerTurn;

		if (isPlayerTurn)
		{
			GiveNewCards();

			playerMana = enemyMana = 10;
			ShowMana();
		}

		StartCoroutine(TurnFunc());
	}

	public void CardsFight(CardInfo playerCard, CardInfo enemyCard)
	{
		List<Transform> taunts = new List<Transform>();
		if (isPlayerTurn)
		{
			taunts.Clear();
			for (int i = 0; i < enemyRangeField.childCount; i++)
			{
				taunts.Add(enemyRangeField.GetChild(i));
			}
		}
		else
		{
			taunts.Clear();
			for (int i = 0; i < selfRangeField.childCount; i++)
			{
				taunts.Add(selfRangeField.GetChild(i));
			}
		}


		if (taunts.Count != 0)
		{
			if (taunts.Contains(enemyCard.transform))
			{
				if (playerCard.selfCard.Windfury && playerCard.selfCard.attack < 1)
				{
					playerCard.selfCard.attack++;
					Debug.Log($"{playerCard.selfCard.attack} aaaaaaaaaaaaaaAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaa");
					playerCard.selfCard.GetDamage(enemyCard.selfCard.Attack <= playerCard.selfCard.Protection
						? 0
						: enemyCard.selfCard.Attack - playerCard.selfCard.Protection);
					enemyCard.selfCard.GetDamage(playerCard.selfCard.Attack <= enemyCard.selfCard.Protection
						? 0
						: playerCard.selfCard.Attack - enemyCard.selfCard.Protection);
					playerCard.HighliteCard();
					playerCard.selfCard.ChangeAttackState(true);
				}

				else
				{
					playerCard.selfCard.GetDamage(enemyCard.selfCard.Attack <= playerCard.selfCard.Protection
						? 0
						: enemyCard.selfCard.Attack - playerCard.selfCard.Protection);
					enemyCard.selfCard.GetDamage(playerCard.selfCard.Attack <= enemyCard.selfCard.Protection
						? 0
						: playerCard.selfCard.Attack - enemyCard.selfCard.Protection);
					playerCard.selfCard.attack = 0;
				}
			}
			else
			{
				Debug.Log("You can't attack this card AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
				playerCard.HighliteCard();
				playerCard.selfCard.ChangeAttackState(true);
			}
		}
		else
		{
			if (playerCard.selfCard.Windfury && playerCard.selfCard.attack < 1)
			{
				playerCard.selfCard.attack++;
				Debug.Log($"{playerCard.selfCard.attack} aaaaaaaaaaaaaaAAAAAAAAAAAAAAAAAAAAAaaaaaaaaaaaaaaaaaa");
				playerCard.selfCard.GetDamage(enemyCard.selfCard.Attack <= playerCard.selfCard.Protection
					? 0
					: enemyCard.selfCard.Attack - playerCard.selfCard.Protection);
				enemyCard.selfCard.GetDamage(playerCard.selfCard.Attack <= enemyCard.selfCard.Protection
					? 0
					: playerCard.selfCard.Attack - enemyCard.selfCard.Protection);
				playerCard.HighliteCard();
				playerCard.selfCard.ChangeAttackState(true);
			}
			else
			{
				playerCard.selfCard.GetDamage(enemyCard.selfCard.Attack <= playerCard.selfCard.Protection
					? 0
					: enemyCard.selfCard.Attack - playerCard.selfCard.Protection);
				enemyCard.selfCard.GetDamage(playerCard.selfCard.Attack <= enemyCard.selfCard.Protection
					? 0
					: playerCard.selfCard.Attack - enemyCard.selfCard.Protection);
				playerCard.selfCard.attack = 0;
			}
		}


		if (!playerCard.selfCard.isAlive)
		{
			DestroyCard(playerCard);
		}
		else
		{
			playerCard.RefresherData();
		}

		if (!enemyCard.selfCard.isAlive)
		{
			DestroyCard(enemyCard);
		}
		else
		{
			enemyCard.RefresherData();
		}
	}

	private void DestroyCard(CardInfo card)
	{
		card.GetComponent<CardMovement>().OnEndDrag(null);

		if (EnemyFieldCards.Exists(x => x == card))
			EnemyFieldCards.Remove(card);

		if (PlayerFieldCards.Exists(x => x == card))
			PlayerFieldCards.Remove(card);

		Destroy(card.gameObject);
	}

	private void ShowMana()
	{
		playerManaTxt.text = playerMana.ToString();
		enemyManaTxt.text = enemyMana.ToString();
	}

	private void ShowHP()
	{
		playerHPTxt.text = playerHP.ToString();
		enemyHPTxt.text = enemyHP.ToString();
	}


	public void ReduceMana(bool isPlayerMana, int manacost)
	{
		if (isPlayerMana)
			playerMana = Mathf.Clamp(playerMana - manacost, 0, int.MaxValue);
		else
			enemyMana = Mathf.Clamp(enemyMana - manacost, 0, int.MaxValue);

		ShowMana();
	}

	public void DamageHero(CardInfo card, bool isEnemyAttacked)
	{
		List<Transform> taunts = new List<Transform>();
		if (isPlayerTurn)
		{
			taunts.Clear();
			for (int i = 0; i < enemyRangeField.childCount; i++)
			{
				taunts.Add(enemyRangeField.GetChild(i));
			}
		}
		else
		{
			taunts.Clear();
			for (int i = 0; i < selfRangeField.childCount; i++)
			{
				taunts.Add(selfRangeField.GetChild(i));
			}
		}

		if (taunts.Count != 0)
		{
			card.HighliteCard();
			Debug.Log("you can't attack enemy AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
		}
		else
		{
			if (card.selfCard.Windfury && card.selfCard.attack < 1)
			{
				card.selfCard.attack++;
				if (isEnemyAttacked)
					enemyHP = Mathf.Clamp(enemyHP - card.selfCard.Attack, 0, int.MaxValue);
				else
					playerHP = Mathf.Clamp(playerHP - card.selfCard.Attack, 0, int.MaxValue);
				card.HighliteCard();
				card.selfCard.ChangeAttackState(true);
			}
			else
			{
				if (isEnemyAttacked)
					enemyHP = Mathf.Clamp(enemyHP - card.selfCard.Attack, 0, int.MaxValue);
				else
					playerHP = Mathf.Clamp(playerHP - card.selfCard.Attack, 0, int.MaxValue);
				card.selfCard.attack = 0;
				card.DeHighliteCard();
			}
		}


		ShowHP();
		CheckForResult();
	}

	private void CheckForResult()
	{
		if (enemyHP == 0 || playerHP == 0)
		{
			resultGO.SetActive(true);
			StopAllCoroutines();

			if (enemyHP == 0)
				resultTxt.text = "win";
			else
				resultTxt.text = "lose";
		}
	}
}