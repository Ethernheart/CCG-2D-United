using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform defaultParent;
    public Transform defaultTempCardParent;
    public GameManager gameManager;

    private GameObject _tempCard;
    private Vector3 _offset;
    private Camera _camera;
    private bool _isDraggable;
    private bool _isOnBoard;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    private void Awake()
    {
        _camera = Camera.allCameras[0];
        _tempCard = GameObject.Find("TempCard");
        gameManager = FindObjectOfType<GameManager>();
    }

    private void CheckPosition()
    {
        int newIndex = defaultTempCardParent.childCount;

        for (int i = 0; i < defaultTempCardParent.childCount; i++)
        {
            if (transform.position.x < defaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;

                if (_tempCard.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
        }

        _tempCard.transform.SetSiblingIndex(newIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
 
        _offset = transform.position - _camera.ScreenToWorldPoint(eventData.position);

        defaultParent = transform.parent;
        defaultTempCardParent = transform.parent;

        _isDraggable = (defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_HAND &&
            //|| 
                        //defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_MELEE_FIELD || 
                        //defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_RANGE_FIELD ||   
						//defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_MEDIUM_FIELD) && 
                        gameManager.isPlayerTurn);

        _isOnBoard = (defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_MELEE_FIELD ||
                        defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_RANGE_FIELD ||
                        defaultParent.GetComponent<DropPlace>().Type == FieldType.SELF_MEDIUM_FIELD) &&
                        gameManager.isPlayerTurn;
        if(_isOnBoard)
        {
            AttackArrowEffect arrow = FindObjectOfType<AttackArrowEffect>();

            arrow.transform.GetChild(0).gameObject.SetActive(true);
            arrow.pointA = _pointA;
            arrow.pointB = _pointB;
        }

        if (!_isDraggable)
            return;

        _tempCard.transform.SetParent(defaultParent);
        _tempCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDraggable)
            return;

        Vector3 newPos = _camera.ScreenToWorldPoint(eventData.position);
        transform.position = newPos + _offset;

        if (_tempCard.transform.parent != defaultTempCardParent)
            _tempCard.transform.SetParent(defaultTempCardParent);
        if (defaultParent.GetComponent<DropPlace>().Type != FieldType.SELF_MELEE_FIELD && 
            defaultParent.GetComponent<DropPlace>().Type != FieldType.SELF_RANGE_FIELD && 
            defaultParent.GetComponent<DropPlace>().Type != FieldType.SELF_MEDIUM_FIELD)
            CheckPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isOnBoard)
        {
            AttackArrowEffect arrow = FindObjectOfType<AttackArrowEffect>();
            arrow.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (!_isDraggable)
            return;

        transform.SetParent(defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(_tempCard.transform.GetSiblingIndex());

        _tempCard.transform.SetParent(GameObject.Find("Canvas").transform);
        _tempCard.transform.localPosition = new Vector2(2270, -540);
    }
}
