using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEndDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Vector2 _scaleTo;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private GameObject _extraHoverSpace;
    private Vector2 _originScale;
    private Vector2 _originPosition;
    private static bool canDoHover = true;

    private CardInfo card;

    private void Start()
    {
        _extraHoverSpace.SetActive(false);
        _originScale = transform.localScale;
        _originPosition = transform.localPosition;

        card = GetComponent<CardInfo>();
        
    }

    private void HoverEnter()
    {
        _extraHoverSpace.SetActive(true);

        if (canDoHover)
        {
            transform.DOScale(_scaleTo, _duration)
                .SetEase(Ease.OutCirc);
            _rectTransform.DOAnchorPosY(50f, _duration);
        }
    }

    private void HoverExit()
    {
        _extraHoverSpace.SetActive(false);

        if (canDoHover)
        {
            transform.DOScale(_originScale, _duration)
                .SetEase(Ease.OutCirc);

            _rectTransform.DOAnchorPosY(_originPosition.y, _duration);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (card != null && card.isInHand && card.isPlayer)
        {
            HoverEnter();
        }
        //HoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (card != null && card.isInHand && card.isPlayer)
        {
            HoverExit();
        }
        //HoverExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        canDoHover = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canDoHover = true;
        HoverExit();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canDoHover = true;
    }
}
