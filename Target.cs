using UnityEngine;

public class Target : MonoBehaviour
{
    // 得分
    public int score = 1; 
    // 是否为运动靶子
    public bool isSportsTarget;
    // 靶子的位置点
    private Transform point;
    // 靶子的索引
    public int indexTarget;

    // 音效组件
    private AudioSource audioSource;
    // 击中靶子的音效
    public AudioClip hitSound;

    // 是否已经被射中
    private bool isHit = false;

    // 刚体组件
    private Rigidbody rb;

    private void Start()
    {
        // 获取靶子的父对象作为位置点
        point = transform.parent;

        // 获取刚体组件
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            // 如果靶子上没有刚体组件，则添加一个
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // 确保刚体组件的 isKinematic 选项启用
        rb.isKinematic = true;

        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 如果靶子上没有 AudioSource 组件，则添加一个
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isHit)
        {
            // 标记靶子已经被射中
            isHit = true;

            // 播放击中音效
            PlayHitSound();

            // 调用函数计算得分
            CalculateScore();

            // 让靶子向后上方跳一下
            JumpBackAndFall(collision);

            // 禁用靶子的碰撞检测
            GetComponent<Collider>().enabled = false;

            // 禁用靶子的移动（如果靶子有移动脚本）
            TargetMove targetMove = GetComponent<TargetMove>();
            if (targetMove != null)
            {
                targetMove.enabled = false;
            }
        }
    }

    private void CalculateScore()
    {
        // 如果靶子的标签为 Bullseye，即为靶心
        if (gameObject.tag == "Bullseye")
        {
            // 如果为运动靶子
            if (isSportsTarget)
            {
                // 将得分加 10
                Tips.Instance.SetScore(10);
                Tips.Instance.SetText("在" + indexTarget + "号射击位上射中" + indexTarget + "号靶子,加10分");
            }
            else
            {
                // 如果不是运动靶子，将得分加 8
                Tips.Instance.SetScore(8);
                Tips.Instance.SetText("在" + indexTarget + "号射击位上射中" + indexTarget + "号靶子,加8分");
            }
        }
        // 如果靶子的标签为 Circle，即为白色区域
        else if (gameObject.tag == "Circle")
        {
            // 如果为运动靶子
            if (isSportsTarget)
            {
                // 将得分加 5
                Tips.Instance.SetScore(5);
                Tips.Instance.SetText("在" + indexTarget + "号射击位上射中" + indexTarget + "号靶子,加5分");
            }
            else
            {
                // 如果不是运动靶子，就将得分加 3
                Tips.Instance.SetScore(3);
                Tips.Instance.SetText("在" + indexTarget + "号射击位上射中" + indexTarget + "号靶子,加3分");
            }
        }
    }

    // 让靶子向后上方跳一下然后坠落
    private void JumpBackAndFall(Collision collision)
    {
        if (rb != null)
        {
            // 禁用 isKinematic，让刚体受到物理引擎的影响
            rb.isKinematic = false;

            // 施加一个向后上方的力
            Vector3 jumpDirection = new Vector3(-1f, 1f, 0f).normalized;
            rb.AddForce(jumpDirection * 10f, ForceMode.Impulse); // 增加力的大小

            // 启用重力，让靶子坠落
            rb.useGravity = true;
        }
    }

    // 播放击中音效的方法
    private void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}