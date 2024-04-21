using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    public Transform baseTransform; // 底座变换
    public Transform endEffector; // 终端执行器变换
    public float link1Length = 0.24f; // 第一段连杆长度
    public float link2Length = 0.24f; // 第二段连杆长度

    // 计算机械臂的逆动力学
    public void CalculateInverseKinematics(Vector3 targetPosition)
    {
        // 将目标位置从世界空间转换为局部空间
        Vector3 targetLocalPosition = baseTransform.InverseTransformPoint(targetPosition);

        // 计算目标位置与底座之间的距离和方向
        float distanceToTarget = targetLocalPosition.magnitude;
        Vector3 directionToTarget = targetLocalPosition.normalized;

        // 判断是否能够到达目标位置
        if (distanceToTarget > link1Length + link2Length)
        {
            Debug.LogWarning("目标位置超出机械臂的工作范围！");
            return;
        }

        // 计算第一段连杆的角度
        float theta1 = Mathf.Atan2(directionToTarget.y, directionToTarget.x);
        float cosTheta2 = (distanceToTarget * distanceToTarget + link1Length * link1Length - link2Length * link2Length) / (2 * distanceToTarget * link1Length);
        float sinTheta2 = Mathf.Sqrt(1 - cosTheta2 * cosTheta2);
        float theta2 = Mathf.Atan2(sinTheta2, cosTheta2);

        // 根据目标位置和计算出的角度，更新机械臂节点的旋转
        baseTransform.rotation = Quaternion.Euler(0f, 0f, theta1 * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(0f, 0f, (theta1 + theta2) * Mathf.Rad2Deg);
    }

    // 在Unity中绘制调试信息
    private void OnDrawGizmos()
    {
        if (baseTransform == null || endEffector == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(baseTransform.position, endEffector.position);
    }
}
