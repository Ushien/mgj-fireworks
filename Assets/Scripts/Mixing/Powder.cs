using UnityEngine;
using UnityEngine.InputSystem;

public class Powder : MonoBehaviour
{
    private Vector2 mouseScreenPos;
    private Vector2 worldPos;
    void Update()
    {
        mouseScreenPos = Mouse.current.position.ReadValue();
        worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
        Debug.Log("in collider bounds");
        transform.position = new Vector2(worldPos.x, worldPos.y);
    }
    
    
}
