using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 人物控制器
    private CharacterController controller;
    // 人物移动速度
    public float speed = 5f;
    public float gravity = -15f;
    public float jumpHeight = 2f; // 跳跃高度
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        // 检测是否在地面上
        isGrounded = controller.isGrounded;

        // 键盘输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // 应用重力
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 确保玩家在地面上时速度为负值
        }

        velocity.y += gravity * Time.deltaTime;

        // 检测跳跃
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        controller.Move(velocity * Time.deltaTime);
    }
}