using UnityEngine;
public class InGameCamera : MonoBehaviour
{
    enum Directions { Up, Left, Down, Right }

    [SerializeField] Camera instance = null;
    [SerializeField] bool edgeMovement = true;

    [Header("Scene")]
    [SerializeField] Transform minPosTranform = null;
    [SerializeField] Transform maxPosTransform = null;

    [SerializeField] Vector3 maxPos = new Vector3();
    [SerializeField] Vector3 minPos = new Vector3();

    [Header("Camera")]
    [SerializeField] CameraEffect cameraEffect = null;
    [SerializeField] float cameraLerpSpeed = 5;
    [Range(0, 100)] [SerializeField] float cameraSpeed = 0;

    [Header("Zoom")]
    [SerializeField] float zoomMin = 0;
    [SerializeField] float zoomMax = 0;
    [SerializeField] float zoomLerpSpeed = 0;
    [SerializeField] float zoomSpeed = 1;
    [Range(1, 10)] [SerializeField] float zoomSpeedScale = 1;

    public delegate void CameraTrackHandler(Transform transform);
    public static CameraTrackHandler OnSelectedCharacter;

    CameraTransform newCameraTransform = new CameraTransform();

    Vector3 mouseDownPos = new Vector3();
    Vector3 mouseUpPos = new Vector3();

    float cursorDetectionRange = 30;

    [Header("Tracking")]
    bool isTracking = false;
    float detectRange = 0.5f;
    [SerializeField] Vector3 currentTrackingPoint = new Vector3();


    private void Start()
    {
        newCameraTransform = new CameraTransform(instance);

        if (minPosTranform != null && maxPosTransform != null)
        {
            minPos = minPosTranform.localPosition;
            maxPos = maxPosTransform.localPosition;
        }
    }

    private void Update()
    {
        TrackPosition();
        InputHandler();
        UpdatePosition();
    }
    private void OnEnable()
    {
        OnSelectedCharacter += SetTarget;
    }
    private void OnDisable()
    {
        OnSelectedCharacter -= SetTarget;
    }
    public void SetCamera(Camera camera)
    {
        instance = camera;
    }
    void InputHandler()
    {
        //Zoom
        if (Input.GetAxis("CameraZoom") != 0)
        {
            //newCameraTransform.cameraPosition = transform.position;
            newCameraTransform.cameraSize -= Input.GetAxis("CameraZoom") * zoomSpeed * Time.deltaTime;
        }
        //Middle Mouse Movement
        if (Input.GetMouseButtonDown(2))
        {
            mouseDownPos = instance.ScreenToWorldPoint(Input.mousePosition);
            isTracking = false;
        }
        if (Input.GetMouseButton(2))
        {
            mouseUpPos = instance.ScreenToWorldPoint(Input.mousePosition);
            newCameraTransform.cameraPosition = transform.localPosition + mouseDownPos - mouseUpPos;
        }
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //Key movement
            MoveDirection(Directions.Right, Input.GetAxis("Horizontal") > 0);
            MoveDirection(Directions.Left, Input.GetAxis("Horizontal") < 0);
            MoveDirection(Directions.Down, Input.GetAxis("Vertical") < 0);
            MoveDirection(Directions.Up, Input.GetAxis("Vertical") > 0);
        }
        else if (edgeMovement)
        {
            //Edge detection
            MoveDirection(Directions.Right, Input.mousePosition.x > Screen.width - cursorDetectionRange);
            MoveDirection(Directions.Left, Input.mousePosition.x < 0 + cursorDetectionRange);
            MoveDirection(Directions.Down, Input.mousePosition.y < 0 + cursorDetectionRange);
            MoveDirection(Directions.Up, Input.mousePosition.y > Screen.height - cursorDetectionRange);
        }
        void MoveDirection(Directions direction, bool isButtonPressed)
        {
            if (isButtonPressed)
            {
                isTracking = false;
                float zoomScale = instance.orthographicSize * zoomSpeedScale / zoomMax;
                switch (direction)
                {
                    case Directions.Up:
                        newCameraTransform.cameraPosition.y += cameraSpeed * zoomScale * Time.deltaTime;
                        break;
                    case Directions.Left:
                        newCameraTransform.cameraPosition.x -= cameraSpeed * zoomScale * Time.deltaTime;
                        break;
                    case Directions.Down:
                        newCameraTransform.cameraPosition.y -= cameraSpeed * zoomScale * Time.deltaTime;
                        break;
                    case Directions.Right:
                        newCameraTransform.cameraPosition.x += cameraSpeed * zoomScale * Time.deltaTime;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public void UpdatePosition()
    {
        //Apply zoom
        newCameraTransform.cameraSize = Mathf.Clamp(newCameraTransform.cameraSize, zoomMin, zoomMax);
        instance.orthographicSize = Mathf.Lerp(instance.orthographicSize, newCameraTransform.cameraSize, zoomLerpSpeed * Time.deltaTime);

        //Apply movement
        float halfScreenWidth = instance.ScreenToWorldPoint(new Vector3(instance.scaledPixelWidth, 0, 0)).x - transform.localPosition.x;
        float halfScreenHeight = instance.ScreenToWorldPoint(new Vector3(0, instance.scaledPixelHeight, 0)).y - transform.localPosition.y;
        newCameraTransform.cameraPosition.x = Mathf.Clamp(newCameraTransform.cameraPosition.x, minPos.x + halfScreenWidth, maxPos.x - halfScreenWidth);
        newCameraTransform.cameraPosition.y = Mathf.Clamp(newCameraTransform.cameraPosition.y, minPos.y + halfScreenHeight, maxPos.y - halfScreenHeight);
        newCameraTransform.cameraPosition.z = -5;

        Vector3 lerpVector = Vector3.Lerp(transform.localPosition, newCameraTransform.cameraPosition, cameraLerpSpeed * Time.deltaTime);
        transform.position = lerpVector;
    }

    #region Tracking
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
            if (Vector2.Distance(instance.transform.position, newCameraTransform.cameraPosition) < detectRange)
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
        isTracking = true;
    }
    #endregion
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
    public void SetBoundries(Vector3 min, Vector3 max)
    {
        minPos = min;
        maxPos = max;
    }
    public void SetBoundries(Transform min, Transform max)
    {
        minPos = min.position;
        maxPos = max.position;
    }
}