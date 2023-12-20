using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Card
{
    public string Name;
    public string Type;
    public Sprite Logo;
    public int Attack;
    public int HP;
    public int Manacost;
    public bool CanAttack;
    public int Protection;
    public bool Windfury;
    public int attack;

    public delegate void BattlecryDelegate(Card card);
    public BattlecryDelegate Battlecry;

    public bool isAlive
    {
        get { return HP > 0; }
    }

    public Card(string name, string logoPath, string type, int attack, int hp, int manacost, int protect, bool windfury)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Type = type;
        Attack = attack;
        HP = hp;
        Manacost = manacost;
        CanAttack = false;
        Protection = protect;
        this.attack = 0;
        Windfury = windfury;
        Battlecry = null;
    }

    public void ChangeAttackState(bool can)
    {
        CanAttack = can;
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
    }
}

public static class CardList
{
    public static List<Card> allPlayerCards = new List<Card>();
	public static List<Card> allEnemyCards = new List<Card>();
}

public class CardManager : MonoBehaviour
{
    public void Awake()
    {
        CardList.allPlayerCards.Add(new Card("Рекруты", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Recruits.png", "Авангард", 1, 1, 3, 0, false));
		CardList.allPlayerCards.Add(new Card("Регулярная пехота", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/LightPehota.png", "Авангард", 2, 1, 4, 0, false));
		CardList.allPlayerCards.Add(new Card("Гренадеры", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Grenaders.jpg", "Авангард", 3, 2, 7, 0, false));
		CardList.allPlayerCards.Add(new Card("Мушкетёры", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Mushketers.png", "Основа", 4, 2, 2, 0, false));
		CardList.allPlayerCards.Add(new Card("Интендант", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Indendant.jpg", "Основа", 4, 4, 5, 0, false));
		CardList.allPlayerCards.Add(new Card("Конные егеря", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/HorsesEger.png", "Основа", 4, 4, 2, 0, false));
		CardList.allPlayerCards.Add(new Card("Казаки", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Cossacks.png", "Основа", 4, 4, 5, 0, false));
		CardList.allPlayerCards.Add(new Card("Атаман", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Ataman.jpg", "Основа", 5, 1, 3, 0, false));
		CardList.allPlayerCards.Add(new Card("Адъютант", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Adjutant.png", "Арьергард", 4, 2, 3, 0, false));
		CardList.allPlayerCards.Add(new Card("Полевая артиллерия", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Artillery.jpg", "Арьергард", 7, 6, 6, 0, false));
		CardList.allPlayerCards.Add(new Card("Подкрепления", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Help.jpg", "Авангард", 3, 0, 0, 0, false));
		CardList.allPlayerCards.Add(new Card("Залп", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Artillery.jpg", "Авангард", 5, 0, 0, 0, false));
		CardList.allPlayerCards.Add(new Card("Фокусированный огонь", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Artillery.jpg", "Авангард", 5, 0, 0, 0, false));

        CardList.allEnemyCards.Add(new Card("Рекруты", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Recruits.png", "Авангард", 1, 1, 3, 0, false));
		CardList.allEnemyCards.Add(new Card("Регулярная пехота", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/LightPehota.png", "Авангард", 2, 1, 4, 0, false));
		CardList.allEnemyCards.Add(new Card("Гренадеры", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Grenaders.jpg", "Авангард", 3, 2, 7, 0, false));
		CardList.allEnemyCards.Add(new Card("Мушкетёры", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Mushketers.png", "Основа", 4, 2, 2, 0, false));
		CardList.allEnemyCards.Add(new Card("Интендант", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Indendant.jpg", "Основа", 4, 4, 5, 0, false));
		CardList.allEnemyCards.Add(new Card("Конные егеря", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/HorsesEger.png", "Основа", 4, 4, 2, 0, false));
		CardList.allEnemyCards.Add(new Card("Казаки", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Cossacks.png", "Основа", 4, 4, 5, 0, false));
		CardList.allEnemyCards.Add(new Card("Атаман", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Ataman.jpg", "Основа", 5, 1, 3, 0, false));
		CardList.allEnemyCards.Add(new Card("Адъютант", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Adjutant.png", "Арьергард", 4, 2, 3, 0, false));
		CardList.allEnemyCards.Add(new Card("Полевая артиллерия", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Artillery.jpg", "Арьергард", 7, 6, 6, 0, false));
		CardList.allEnemyCards.Add(new Card("Подкрепления", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Help.jpg", "Авангард", 3, 0, 0, 0, false));
		CardList.allEnemyCards.Add(new Card("Залп", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Artillery.jpg", "Авангард", 5, 0, 0, 0, false));
		CardList.allEnemyCards.Add(new Card("Фокусированный огонь", "Assets/Resources/Sprites/Cards/PlayerCardsLogo/Artillery.jpg", "Авангард", 5, 0, 0, 0, false));
	}


}