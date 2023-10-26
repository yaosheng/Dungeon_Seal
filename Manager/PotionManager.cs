using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Camera mainCamera
    {
        get {
            return GameObject.FindObjectOfType<Camera>( ) as Camera;
        }
    }
    private GameManager GM
    {
        get {
            return GameObject.FindObjectOfType<GameManager>( ) as GameManager;
        }
    }
    private Vector2 result;
    private bool IsClickToUse = false;
    private Slot currentSlot;
    private Canvas canvas;
    public int potionNumber;
    public Text potionText;
    //public List<Potion> potionGroup = new List<Potion>( );

    void Awake( )
    {
        currentSlot = GetComponent<Slot>( );
    }

    public void ShowPotionNumber( )
    {
        potionText.text = potionNumber.ToString( );
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        Debug.Log("down");
        if(currentSlot.currentTreasrue.usedType == Type.ClickToUse) {
            IsClickToUse = true;
        }
        else {
            return;
        }
        //currentSlot = eventData.pointerPressRaycast.gameObject.GetComponentInParent<Slot>( );
        //if(currentSlot.currentTreasrue.usedType == Type.ClickToUse) {
        //    IsClickToUse = true;
        //}
        //else {
        //    return;
        //}
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        Debug.Log("up");
        if(IsClickToUse && potionNumber > 0) {
            currentSlot.UseTreasureAbility(GM.hero);
            potionNumber--;
            ShowPotionNumber( );
            IsClickToUse = false;
        }
        else {
            IsClickToUse = false;
            return;
        }
    }
}
