using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwith : MonoBehaviour
{
    public GameObject Cam01, Cam02, Cam03, Cam04, Cam05, CamOrg; //六个相机，camorg对应main camera

    void Start()
    {
        //设定camorg为启用，其他为停用状态
        CamOrg.SetActive(true);
        Cam01.SetActive(false);
        Cam02.SetActive(false);
        Cam03.SetActive(false);
        Cam04.SetActive(false);
        // Cam05.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 按下tab键，依次启用相机
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (CamOrg.activeSelf == true)
            {
                OpCam01();
            }
            else if (Cam01.activeSelf == true)
            {
                OpCam02();
            }
            else if (Cam02.activeSelf == true)
            {
                OpCam03();
            }
            else if (Cam03.activeSelf == true)
            {
                OpCam04();
            }
            else if (Cam04.activeSelf == true)
            {
                OpCam05();
            }
            else if (Cam04.activeSelf == true)
            {
                OpCamOrg();
            }

        }
    }

    void OpCamOrg() //自定义的程式OpcamOrg
    {
        CamOrg.SetActive(true);
        Cam01.SetActive(false);
        Cam02.SetActive(false);
        Cam03.SetActive(false);
        Cam04.SetActive(false);
        Cam05.SetActive(false);
    }

    void OpCam01()  //自定义的程式Opcam01
    {
        CamOrg.SetActive(false);
        Cam01.SetActive(true);
        Cam02.SetActive(false);
        Cam03.SetActive(false);
        Cam04.SetActive(false);
        Cam05.SetActive(false);
    }
    void OpCam02()  //自定义的程式Opcam02
    {
        CamOrg.SetActive(false);
        Cam01.SetActive(false);
        Cam02.SetActive(true);
        Cam03.SetActive(false);
        Cam04.SetActive(false);
        Cam05.SetActive(false);
    }
    void OpCam03()  //自定义的程式Opcam03
    {
        CamOrg.SetActive(false);
        Cam01.SetActive(false);
        Cam02.SetActive(false);
        Cam03.SetActive(true);
        Cam04.SetActive(false);
        Cam05.SetActive(false);
    }
    void OpCam04()  //自定义的程式Opcam04
    {
        CamOrg.SetActive(false);
        Cam01.SetActive(false);
        Cam02.SetActive(false);
        Cam03.SetActive(false);
        Cam04.SetActive(true);
        Cam05.SetActive(false);
    }
    void OpCam05()  //自定义的程式Opcam05
    {
        CamOrg.SetActive(false);
        Cam01.SetActive(false);
        Cam02.SetActive(false);
        Cam03.SetActive(false);
        Cam04.SetActive(false);
        Cam05.SetActive(true);
    }
}
