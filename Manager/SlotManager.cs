using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
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
    private Slot currentSlot;
    private Vector2 result;
    private bool IsThereDragItem = false;
    private bool IsRaycastHit = false;
    private bool IsClickToUse = false;
    private Collider hitCollider;
    private int inventoryCount = 5;
    private Image currentImage;

    public Slot slotObject;
    public ScrollRect scrollRect;
    private Canvas canvas;

    void Awake( )
    {
        canvas = transform.root.GetComponent<Canvas>( );
    }

    void Start( )
    {
        SetSlots( );
    }

    void SetSlots( )
    {
        for(int i = 0; i < inventoryCount; i++) {
            Slot slot = Instantiate(slotObject) as Slot;
            slot.transform.SetParent(this.transform);
            slot.GetComponent<RectTransform>( ).localPosition = Vector3.zero;
            slot.transform.localScale = Vector3.one;
            slot.GetComponent<RectTransform>( ).localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnDrag( PointerEventData eventData )
    {
        IsClickToUse = false;
        if(IsThereDragItem) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            RaycastHit hit;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>( ), Input.mousePosition, mainCamera, out result);
            currentImage.transform.localPosition = new Vector3(result.x, result.y, 0);
            if(Physics.Raycast(ray, out hit)) {
                IsRaycastHit = true;
                hitCollider = hit.collider;
                //Debug.Log(hit.collider.name);
                GM.fm.ChangeRenderer(hit.collider);
            }
            else {
                GM.fm.ResetRenderer( );
                IsRaycastHit = false;
            }
        }
        else {
            if(Mathf.Abs(eventData.delta.x) >= Mathf.Abs(eventData.delta.y)) {
                scrollRect.horizontalNormalizedPosition -= eventData.delta.x / ((float)Screen.width);
            }
        }
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        currentSlot = eventData.pointerPressRaycast.gameObject.GetComponentInParent<Slot>( );
        if(currentSlot.currentTreasrue != null && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y)) {
            if(currentSlot.currentTreasrue.usedType == Type.ClickToUse) {
                currentSlot = null;
                return;
            }
            else {
                IsThereDragItem = true;
                currentImage = eventData.pointerPressRaycast.gameObject.GetComponent<Image>( );
                currentImage.transform.SetParent(canvas.transform);
            }
        }
        else {
            currentSlot = null;
            currentImage = null;
            IsThereDragItem = false;
        }
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        if(IsThereDragItem) {
            if(!IsRaycastHit) {
                currentImage.transform.SetParent(currentSlot.transform);
                currentImage.transform.localPosition = Vector3.zero;
                GM.fm.ResetRenderer( );
            }
            else {
                currentImage.transform.SetParent(currentSlot.transform);
                currentImage.transform.localPosition = Vector3.zero;
                currentSlot.UseTreasureAbility(GM.hero, hitCollider);
                currentSlot.RunOut( );
                currentSlot.AssignSetting(this.transform, inventoryCount - 1, 1);
            }
            currentSlot = null;
            hitCollider = null;
            currentImage = null;
            IsThereDragItem = false;
        }
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        currentSlot = eventData.pointerPressRaycast.gameObject.GetComponentInParent<Slot>( );
        if(currentSlot.currentTreasrue != null && currentSlot.currentTreasrue.usedType == Type.ClickToUse) {
            IsClickToUse = true;
        }
        else {
            currentSlot = null;
            currentImage = null;
            return;
        }
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        if(IsClickToUse) {
            currentSlot.UseTreasureAbility(GM.hero);
            currentSlot.RunOut( );
            currentSlot.AssignSetting(this.transform, inventoryCount - 1, 1);
            IsClickToUse = false;
            currentSlot = null;
            currentImage = null;
        }
    }

    void AddNewSlot( )
    {
        Slot slot = Instantiate(slotObject) as Slot;
        slot.transform.SetParent(this.transform);
    }
}

//Floor fr;
//if(hitCollider.tag == "Enemy") {
//    fr = hitCollider.GetComponentInParent<Enemy>( ).data.currentFloor;
//    currentSlot.UseThis(GM.hero, fr);
//}
//else if (hitCollider.tag == "Obstacle") {
//    fr = hitCollider.GetComponentInParent<Obstacle>( ).currentFloor;
//    currentSlot.UseThis(GM.hero, fr);
//}

//currentSlot.RunOut( );
//currentSlot.Setting(this.transform, 1);

//currentSlot.UseThis(GM.hero, null);
//currentSlot.RunOut( );
//currentSlot.AssignSetting(this.transform, inventoryCount, 1);

//if(currentSlot != null && IsDragItem && !IsraycastHit) {
//    currentSlot.PutInSetting(this.transform, slotNumber, 1);
//    floorManager.ResetObstaclesRenderer( );
//    currentSlot = null;
//    IsDragItem = false;
//}
//else {
//}

//currentSlot.transform.SetParent(canvas.transform);
//currentSlot.transform.localPosition = Vector3.zero;
//currentSlot.GetComponent<Image>( ).color = new Color(0, 0, 0, 0);
//currentSlot.transform.SetParent(this.transform);
//currentSlot.transform.SetSiblingIndex(slotNumber);
//currentSlot.GetComponent<Image>( ).color = new Color(0, 0, 0, 1);

//currentSlot.transform.localPosition = tempVector;
//Debug.Log("x :" + Input.mousePosition.x + ", " +"y :" +Input.mousePosition.y);
//RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, mainCamera, out result);
//Debug.Log(RectTransformUtility.PixelAdjustPoint(Input.mousePosition, currentSlot.transform, canvas));
//Debug.Log("result :"+ result);
//currentSlot.transform.position = Input.mousePosition;
//currentSlot.GetComponent<RectTransform>()
//else {
//    Debug.Log("drag item");
//    Debug.Log(eventData.pointerPressRaycast.gameObject.name);
//}

//MeshRenderer mr;
//foreach(Floor floor in floorManager.floors) {
//    if(floor.obstacle != null) {
//        mr = floor.obstacle.GetComponentInChildren<MeshRenderer>( );
//        if(mr.sharedMaterial == attackedMaterial) {
//            mr.sharedMaterial = standardMaterial;
//        }
//    }
//}

//Collider co;
//MeshRenderer mr1;
//foreach(Floor floor in floorManager.floors) {
//    if(floor.obstacle != null) {
//        co = floor.obstacle.GetComponentInChildren<Collider>( );
//        mr1 = floor.obstacle.GetComponentInChildren<MeshRenderer>( );
//        if(hit.collider == co) {
//            if(mr1.sharedMaterial == standardMaterial) {
//                mr1.sharedMaterial = attackedMaterial;
//            }
//        }
//        else {
//            if(mr1.sharedMaterial == attackedMaterial) {
//                mr1.sharedMaterial = standardMaterial;
//            }
//        }
//    }
//}
