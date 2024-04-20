using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;


public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void PointerEnterDelegate();
    public delegate void PointerExitDelegate();

    public event PointerEnterDelegate OnPointerEnterEvent;
    public event PointerExitDelegate OnPointerExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke();
    }
}
