using UnityEngine;
public class InGameCamera : MonoBehaviour
{
    const float SCENEWIDTH = 80, SCENEHEIGHT = 40;
    enum Directions { Up, Left, Down, Right }

    Camera instance = null;
    [SerializeField] CameraEffect cameraEffect = null;

    float maxPosX = 0;
    float minPosX = 0;
    float maxPosY = 0;
    float minPosY = 0;
    [SerializeField] float fallOffDistance = 0;
    [Range(0, 100)] [SerializeField] float cameraSpeed = 0;

    [Header("Zoom")]
    [SerializeField] float zoomMin = 0;
    [SerializeField] float zoomMax = 0;
    [SerializeField] float zoomLerpSpeed = 0;
    [Range(0, 1)] [SerializeField] float zoomSpeedScale = 0;

    CameraTransform newCameraTransform = new CameraTransform();

    Vector3 mouseDownPos = new Vector3();
    Vector3 mouseUpPos = new Vector3();

    float cursorDetectionRange = 30;

    bool isTracking = false;
    [SerializeField] Vector3 currentTrackingPoint = new Vector3();

    private void Start()
    {
        instance = Camera.main;
        newCameraTransform = new CameraTransform(instance);
    }
    private void Update()
    {
        TrackPosition();
        InputHandler();
        UpdatePosition();
    }
    void InputHandler()
    {
        //Zoom
        if (Input.GetAxis("CameraZoom") != 0)
        {
            newCameraTransform.cameraPosition = transform.position;
            newCameraTransform.cameraSize -= Input.GetAxis("CameraZoom") * zoomSpeedScale;
        }
        //Edge detection
        if (Input.mousePosition.x > Screen.width - cursorDetectionRange)
        {
            MoveDirection(Directions.Right);
        }
        if (Input.mousePosition.x < 0 + cursorDetectionRange)
        {
            MoveDirection(Directions.Left);
        }
        if (Input.mousePosition.y > Screen.height - cursorDetectionRange)
        {
            MoveDirection(Directions.Up);
        }
        if (Input.mousePosition.y < 0 + cursorDetectionRange)
        {
            MoveDirection(Directions.Down);
        }
        //Middle Mouse Movement
        if (Input.GetMouseButtonDown(2))
        {
            mouseDownPos = instance.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            mouseUpPos = instance.ScreenToWorldPoint(Input.mousePosition);
            newCameraTransform.cameraPosition = transform.position + mouseDownPos - mouseUpPos;
        }
        //Keys
        if (Input.GetAxis("Horizontal") > 0)
        {
            MoveDirection(Directions.Right);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            MoveDirection(Directions.Left);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            MoveDirection(Directions.Up);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            MoveDirection(Directions.Down);
        }
    }
    public void UpdatePosition()
    {
        //Apply zoom
        newCameraTransform.cameraSize = Mathf.Clamp(newCameraTransform.cameraSize, zoomMin, zoomMax);
        instance.orthographicSize = Mathf.Lerp(instance.orthographicSize, newCameraTransform.cameraSize, zoomLerpSpeed * Time.deltaTime);
        //Apply Movement
        maxPosX = SCENEWIDTH + (instance.ScreenToWorldPoint(new Vector3(0, instance.scaledPixelWidth / 2, 0)).x - transform.position.x);
        minPosX = -maxPosX;
        maxPosY = SCENEHEIGHT + (instance.ScreenToWorldPoint(new Vector3(instance.scaledPixelHeight / 2, 0, 0)).y - transform.position.y);
        minPosY = -maxPosY;
        Vector3 lerpVector = Vector3.Lerp(transform.position, newCameraTransform.cameraPosition, cameraSpeed * (instance.orthographicSize / zoomMax) * Time.deltaTime);
        Vector3 clampedVector = new Vector3(Mathf.Clamp(lerpVector.x, minPosX, maxPosX), Mathf.Clamp(lerpVector.y, minPosY, maxPosY), -10);

        transform.position = clampedVector;
    }
    void MoveDirection(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                newCameraTransform.cameraPosition.y += cameraSpeed;
                break;
            case Directions.Left:
                newCameraTransform.cameraPosition.x -= cameraSpeed;
                break;
            case Directions.Down:
                newCameraTransform.cameraPosition.y -= cameraSpeed;
                break;
            case Directions.Right:
                newCameraTransform.cameraPosition.x += cameraSpeed;
                break;
            default:
                break;
        }
        //Clamp newCameraPosition direction
        newCameraTransform.cameraPosition = new Vector3(
        Mathf.Clamp(newCameraTransform.cameraPosition.x, transform.position.x - fallOffDistance, transform.position.x + fallOffDistance),
        Mathf.Clamp(newCameraTransform.cameraPosition.y, transform.position.y - fallOffDistance, transform.position.y + fallOffDistance)
        );
    }
    void TrackPosition()
    {
        if (isTracking)
        {
            IsPointReached();
            if (currentTrackingPoint == null)
            {
                isTracking = false;
                return;
            }
            else
            {
                newCameraTransform.cameraPosition = currentTrackingPoint;
            }
        }
        bool IsPointReached()
        {
            const float DETECTRANGE = 0.5f;

            if (Vector2.Distance(instance.transform.position, newCameraTransform.cameraPosition) < DETECTRANGE)
            {
                Debug.Log("reached");
                return true;
            }
            return false;
        }
    }
    public void ToggleTracking()
    {
        isTracking = !isTracking;
    }
    public void SetTarget(Transform trackPos)
    {
        currentTrackingPoint = trackPos.position;
    }
    public void CameraShake(float intensity, float duration)
    {
        cameraEffect.ApplyEffect(intensity, duration, CameraEffect.Effect.Shake);
    }
    protected struct CameraTransform
    {
        public float cameraSize;
        public Vector3 cameraPosition;
        public CameraTransform(Camera camera)
        {
            cameraSize = camera.orthographicSize;
            cameraPosition = camera.transform.position;
        }
    }
}