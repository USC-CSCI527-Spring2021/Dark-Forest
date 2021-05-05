using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MovePeople : MonoBehaviour
{

    //人物移动
    private int State;
    private int oldState = 0;
    public GameDataMgr gameDataMgr;

    private Animator ani;
    private const int Att1 = 1;
    private const int Att2 = 2;
    private double x_position;
    private double z_position;

    private bool can_attack;
    private String Oname;
    private GameObject obj;
    private int id;

    //public Transform target;
    public float speed = 1;

    void Start()
    {
        ani = GetComponent<Animator>();
    }


    void Update()
    {
        obj = this.gameObject;
        Oname = obj.name;

        // drink and eat
        x_position = transform.position.x;
        z_position = transform.position.z;
        //Debug.Log(x_position);

        if (Input.GetKeyUp("r"))
        {
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);
            if (!GameDataMgr.GetInstance().playerInfos[id].alive)
            {
                GameDataMgr.GetInstance().playerInfos[id].health = 100;
                GameDataMgr.GetInstance().playerInfos[id].thirsty = 100;
                GameDataMgr.GetInstance().playerInfos[id].starving = 100;
                GameDataMgr.GetInstance().playerInfos[id].alive = true;
            }
        }


        if (Input.GetKeyUp("n"))
        {
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);
            x_position = GameDataMgr.GetInstance().playerInfos[id].posX;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY;
            GameDataMgr.GetInstance().playerDrink(id, x_position, z_position);
        }

        if (Input.GetKeyUp("m"))
        {
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);
            x_position = GameDataMgr.GetInstance().playerInfos[id].posX;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY;
            GameDataMgr.GetInstance().playerEat(id, x_position, z_position);
        }

        // move people 
        if (Input.GetKeyUp("w"))
        {
            //setState(UP);
            //Debug.Log("move w");
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);
            x_position = GameDataMgr.GetInstance().playerInfos[id].posX - 1;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY;
            // x_position = transform.position.x;
            // z_position = transform.position.z-1;


            //print(GameDataMgr.GetInstance().playerInfos[id].posX);
            GameDataMgr.GetInstance().updatePlayerPos(id, x_position, z_position);
            //print(GameDataMgr.GetInstance().playerInfos[id].posX);
        }

        else if (Input.GetKeyUp("s"))
        {
            //setState(DOWN);
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);

            // x_position = transform.position.x;
            // z_position = transform.position.z+1;

            x_position = GameDataMgr.GetInstance().playerInfos[id].posX + 1;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY;
            GameDataMgr.GetInstance().updatePlayerPos(id, x_position, z_position);
        }

        else if (Input.GetKeyUp("a"))
        {
            //setState(LEFT);
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);

            // x_position = transform.position.x-1;
            // z_position = transform.position.z;

            x_position = GameDataMgr.GetInstance().playerInfos[id].posX;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY - 1;
            GameDataMgr.GetInstance().updatePlayerPos(id, x_position, z_position);
        }
        else if (Input.GetKeyUp("d"))
        {
            //setState(RIGHT);
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);

            // x_position = transform.position.x+1;
            // z_position = transform.position.z;

            x_position = GameDataMgr.GetInstance().playerInfos[id].posX;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY + 1;

            GameDataMgr.GetInstance().updatePlayerPos(id, x_position, z_position);
        }

        else if (Input.GetKeyUp("b"))
        {
            can_attack = false;

            // Attack eight positions, if eight positions has people attack them 
            Oname = Oname.Split('_')[1];
            id = int.Parse(Oname);

            // People's position
            x_position = GameDataMgr.GetInstance().playerInfos[id].posX;
            z_position = GameDataMgr.GetInstance().playerInfos[id].posY;

            GameDataMgr.GetInstance().playerAttack(id, x_position, z_position);

        }


        else
        {
            ani.SetInteger("states", Att2);
        }

    }

    public int hasPeople(int posX, int posY)
    {
        Debug.Log(String.Format("Attack poeple {0:D},{1:D}", posX, posY));
        Debug.Log(String.Format("Attacked poeple {0:D},{1:D}", (int)GameDataMgr.GetInstance().playerInfos[0].posX, (int)GameDataMgr.GetInstance().playerInfos[0].posY));
        foreach (int id in GameDataMgr.GetInstance().playerInfos.Keys)
        {
            if ((int)GameDataMgr.GetInstance().playerInfos[id].posX == posX && (int)GameDataMgr.GetInstance().playerInfos[id].posY == posY)
            {
                return id;
            }
        }
        return -1;
    }

    void setState(int currState)
    {
        ani.SetInteger("states", Att1);
        Vector3 transformValue = new Vector3();
        int rotateValue = (currState - State) * 90;
        //transform.GetComponent<Animation>().Play("walk");
        switch (currState)
        {
            case 0:
                transformValue = Vector3.forward * Time.deltaTime * speed;
                break;
            case 1:
                transformValue = Vector3.right * Time.deltaTime * speed;
                break;
            case 2:
                transformValue = Vector3.back * Time.deltaTime * speed;
                break;
            case 3:
                transformValue = Vector3.left * Time.deltaTime * speed;
                break;
        }
        transform.Rotate(Vector3.up, rotateValue);
        transform.Translate(transformValue, Space.World);
        oldState = State;
        State = currState;
    }

}
