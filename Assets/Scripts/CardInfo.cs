using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    public Card selfCard;
    public bool isPlayer;
    public bool isInHand = true;

    [SerializeField] private Image _logo;
    [SerializeField] private TextMeshProUGUI _name;
	[SerializeField] private TextMeshProUGUI _type;
	[SerializeField] private TextMeshProUGUI _hp;
    [SerializeField] private TextMeshProUGUI _attack;
    [SerializeField] private TextMeshProUGUI _manacost;
    [SerializeField] private GameObject _hideObj;
    [SerializeField] private GameObject _highliteObj;

    public void HideCardInfo(Card card)
    {
        selfCard = card;
        _hideObj.SetActive(true);
        _manacost.text = "";
    }

    public void ShowCardInfo(Card card, bool isPlayer)
    {
        this.isPlayer = isPlayer;
        _hideObj.SetActive(false);
        selfCard = card;
        _logo.sprite = card.Logo;
        _name.text = card.Name;
        _type.text = card.Type;

        RefresherData();
    }

    public void RefresherData()
    {
        _hp.text = selfCard.HP.ToString();
        _attack.text = selfCard.Attack.ToString();
        _manacost.text = selfCard.Manacost.ToString();
    }

    public void HighliteCard()
    {
        _highliteObj.SetActive(true);
    }

    public void DeHighliteCard()
    {
        _highliteObj.SetActive(false);
    }

    public void TriggerBattlecry()
    {
        if (selfCard.Battlecry != null)
            selfCard.Battlecry.Invoke(selfCard);
    }

    public void ChangeIsInHandState()
    {
        if (isInHand == true)
        {
            isInHand = false;
        }
    }
}