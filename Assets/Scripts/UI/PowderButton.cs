using UnityEngine;

public class PowderButton : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    
    public void OnPowderButtonClicked()
    {
        
        Debug.Log("Powder button clicked : " + transform.name);
    }
}
