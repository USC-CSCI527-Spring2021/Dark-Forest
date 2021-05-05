using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using System;

public class AgentDemo : Agent
{
    public int id;
    // public override void Initialize()
    // {
    //     base.Initialize();
    //     Debug.Log(transform.gameObject.name);
    //     id = int.Parse(transform.gameObject.name.Split('_')[1]);
    //     transform.parent.GetComponent<BehaviorParameters>().TeamId = id;
    // }

    public bool dead = false;
    public override void OnEpisodeBegin()
    {
        // NUll: 不用做任何的事情
        // 从csv读取位置数据
        dead = false;
        id = int.Parse(transform.gameObject.name.Split('_')[1]);
        transform.GetComponent<BehaviorParameters>().TeamId = id;
        GameDataMgr dataMgr = GameDataMgr.GetInstance();

        //Debug.Log($"Start : {GameDataMgr.GetInstance().playerInfos[id].posX}");
        GameDataMgr.GetInstance().end = true;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // 形成一个四维向量: (posX, posY, edible, drinkable)
        //sensor.AddObservation(transform.position);
        // String Oname = transform.gameObject.name;
        id = int.Parse(transform.gameObject.name.Split('_')[1]);

        float posX = (float)GameDataMgr.GetInstance().playerInfos[id].posX;
        float posY = (float)GameDataMgr.GetInstance().playerInfos[id].posY;
        sensor.AddObservation(posX);
        sensor.AddObservation(posY);
        sensor.AddObservation((float)GameDataMgr.GetInstance().playerInfos[id].health);
        sensor.AddObservation((float)GameDataMgr.GetInstance().playerInfos[id].thirsty);
        sensor.AddObservation((float)GameDataMgr.GetInstance().playerInfos[id].starving);
        int[,] ret = GameDataMgr.GetInstance().getPlayerVision(id, posX, posY);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                sensor.AddObservation((float)ret[i, j]);
            }
        }

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        // ContinuousActions-> DiscreteActions
        // output 一个七维向量: (w,a,s,d,k,o,j)
        id = int.Parse(transform.gameObject.name.Split('_')[1]);
        int[] A = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        A[actions.DiscreteActions[0]] = 1;
        int moveLeft = A[0];
        int moveRight = A[1];
        int moveUp = A[2];
        int moveDown = A[3];
        int drink = A[4];
        int eat = A[5];
        int attack = A[6];
        // Debug.Log($"{moveLeft}, {moveRight}, {moveUp}, {moveDown}, {drink}, {eat}");


        if (!GameDataMgr.GetInstance().playerInfos[id].alive)
        {
            if (!GameDataMgr.GetInstance().playerInfos[id].dead)
            {
                GameDataMgr.GetInstance().playerInfos[id].dead = true;
                SetReward(-5f);
                // Debug.Log($"Death of {id}");
                dead = true;
                GameObject player = GameObject.Find("player_" + id.ToString());
                player.transform.localScale = new Vector3(0, 0, 0);
                double aggressivity = 0f, averageStarveOnEat = 0f, averageThirstyOnDrink = 0f, averageHealthOnAttack = 0f;
                int live = 0;
                foreach (int id in GameDataMgr.GetInstance().playerInfos.Keys)
                {
                    if (!GameDataMgr.GetInstance().playerInfos[id].alive) continue;
                    live++;
                    aggressivity += GameDataMgr.GetInstance().playerInfos[id].aggressivity;
                    averageStarveOnEat += GameDataMgr.GetInstance().playerInfos[id].averageStarveOnEat;
                    averageThirstyOnDrink += GameDataMgr.GetInstance().playerInfos[id].averageThirstyOnDrink;
                    averageHealthOnAttack += GameDataMgr.GetInstance().playerInfos[id].averageHealthOnAttack;
                }
                aggressivity = aggressivity / (float)live;
                averageStarveOnEat = averageStarveOnEat / (float)live;
                averageThirstyOnDrink = averageThirstyOnDrink / (float)live;
                averageHealthOnAttack = averageHealthOnAttack / (float)live;
                Debug.Log($"Death {id}, Agg{aggressivity}, soe{averageStarveOnEat}, tod{averageThirstyOnDrink}, hoa{averageHealthOnAttack}");
            }
        }
        else
        {
            float reward = ((float)Math.Min(GameDataMgr.GetInstance().playerInfos[id].thirsty, GameDataMgr.GetInstance().playerInfos[id].starving) - 50f) / 50f;
            SetReward(reward);
        }
        // GameDataMgr.GetInstance().deathCounter[id] += 1;
        int cnt = 0;
        foreach (int id in GameDataMgr.GetInstance().playerInfos.Keys)
        {
            if (GameDataMgr.GetInstance().playerInfos[id].alive)
            {
                cnt++;
            }
        }

        if (cnt < 1)
        {
            GameDataMgr.GetInstance().restart = true;
            // while(GameDataMgr.GetInstance().end){
            // }
            foreach (int id in GameDataMgr.GetInstance().playerInfos.Keys)
            {
                GameDataMgr.GetInstance().playerRestart(id);
                GameObject player = GameObject.Find("player_" + id.ToString());
                player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            EndEpisode();
        }

        float posX = (float)GameDataMgr.GetInstance().playerInfos[id].posX;
        float posY = (float)GameDataMgr.GetInstance().playerInfos[id].posY;
        double nx = posX + moveDown - moveUp;
        double ny = posY - moveLeft + moveRight;
        if (isAttackable((int)posX, (int)posY))
        {
            GameDataMgr.GetInstance().playerInfos[id].canAttackCount += 1;
        }
        if (drink == 1)
        {
            GameDataMgr.GetInstance().playerDrink(id, nx, ny);
        }
        if (eat == 1)
        {
            GameDataMgr.GetInstance().playerEat(id, nx, ny);
        }
        if (attack == 1)
        {
            GameDataMgr.GetInstance().playerAttack(id, posX, posY);
            // Debug.Log($"Attack, {id}");
        }
        GameDataMgr.GetInstance().updatePlayerPos(id, (float)nx, (float)ny);

    }

    public bool isAttackable(int posX, int posY)
    {
        GameDataMgr mgr = GameDataMgr.GetInstance();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    int nx = posX + i;
                    int ny = posY + j;
                    if (nx < 0 || nx >= mgr.row || ny < 0 || ny >= mgr.col || mgr.map[(int)nx, (int)ny] != 0)
                    {
                        continue;
                    }
                    else if (mgr.hasPeople(nx, ny) != -1)
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }
}

