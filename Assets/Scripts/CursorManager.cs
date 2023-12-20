using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D mainCursor;
    [SerializeField] private Texture2D attackCursor;
    private Vector2 cursorHotSpot;
    void Awake() 
    {
        SetMainCursor();
    }

    public void SetAttackCursor()
    {
        cursorHotSpot = new Vector2(attackCursor.width / 2, attackCursor.height / 2);
        Cursor.SetCursor(attackCursor, cursorHotSpot, CursorMode.ForceSoftware);
    }

    public void SetMainCursor()
    {
        Cursor.SetCursor(mainCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetAttackCursor();
        }
    }

}
