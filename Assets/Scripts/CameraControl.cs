using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 m_camRot;
    private Transform m_camTransform;
    private Transform m_transform;
    public float m_movSpeed = 1;
    public float m_rotateSpeed = 1;
    private void Start()
    {
        m_camTransform = Camera.main.transform;
        m_transform = GetComponent<Transform>();
    }
    private void Update()
    {
        Control();
    }
    void Control()
    {
        if (Input.GetMouseButton(0))
        {

            float rh = Input.GetAxis("Mouse X");
            float rv = Input.GetAxis("Mouse Y");
            //float rz = Input.GetAxis("Mouse Z");

            m_camRot.x -= rv * m_rotateSpeed;
            m_camRot.y += rh * m_rotateSpeed;
            //m_camRot.z += rz * m_rotateSpeed;

        }

        m_camTransform.eulerAngles = m_camRot;
        //m_camTransform.eulerAngles = new Vector3(m_camRot.x,m_camRot.y,0.0f);

        Vector3 camrot = m_camTransform.eulerAngles;
        //camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot;

        float xm = 0, ym = 0, zm = 0;


        if (Input.GetKey(KeyCode.I))
        {
            ///zm += m_movSpeed * Time.deltaTime;
            zm += m_movSpeed;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            ///zm -= m_movSpeed * Time.deltaTime;
            zm -= m_movSpeed;
        }

        if (Input.GetKey(KeyCode.J))
        {
            ///xm -= m_movSpeed * Time.deltaTime;
            xm -= m_movSpeed;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            ///xm += m_movSpeed * Time.deltaTime;
            xm += m_movSpeed;
        }
        if (Input.GetKey(KeyCode.U))
        {
            ///ym += m_movSpeed * Time.deltaTime;
            ym += m_movSpeed;
        }
        if (Input.GetKey(KeyCode.O))
        {
            ///ym -= m_movSpeed * Time.deltaTime;
            ym -= m_movSpeed;
        }
        m_transform.Translate(new Vector3(xm, ym, zm), Space.Self);
    }
}
