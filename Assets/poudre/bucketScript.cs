using UnityEngine;

public class bucketScript : MonoBehaviour
{
    public bool isDragging = false;
    private Vector3 offset;
    public Camera powderCamera;
    public Vector3 basePosition;
    public float returnSpeed = 5f;
    public int index;
    public float rotSpeed = 1f;
    public FireworkAudioManager fireworkAudioManager;

    void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        if(!MixingManager.Instance.inStudio)
            return;
        if(!isDragging){
            transform.position = Vector3.Lerp(transform.position, basePosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * rotSpeed);
        }

        if (powderCamera == null)
        {
            return;
        }

        Ray ray = powderCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!isDragging)
                    {
                        fireworkAudioManager.PlayBipSound();
                    }
                    isDragging = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y, basePosition.z-40f);
                    PowderManager.Instance.SetParticleColor(index);
                    PowderManager.Instance.SetParticleAmount(2000);
                    //transform.rotation = Quaternion.Euler(0,0,120);
                    // Calculate offset between object position and hit point
                    offset = transform.position - hit.point;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(isDragging){
                isDragging = false;
                transform.position = new Vector3 (transform.position.x, transform.position.y, basePosition.z);
            }
            PowderManager.Instance.SetParticleAmount(0);
        }

        if (isDragging)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,130), Time.deltaTime * rotSpeed);
            Vector3 mouseWorldPos = powderCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3 (mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
            // // Project mouse position to some plane to get world position for dragging
            // Plane dragPlane = new Plane(Vector3.back, Vector3.zero); // Z=0 plane, adjust if needed
            // if (dragPlane.Raycast(ray, out float enter))
            // {
            //     Vector3 hitPoint = ray.GetPoint(enter);
            //     transform.position = hitPoint + offset;
            // }
        }
    }
}
