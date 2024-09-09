using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody GGravity;

    public float speed = 10.0f;
    public float jumpHeight = 1;

    // Start is called before the first frame update
    void Start()
    {
        GGravity = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = GGravity.velocity;

        temp.x = Input.GetAxisRaw("Vertical") * speed;
        temp.z = Input.GetAxisRaw("Horizontal") * speed;

        GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right);

        if(Input.GetKeyDown(KeyCode.Space))
            temp.y = jumpHeight;

        GGravity.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);
    }
}
