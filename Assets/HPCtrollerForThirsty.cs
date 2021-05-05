using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;  //Slider的调用需要引用UI源文件

public class HPCtrollerForThirsty : MonoBehaviour
{
    public Slider THIRSTY;  //实例化一个Slider
    public GameDataMgr gameDataMgr;

    private String Oname;
	private GameObject obj;
	private int id;

    private void Start()
    {
        //Value的值介于0-1之间，且为浮点数
        THIRSTY.value = 100;
    }

    void Update()
    {
        obj = this.transform.parent.parent.gameObject;
		Oname = obj.name;
        //THIRSTY.value -= Time.deltaTime / 0.3f;
        //GameDataMgr.GetInstance().playerInfos[0].thirsty=(float)THIRSTY.value;
        Oname = Oname.Split('_')[1];
		id = int.Parse(Oname);
        if(!GameDataMgr.GetInstance().playerInfos[id].alive){
            GameDataMgr.GetInstance().playerInfos[id].thirsty = 0;
            return;
        }
        GameDataMgr.GetInstance().playerInfos[id].thirsty -= Time.deltaTime / 0.3f;
        if(GameDataMgr.GetInstance().playerInfos[id].thirsty<0){
            GameDataMgr.GetInstance().playerInfos[id].thirsty = 0;
        }
        THIRSTY.value = (float)GameDataMgr.GetInstance().playerInfos[id].thirsty;
    }
}