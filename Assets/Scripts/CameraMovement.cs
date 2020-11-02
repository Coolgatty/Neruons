using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 rotation = new Vector2(0, 0);
    public float speed = 3;
    public float moveSpeed = 0.2f;
    public Button button;
    void Start()
    {
        Cursor.visible = false;
        button.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        transform.eulerAngles = (Vector2)rotation * speed;

        transform.position += (transform.forward * Input.GetAxis("Vertical") + transform.up * (Input.GetAxis("Jump") + Input.GetAxis("Down")) + transform.right * Input.GetAxis("Horizontal"))*moveSpeed;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            if(button.gameObject.activeSelf)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    public static void Exit()
    {
        Application.Quit();
    }
}
