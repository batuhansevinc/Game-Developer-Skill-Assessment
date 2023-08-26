using UnityEngine;

namespace HappyHour.Concretes.Controllers
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] Vector3 touchStart;
        [SerializeField] float zoomOutMin = 1;
        [SerializeField] float zoomOutMax = 8;
        [SerializeField] float limit = 30f;
        
        float initialTouchDistance;
        float initialOrthographicSize;
        

        void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        void HandleMovement()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.touchCount == 1)
            {
                touchStart = GetWorldPosition();
            }
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.touchCount == 1)
            {
                Vector3 direction = touchStart - GetWorldPosition();
                transform.position += direction;
                
                // Kamera sınırlamasını uygula
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, -limit, limit),
                    Mathf.Clamp(transform.position.y, -limit, limit),
                    transform.position.z
                );
            }
        }

        void HandleZoom()
        {
            // Mouse Zoom
            float zoomChange = Input.GetAxis("Mouse ScrollWheel");
            float zoomAmount = Camera.main.orthographicSize;
            zoomAmount -= zoomChange * 4f;
            
            // Touch Zoom (Pinch to Zoom)
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    initialTouchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    initialOrthographicSize = Camera.main.orthographicSize;
                }
                else
                {
                    var currentTouchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    var distanceDifference = initialTouchDistance - currentTouchDistance;
                    zoomAmount = initialOrthographicSize + distanceDifference * 0.01f;
                }
            }

            // Apply the zoom
            zoomAmount = Mathf.Clamp(zoomAmount, zoomOutMin, zoomOutMax);
            Camera.main.orthographicSize = zoomAmount;
        }

        Vector3 GetWorldPosition()
        {
            if (Input.touchCount > 0)
            {
                return Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            }
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
