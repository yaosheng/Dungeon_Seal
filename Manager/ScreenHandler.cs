using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScreenHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [System.Serializable]
    public class ScreenEvent : UnityEvent<int> { }

    public UnityEvent beginControl;
    public ScreenEvent controlling;
    public UnityEvent endControl;

    public void OnBeginDrag( PointerEventData eventData )
    {
        this.beginControl.Invoke( );
    }

    public void OnDrag( PointerEventData eventData )
    {
        float deltaX = eventData.delta.x;
        float deltaY = eventData.delta.y;
        //string tempString = "";
        int tempInt = 0;
        if(Mathf.Abs(deltaX) >= Mathf.Abs(deltaY)) {
            if(deltaX >= 0) {
                //tempString = "Right";
                tempInt = 0;
            }
            else {
                //tempString = "Left";
                tempInt = 1;
            }
        }
        else {
            if(deltaY >= 0) {
                //tempString = "Up";
                tempInt = 2;
            }
            else {
                //tempString = "Down";
                tempInt = 3;
            }
        }
        //Debug.Log("tempInt : " + tempInt);
        ////Debug.Log(eventData.delta);
        ////this.controlling.Invoke(tempString);
        this.controlling.Invoke(tempInt);
    }

    public void OnEndDrag( PointerEventData eventData )
    {

        this.endControl.Invoke( );
    }
}
