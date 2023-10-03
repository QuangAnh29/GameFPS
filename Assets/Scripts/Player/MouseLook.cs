
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    /*public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation;*/
    private bool isMouseLeftPressed = false;

    // Tốc độ xoay của camera
    public float mouseSensitivity = 100f;

    // Góc quay tối đa lên xuống (trên là 90 độ, dưới là -90 độ)
    public float verticalLookLimit = 90f;

    // Lưu giữ góc quay thực tế của camera
    private float verticalRotation = 0f;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isMouseLeftPressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseLeftPressed = false;
        }
        /*float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") *  mouseSensitivity;

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation,-80, 80);
        transform.localRotation = Quaternion.Euler(xRotation, 0 , 0);

        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);*/


        if (isMouseLeftPressed)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Xoay ngang toàn bộ player theo trục y
            transform.Rotate(Vector3.up * mouseX);

            // Tính góc quay thực tế lên xuống
            verticalRotation -= mouseY;
            // Giới hạn góc quay lên xuống
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

            // Xoay camera theo trục x (quay lên xuống)
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }
}
