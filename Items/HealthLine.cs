using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthLine : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform rectTransform;
    private RectTransform canvasRect;
    public Image blood;

    void Awake( )
    {
        mainCamera = GameObject.FindObjectOfType<Camera>( );
        rectTransform = GetComponent<RectTransform>( );
        canvasRect = GameObject.FindObjectOfType<Canvas>( ).GetComponent<RectTransform>( );
        SetPosition( );
    }

    void SetPosition( )
    {
        transform.SetParent(GameObject.FindObjectOfType<Canvas>( ).transform);
        transform.localPosition = Vector3.one;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one;
    }

    public void FollowTarget(Vector3 position)
    {
        Vector2 viewportPosition = mainCamera.WorldToViewportPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
                ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) - 30));//+85
        rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

    public void ShowHealthLine(float health, float maxHealth)
    {
        RectTransform rect = blood.GetComponent<RectTransform>( );
        rect.sizeDelta = new Vector2(90 * (float)(health / maxHealth), 15);
    }
}
