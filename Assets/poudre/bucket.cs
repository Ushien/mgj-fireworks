using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bucket : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
{
    public Canvas parentCanvas;
    public bool pouring = false;
    public Image bucketImage;
    public List<Sprite> bucketSprites;

    // Update is called once per frame
    void Update()
    {

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        transform.position = parentCanvas.transform.TransformPoint(movePos);
    }

    public void ChangePowder(int i){
        bucketImage.sprite = bucketSprites[i];
        powder_shader.Instance.selectedIndex = i;
        powder_shader.Instance.filling = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0,0,120);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0,0,0);
    }
}
