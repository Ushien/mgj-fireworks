using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRocketBuidling : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    
    private Canvas currentCanvas;

    private void Start()
    {
        currentCanvas = GetComponent<Canvas>();
    }
    
    public void OnPlayButtonClicked()    
    {
        Debug.Log("Play button clicked. Starting the rocket...");
        canvas.enabled = true; 
        currentCanvas.enabled = false; 
    }
}
