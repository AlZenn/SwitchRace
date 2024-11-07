using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class btnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image pressedImg;
    public btnHover[] otherButtons;
    public bool isPressed = false;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        if (isPressed)
        {
            ApplyPressedState();
        }
        
        else
        {
            ResetHoverIcon();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed && pressedImg != null)
        {
            pressedImg.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPressed) return;
        
        foreach (var tab in otherButtons)
        {
            if (tab != this)
            {
                tab.ResetTab();
            }
        }
        
        isPressed = true;
        ApplyPressedState();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ApplyPressedState()
    {
        if (pressedImg != null)
        {
            pressedImg.enabled = true;
        }
    }

    private void ResetHoverIcon()
    {
        isPressed = false;
        if (pressedImg != null)
        {
            pressedImg.enabled = false;
        }
    }

    public void ResetTab()
    {
        isPressed = false;
        if (pressedImg != null)
        {
            pressedImg.enabled = false;
        }
    }
}
