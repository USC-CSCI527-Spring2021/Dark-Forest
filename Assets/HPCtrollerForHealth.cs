using System.Collections;
using System.Collections.Generic;
// using static System.Math;
using System;
using UnityEngine;
using UnityEngine.UI;  //Slider的调用需要引用UI源文件

public class HPCtrollerForHealth : MonoBehaviour
{
    public Slider HEALTH;  //实例化一个Slider
    private float thirsty;
    private float starving;
    public GameDataMgr gameDataMgr;
    public HPCtrollerForStarving HPCtrollerForStarving;
    public HPCtrollerForThirsty HPCtrollerForThirsty;

    private String Oname;
	private GameObject obj;
	private int id;
    
    private void Start()
    {
        HEALTH.value = 100;
    }

    void Update()
    {
        obj = this.transform.parent.parent.gameObject;
		Oname = obj.name;
        
        GameDataMgr mgr = GameDataMgr.GetInstance();
        Oname = Oname.Split('_')[1];
		id = int.Parse(Oname);
        thirsty = (float)mgr.playerInfos[id].thirsty;
        starving = (float)mgr.playerInfos[id].starving;
        if (thirsty == 0 && starving != 0){
            // HEALTH.value -= Time.deltaTime / 1f;  
            // GameDataMgr.GetInstance().playerInfos[0].health = (float)HEALTH.value;
            mgr.playerInfos[id].health-= Time.deltaTime / 1f;
            mgr.playerInfos[id].health = Math.Max(0,mgr.playerInfos[id].health); 
            HEALTH.value = (float)mgr.playerInfos[id].health;
        }
        if (thirsty != 0 && starving != 0) {
            // HEALTH.value = 100;
            // GameDataMgr.GetInstance().playerInfos[0].health = (float)HEALTH.value;
            if(thirsty>=70 && starving>=70){
                mgr.playerInfos[id].health+= Time.deltaTime / 0.2f;
                if(mgr.playerInfos[id].health>100){
                    mgr.playerInfos[id].health = 100;
                }
            }
             
            HEALTH.value = (float)mgr.playerInfos[id].health;
        }
        if (thirsty != 0 && starving == 0){
            // HEALTH.value -= Time.deltaTime / 0.6f;  
            // GameDataMgr.GetInstance().playerInfos[0].health = (float)HEALTH.value;
            mgr.playerInfos[id].health-= Time.deltaTime / 0.6f;
             mgr.playerInfos[id].health = Math.Max(0,mgr.playerInfos[id].health);
            HEALTH.value = (float)mgr.playerInfos[id].health;
        }
        if (thirsty == 0 && starving == 0){
            // HEALTH.value -= Time.deltaTime / 0.3f;  
            // GameDataMgr.GetInstance().playerInfos[0].health = (float)HEALTH.value;
            mgr.playerInfos[id].health-= Time.deltaTime / 0.3f; 
            mgr.playerInfos[id].health = Math.Max(0,mgr.playerInfos[id].health);
            HEALTH.value = (float)mgr.playerInfos[id].health;
        }
        if(mgr.playerInfos[id].health <= 0){
            GameDataMgr.GetInstance().playerInfos[id].alive = false;
        }
        // Debug.Log($"{thirsty},{starving}");
        // Debug.Log(HEALTH.value);
    }
}