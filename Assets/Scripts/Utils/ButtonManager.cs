using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button launchButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button backButton;

    void Start()
    {
        startButton.gameObject.SetActive(true);
        launchButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void StartButtonClicked()
    {
        startButton.gameObject.SetActive(false);
    }

    public void LaunchButtonClicked()
    {
        launchButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }
    
    public void BackButtonClicked()
    {
        shootButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
}
