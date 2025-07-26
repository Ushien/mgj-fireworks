using UnityEngine;
using UnityEngine.EventSystems;

public class shelf : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        powder_shader.Instance.overShelf = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        powder_shader.Instance.overShelf = false;
    }
}
