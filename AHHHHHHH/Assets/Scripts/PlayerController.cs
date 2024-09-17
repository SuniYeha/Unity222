using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody GGravity;
    Camera playerCam;
    Transform weaponSlot;

    Vector2 camRotation;

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 3;
    public int Heal = 1;

    [Header("Weapon Stats")]
    public int weaponID = -1;
    public int fireMode = 0;
    public float fireRate = .5f;
    public float maxAmmo = 10;
    public float currentAmmo = 0;
    public float reloadAmt = 5;
    public float currentClip = 0;
    public float clipSize = 0;
    public bool canfire = true;

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
        print(health);
    }

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        if (Input.GetMouseButton(0) && canfire && currentClip > 0)
        {
           canfire = false;
            currentClip--;
            StartCoroutine("cooldownFire");
        }

        if (Input.GetKey(KeyCode.R))
            reloadClip();

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

        if (Input.GetKey(KeyCode.LeftShift))
            temp.x *= sprintMultiplier;

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            temp.y = jumpHeight;

        GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);
    }

    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        if ((health < maxHealth) && collision.gameObject.tag == "Healitem")
        {
            health += Heal;
            if (health > maxHealth)
                health = maxHealth;

            Destroy(collision.gameObject);
            print(health);
        }
        if (collision.gameObject.tag == "weapon")
        {
            collision.gameObject.transform.position = weaponSlot.position;

            collision.gameObject.transform.SetParent(weaponSlot);

            switch (collision.gameObject.name)
            {
                case "weapon1":
                    weaponID = -1;
                    fireMode = 0;
                    fireRate = 0;
                    maxAmmo = 0;
                    currentAmmo = 0;
                    reloadAmt = 0;
                    currentClip = 0;
                    clipSize = 0;
                    break;

                default:
                    break;
            }
        }
            if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "Ammo")
            {
                currentAmmo += reloadAmt;
                if (currentAmmo > maxAmmo)
                    currentAmmo = maxAmmo;

                Destroy(collision.gameObject);
                print(currentAmmo);
        }
    }

    public void reloadClip()
    {
        if (currentClip >= clipSize)
            return;

        else
        {
            float reloadCount = clipSize - currentClip;

            if (currentAmmo < reloadCount)
            {
                currentClip += currentAmmo;

                currentAmmo = 0;
                return;
            }

            else
            {
                currentClip += reloadCount;

                currentAmmo -= reloadCount;
                return;
            }
        }
    }
      


    IEnumerable Cooldown()
    {
        yield return new WaitForSeconds(fireRate);
        canfire = true;
    }
}
