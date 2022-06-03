using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public float ScreenEdgeBorderThickness = 5.0f; // distance from screen edge. Used for mouse movement

    [Header("Movement Speeds")]
    [Space]
    public float panSpeed = 10;
    public float secToMaxSpeed; //seconds taken to reach max speed;
    public float zoomSpeed=5;

    [Header("Movement Limits")]
    [Space]
    public bool enableMovementLimits;
    public Vector2 heightLimit;
    public Vector2 lenghtLimit;
    public Vector2 widthLimit;
    private Vector2 zoomLimit;

    private Vector3 panMovement;
    private Vector3 pos;
    private bool rotationActive = false;
    private Vector3 lastMousePosition;

    [Header("Rotation")]
    [Space]
    public bool rotationEnabled;
    public float rotateSpeed;


    // Use this for initialization
    void Start()
    {
        zoomLimit.x = 30;
        zoomLimit.y = 80;
    }


    void Update()
    {
        #region Movement

        panMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            panMovement += Vector3.forward * (panSpeed+ 100) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            panMovement -= Vector3.forward * (panSpeed + 100) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            panMovement += Vector3.left * (panSpeed + 100) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            panMovement += Vector3.right * (panSpeed + 100) * Time.deltaTime;
            //pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.forward * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= ScreenEdgeBorderThickness)
        {
            panMovement -= Vector3.forward * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.left * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.right * panSpeed * Time.deltaTime;
            //pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            panMovement += Vector3.up * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            panMovement += Vector3.down * panSpeed * Time.deltaTime;
        }

        transform.Translate(panMovement, Space.World);

        //increase pan speed
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
        //    || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
        //    || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)
        //    || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness
        //    || Input.mousePosition.y <= ScreenEdgeBorderThickness
        //    || Input.mousePosition.x <= ScreenEdgeBorderThickness
        //    || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
        //{
        //    panIncrease += Time.deltaTime / secToMaxSpeed;
        //    panSpeed = Mathf.Lerp(minPanSpeed, maxPanSpeed, panIncrease);
        //}
        //else
        //{
        //    panIncrease = 0;
        //    panSpeed = minPanSpeed;
        //}

        #endregion

        #region Zoom
        Camera.main.fieldOfView -= Input.mouseScrollDelta.y * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomLimit.x, zoomLimit.y);
        #endregion

        #region mouse rotation

        if (rotationEnabled)
        {
            // Mouse Rotation
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
            {
                rotationActive = true;
                var mouseDelta = Vector3.zero;
                if (lastMousePosition.x >= 0 
                    && lastMousePosition.y >= 0 
                    && lastMousePosition.x <= Screen.width
                    && lastMousePosition.y <= Screen.height)
                {
                    mouseDelta = Input.mousePosition - lastMousePosition;
                }

                var rotation = Vector3.up * Time.deltaTime * rotateSpeed * mouseDelta.x;
                rotation += Vector3.left * Time.deltaTime * rotateSpeed * mouseDelta.y;

                transform.Rotate(rotation, Space.World);

                // Make sure z rotation stays locked
                rotation = transform.rotation.eulerAngles;
                rotation.z = 0;
                transform.rotation = Quaternion.Euler(rotation);
            }

            lastMousePosition = Input.mousePosition;
        }
        #endregion


        #region boundaries
        if (enableMovementLimits == true)
        {
            //movement limits
            pos = transform.position;
            pos.y = Mathf.Clamp(pos.y, heightLimit.x, heightLimit.y);
            pos.z = Mathf.Clamp(pos.z, lenghtLimit.x, lenghtLimit.y);
            pos.x = Mathf.Clamp(pos.x, widthLimit.x, widthLimit.y);
            transform.position = pos;
        }
        #endregion
    }
}