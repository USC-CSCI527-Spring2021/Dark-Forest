using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    // 脚本使用在摄像机身上
    public class CameraFollow : MonoBehaviour
    {
        public Transform player; // 主角位置
        public float speed = 10f; // 相机速度   
        public Vector3 distance; // 主角和摄像机之间的距离                   


        void Start ()
        {
            // 计算人物与摄像机之间的向量
            // 用当前摄像机的坐标 - 玩家的坐标（可以画一张图来算一算）
            distance = transform.position - player.position;
        }


        void FixedUpdate ()
        {
            // 摄像机应该在的位置
            // 不直接赋值给当前摄像机的原因是，需要这个参数来实现一个延迟功能
            Vector3 targetCamPos = player.position + distance;

            // 给摄像机移动到应该在的位置的过程中加上延迟效果
            transform.position = Vector3.Lerp (transform.position, targetCamPos, speed * Time.deltaTime);
        }
    }
}
