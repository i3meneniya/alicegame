using UnityEngine;
using Unity.Cinemachine;

public class ChangingSize : MonoBehaviour
{
    private bool isSmall = false;
    [Header("Scale Multipliers")]
    public float normalScaleX = 1f;
    public float normalScaleY = 1f;
    public float smallScaleX = 0.5f;
    public float smallScaleY = 0.5f;
    public CinemachineCamera cinemachineCamera;
    
    [Header("Camera Zoom Settings")]
    public float normalCameraSize = 5f;
    public float smallCameraSize = 2.5f;
    public float zoomSpeed = 5f;
    
    private Vector3 originalScale;
    private float targetCameraSize;
    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetCameraSize = normalCameraSize;
        targetScale = originalScale;
        
        if (cinemachineCamera != null)
        {
            var lens = cinemachineCamera.Lens;
            lens.OrthographicSize = normalCameraSize;
            cinemachineCamera.Lens = lens;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleSize();
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, zoomSpeed * Time.deltaTime);
        
        if (cinemachineCamera != null)
        {
            var lens = cinemachineCamera.Lens;
            float currentSize = lens.OrthographicSize;
            float newSize = Mathf.Lerp(currentSize, targetCameraSize, zoomSpeed * Time.deltaTime);
            lens.OrthographicSize = newSize;
            cinemachineCamera.Lens = lens;
        }
    }

    private void ToggleSize()
    {
        if (isSmall)
        {
            targetScale = originalScale;
            targetCameraSize = normalCameraSize;
            isSmall = false;
        }
        else
        {
            targetScale = new Vector3(
                originalScale.x * smallScaleX,
                originalScale.y * smallScaleY,
                originalScale.z
            );
            targetCameraSize = smallCameraSize;
            isSmall = true;
        }
    }
}
