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

        // Check bounds and update counter
        float xPos = transform.position.x;

        // if (xPos >= minX && xPos <= maxX)
        // {
        //     timer += Time.deltaTime;
        //     if (timer >= counterInterval)
        //     {
        //         counter++;
        //         timer = 0f;
        //         Debug.Log("Counter: " + counter);
        //     }
        // }
        // else
        // {
        //     // Optional: reset timer when out of bounds
        //     timer = 0f;
        // }
    }

    public void ChangePowder(int i){
        bucketImage.sprite = bucketSprites[i];
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
