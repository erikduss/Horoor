using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    //initial speed
    public int speed = 10;
    public Camera mainCamera;
    public MouseLook m_MouseLook;

    // Use this for initialization
    void Start()
    {
        m_MouseLook.Init(transform.parent.transform, mainCamera.transform);
    }

    // Update is called once per frame
    void Update()
    {
        m_MouseLook.LookRotation(transform.parent.transform, mainCamera.transform);
        //press shift to move faster
        /*if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = 20;

        }
        else
        {
            //if shift is not pressed, reset to default speed
            speed = 10;
        }
        //For the following 'if statements' don't include 'else if', so that the user can press multiple buttons at the same time
        //move camera to the left
        if (Input.GetKey(KeyCode.A))
        {
            transform.parent.transform.position = transform.position + Camera.main.transform.right * -1 * speed * Time.deltaTime;
        }

        //move camera backwards
        if (Input.GetKey(KeyCode.S))
        {
            transform.parent.transform.position = transform.position + Camera.main.transform.forward * -1 * speed * Time.deltaTime;

        }
        //move camera to the right
        if (Input.GetKey(KeyCode.D))
        {
            transform.parent.transform.position = transform.position + Camera.main.transform.right * speed * Time.deltaTime;

        }
        //move camera forward
        if (Input.GetKey(KeyCode.W))
        {

            transform.parent.transform.position = transform.position + Camera.main.transform.forward * speed * Time.deltaTime;
        }
        //move camera upwards
        if (Input.GetKey(KeyCode.E))
        {
            transform.parent.transform.position = transform.position + Camera.main.transform.up * speed * Time.deltaTime;
        }
        //move camera downwards
        if (Input.GetKey(KeyCode.Q))
        {
            transform.parent.transform.position = transform.position + Camera.main.transform.up * -1 * speed * Time.deltaTime;
        }*/

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if(hit.collider.transform.parent.tag == "SoundSource")
                    {
                        Destroy(hit.collider.transform.parent.gameObject);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }
}
