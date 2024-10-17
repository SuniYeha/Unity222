using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody GGravity;
    Camera playerCam;
    public Transform weaponSlot;
    public AudioSource weaponSpeaker;
    public AudioClip pyu;
    public AudioClip pyu2;
    public GameManager gm;

    Vector2 camRotation;

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 3;
    public int Heal = 1;

    [Header("Weapon Stats")]
    public GameObject fists;
    public GameObject boxthathits;
    public Transform panefhc;
    public GameObject bullet;
    public int weaponID = -1;
    public int fireMode = 0;
    public float bulletLifespan = .2f;
    public float BulletSpeed = 1500f;
    public float fireRate = .5f;
    public float maxAmmo = 10;
    public float currentAmmo = 10;
    public float reloadAmt = 5;
    public bool canfire = true;
    public float BAM = 7;

    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public bool sprintMode = false;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;
    public float stamina = 100f;
    public float maxstamina = 100f;
    public bool cansprint = true;
    public float sprintCD = 2f;

    [Header("User Settings")]
    public bool sprintToggleOption = true;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2f;
    public float Ysensitivity = 2f;
    public float camRotationLimit = 90f;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        GGravity = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsPaused)
        {
            camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

            playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
            transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);


            Vector3 temp = GGravity.velocity;

            float verticalMove = Input.GetAxisRaw("Vertical");
            float horizontalMove = Input.GetAxisRaw("Horizontal");


            if (stamina <= 0)
            {
                sprintMode = false;
                cansprint = false;
            }
            if (stamina > maxstamina)
                stamina = maxstamina;

            if (!sprintToggleOption)
            {
                if (Input.GetKey(KeyCode.LeftShift) && cansprint)
                    sprintMode = true;
                if (Input.GetKeyUp(KeyCode.LeftShift))
                    sprintMode = false;
            }

            if (sprintToggleOption)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && verticalMove > 0 && cansprint && !sprintMode)
                {
                    sprintMode = true;
                }
                else if (verticalMove <= 0 || (Input.GetKeyDown(KeyCode.LeftShift) && sprintMode))
                {
                    sprintMode = false;
                }
            }

            if (!sprintMode)
                temp.x = verticalMove * speed;

            if (sprintMode)
            {
                temp.x = verticalMove * speed * sprintMultiplier;
            }

            {

            }

            temp.z = horizontalMove * speed;

            GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right);

            if (Input.GetKey(KeyCode.LeftShift))
                temp.x *= sprintMultiplier;

            if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
                temp.y = jumpHeight;

            GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

            //punch system and stfff be here
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (weaponID == 0) 
                { 
                    fists.SetActive(false);
                    weaponID = -1;
                }
                else if (weaponID >= 1)
                {
                    weaponSlot.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                    weaponSlot.GetChild(0).SetParent(null);
                    fists.SetActive(true);
                    weaponID = 0;
                }
                else
                {
                    fists.SetActive(true);
                    weaponID = 0;
                }
            }
            if (weaponID >= 0 && Input.GetMouseButton(0) && canfire)
            {
                if (weaponID == 0)
                {
                    GameObject hb = Instantiate(boxthathits, panefhc.position, panefhc.rotation);
                    hb.transform.SetParent(panefhc);
                    Destroy(hb, bulletLifespan);
                    canfire = false;
                    StartCoroutine("Cooldown");
                }
                if (weaponID >= 1 && currentAmmo > 0)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        weaponSpeaker.clip = pyu;
                    }
                    else
                    {
                        weaponSpeaker.clip = pyu2;
                    }
                    weaponSpeaker.Play();
                    GameObject b = Instantiate(bullet, weaponSlot.position, weaponSlot.rotation);
                    b.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * BulletSpeed);
                    canfire = false;
                    currentAmmo--;
                    StartCoroutine("Cooldown");
                }
            }
        }

    }

    private void OnTriggerEnter(UnityEngine.Collider collision)
    {
        if ((health < maxHealth) && collision.gameObject.tag == "Healitem")
        {
            health += Heal;
            if (health > maxHealth)
                health = maxHealth;

            Destroy(collision.gameObject);
        }
        if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "Ammo")
        {
            currentAmmo += reloadAmt;
            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
       if (collision.gameObject.tag == "weapon")
        {
            if (weaponID >= 1)
            {
                weaponSlot.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                //fuvk my life bruh
                weaponSlot.GetChild(0).SetParent(null);
            }
            if (weaponID == 0)
            {
                fists.SetActive(false);
            }

            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            collision.gameObject.transform.position = weaponSlot.position;
            collision.gameObject.transform.rotation = weaponSlot.rotation;

            collision.gameObject.transform.SetParent(weaponSlot);

            weaponSpeaker = collision.gameObject.GetComponent<AudioSource>();

            switch (collision.gameObject.name)
            {
                case "weapon1":
                    weaponID = 1;
                    fireMode = 0;
                    fireRate = 0.5f;
                    maxAmmo = 10;
                    reloadAmt = 10;
                    if (currentAmmo > maxAmmo)
                        currentAmmo = maxAmmo;
                    break;

                case "weapon2":
                    weaponID = 2;
                    fireMode = 0;
                    fireRate = 0.5f;
                    maxAmmo = 100; 
                    currentAmmo = 100;
                    reloadAmt = 100;
                    break;


                    

                default:
                    break;
            }
        }
      
    }



    IEnumerator Cooldown()
    {
        
        yield return new WaitForSeconds(fireRate);
        canfire = true;

    }
}
