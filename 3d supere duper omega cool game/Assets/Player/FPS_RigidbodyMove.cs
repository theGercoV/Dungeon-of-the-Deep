using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FPS_RigidbodyMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 6f;
    public float mouseSens = 200f;

    Rigidbody rb;
    Camera cam;
    float xRot = 0f;
    bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Maus
        float mx = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float my = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRot -= my;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        transform.Rotate(Vector3.up * mx);

        // Springen
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float x = 0;
        float z = 0;

        if (Input.GetKey(KeyCode.W)) z = 1;
        if (Input.GetKey(KeyCode.S)) z = -1;
        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;

        Vector3 move = transform.forward * z + transform.right * x;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
            grounded = false;
    }
}
