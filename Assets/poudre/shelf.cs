using UnityEngine;
using UnityEngine.EventSystems;

public class shelf : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        powder_charge.Instance.overShelf = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        powder_charge.Instance.overShelf = false;
    }
}
