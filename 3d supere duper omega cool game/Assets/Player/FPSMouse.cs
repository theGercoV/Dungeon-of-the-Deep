using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

using UnityEngine;

public class FPSMouse : MonoBehaviour
{
    public float sensitivity = 200f;
    public Transform player;

    float xRotation = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
