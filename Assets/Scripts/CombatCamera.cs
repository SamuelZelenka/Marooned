using UnityEngine;
public class CombatCamera : MonoBehaviour
{
    const float SCENEWIDTH = 80, SCENEHEIGHT = 40;
    enum Directions {Up, Left, Down , Right}

    float maxPosX;
    float minPosX;
    float maxPosY;
    float minPosY;
    [SerializeField] float fallOffDistance;
    [Range(0, 100)] [SerializeField] float cameraSpeed;

    [Header("Zoom")]
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;
    [SerializeField] float zoomLerpSpeed;
    [Range(0, 1)] [SerializeField] float zoomSpeedScale;

    CameraTransform newCameraTransform;

    Vector3 mouseDownPos;
    Vector3 mouseUpPos;

    float cursorDetectionRange = 30;

    bool isTracking;
    [SerializeField] Vector3 currentTrackingPoint;

    private void Start()
    {
        isTracking = false;
        newCameraTransform = new CameraTransform(Camera.main);
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
            mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    void UpdatePosition()
    {
        //Apply zoom
        newCameraTransform.cameraSize = Mathf.Clamp(newCameraTransform.cameraSize, zoomMin, zoomMax);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, newCameraTransform.cameraSize, zoomLerpSpeed * Time.deltaTime);
        //Apply Movement
        maxPosX = SCENEWIDTH + (Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.scaledPixelWidth / 2, 0)).x - transform.position.x);
        minPosX = -maxPosX;
        maxPosY = SCENEHEIGHT + (Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.scaledPixelHeight / 2, 0, 0)).y - transform.position.y);
        minPosY = -maxPosY;
        Vector3 lerpVector = Vector3.Lerp(transform.position, newCameraTransform.cameraPosition, cameraSpeed * (Camera.main.orthographicSize / zoomMax) * Time.deltaTime);
        Vector3 clampedVector = new Vector3(Mathf.Clamp(lerpVector.x, minPosX, maxPosX), Mathf.Clamp(lerpVector.y, minPosY, maxPosY), -10);

        transform.position = clampedVector;
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

            if (Vector2.Distance(Camera.main.transform.position, newCameraTransform.cameraPosition) < DETECTRANGE)
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
    struct CameraTransform
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