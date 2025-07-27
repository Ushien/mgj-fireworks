using UnityEngine.UI;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera powderCamera;
    [SerializeField] private Camera shootCamera;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private Button launchButton;
    private float saveCameraOrtho;
    private Vector3 saveCameraPosition;
    private bool goToPowder = false;
    private bool goToShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powderCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (goToPowder)
        {
            // lerp position
            shootCamera.transform.position = Vector3.Lerp(
                shootCamera.transform.position,
                powderCamera.transform.position,
                Time.deltaTime * lerpSpeed
            );

            // lerp fov
            shootCamera.orthographicSize = Mathf.Lerp(
                shootCamera.orthographicSize,
                powderCamera.orthographicSize,
                Time.deltaTime * lerpSpeed
            );

            if (Vector3.Distance(shootCamera.transform.position, powderCamera.transform.position) < 0.8f)
            {
                GoToPowderCamera();
            }
        }
        else if (goToShoot)
        {
            // lerp position
            powderCamera.transform.position = Vector3.Lerp(
                powderCamera.transform.position,
                shootCamera.transform.position,
                Time.deltaTime * lerpSpeed
            );

            // lerp fov
            powderCamera.orthographicSize = Mathf.Lerp(
                powderCamera.orthographicSize,
                shootCamera.orthographicSize,
                Time.deltaTime * lerpSpeed
            );

            if (Vector3.Distance(powderCamera.transform.position, shootCamera.transform.position) < 0.8f)
            {
                GoToShootCamera();
            }
        }
    }

    public void SwitchCamera()
    {
        if (powderCamera.enabled)
        {
            goToShoot = true;
            saveCameraOrtho = powderCamera.orthographicSize;
            saveCameraPosition = powderCamera.transform.position;
        }
        else
        {
            goToPowder = true;
            saveCameraOrtho = shootCamera.orthographicSize;
            saveCameraPosition = shootCamera.transform.position;
        }
    }

    private void GoToShootCamera()
    {
        powderCamera.enabled = false;
        shootCamera.enabled = true;
        goToShoot = false;
        goToPowder = false;
        powderCamera.orthographicSize = saveCameraOrtho;
        powderCamera.transform.position = saveCameraPosition;
    }

    private void GoToPowderCamera()
    {
        powderCamera.enabled = true;
        shootCamera.enabled = false;
        goToShoot = false;
        goToPowder = false;
        shootCamera.orthographicSize = saveCameraOrtho;
        shootCamera.transform.position = saveCameraPosition;
        launchButton.gameObject.SetActive(true);
    }
}
