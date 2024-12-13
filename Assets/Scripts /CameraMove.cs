using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // 鼠标x轴灵敏度
    public float mouseXSensitivity = 25f;
    // 人物
    private Transform player;
    // 旋转角度
    float xRotation = 0f;

    // 主摄像机
    private Camera mainCamera;
    // 鸟瞰图摄像机
    public Camera birdsEyeCamera;

    // 鸟瞰图摄像机的初始偏移量（相对于主摄像机）
    public Vector3 birdsEyeOffset = new Vector3(0f, 50000000f, 0f); // 提高高度

    // 鸟瞰图摄像机的移动速度
    public float birdsEyeMoveSpeed = 1000000f; // 移动速度

    private void Start()
    {
        player = transform.parent.transform;
        mainCamera = GetComponent<Camera>();

        // 确保鸟瞰图摄像机默认是关闭的
        if (birdsEyeCamera != null)
        {
            birdsEyeCamera.enabled = false;
        }
    }

    void Update()
    {
        // 按下鼠标右键切换摄像机
        if (Input.GetMouseButtonDown(1))
        {
            SwitchCamera();
        }

        // 如果当前是主摄像机，则允许鼠标控制视角
        if (mainCamera.enabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -45f, 10f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }

        // 如果当前是鸟瞰图摄像机，则使用 IKJL 键移动
        if (birdsEyeCamera.enabled)
        {
            MoveBirdsEyeCameraWithIKJL();
        }
    }

    // 切换摄像机
    private void SwitchCamera()
    {
        if (birdsEyeCamera != null)
        {
            mainCamera.enabled = !mainCamera.enabled;
            birdsEyeCamera.enabled = !birdsEyeCamera.enabled;

            // 切换到鸟瞰图摄像机时，设置其位置
            if (birdsEyeCamera.enabled)
            {
                birdsEyeCamera.transform.position = mainCamera.transform.position + birdsEyeOffset;
                birdsEyeCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // 俯视角度
            }
        }
    }

    // 使用 IKJL 键移动鸟瞰图摄像机
    private void MoveBirdsEyeCameraWithIKJL()
    {
        // 获取 IKJL 键的输入
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.I)) // 上（前进）
        {
            moveZ = 1f;
        }
        else if (Input.GetKey(KeyCode.K)) // 下（后退）
        {
            moveZ = -1f;
        }

        if (Input.GetKey(KeyCode.J)) // 左（左移）
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.L)) // 右（右移）
        {
            moveX = 1f;
        }

        // 计算移动方向
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // 移动鸟瞰图摄像机
        birdsEyeCamera.transform.position += moveDirection * birdsEyeMoveSpeed * Time.deltaTime;
    }
}
