using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody GGravity;
    Camera playerCam;

    Vector2 camRotation;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 1.5f;
    public bool sprintMode = false;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;

    [Header("User Settings")]
    public bool sprintToggleOption = false;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2f;
    public float Ysensitivity = 2f;
    public float camRotationLimit = 90f;
   

    // Start is called before the first frame update
    void Start()
    {
        GGravity = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        Vector3 temp = GGravity.velocity;

        float verticalMove = Input.GetAxisRaw("Vertical");
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        if (!sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;
            if (Input.GetKeyUp(KeyCode.LeftShift))
                sprintMode = false;
        }

        if (sprintToggleOption)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && verticalMove > 0)
                sprintMode |= true;
            if (verticalMove <= 0)
                sprintMode |= false;
        }

        if (!sprintMode)
            temp.x = verticalMove * speed;

        if (sprintMode)
            temp.x = verticalMove * speed * sprintMultiplier;

      
        {
            
        }

        temp.z = horizontalMove * speed;

        GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right);

        if(Input.GetKey(KeyCode.LeftShift))
            temp.x *= sprintMultiplier;

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            temp.y = jumpHeight;

        GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);
    }
}
