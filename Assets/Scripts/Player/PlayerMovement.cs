using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Thông số di chuyển")]
    [SerializeField] private float speed; // Tốc độ di chuyển của nhân vật
    [SerializeField] private float jumpPower; // Lực nhảy của nhân vật

    [Header("Coyote Time (Thời gian treo trên không)")]
    [SerializeField] private float coyoteTime; // Khoảng thời gian có thể nhảy sau khi rời khỏi nền đất
    private float coyoteCounter; // Đếm thời gian đã trôi qua kể từ khi rời khỏi nền đất

    [Header("Nhảy nhiều lần")]
    [SerializeField] private int extraJumps; // Số lần nhảy bổ sung trên không
    private int jumpCounter; // Đếm số lần nhảy bổ sung còn lại

    [Header("Nhảy khi bám tường")]
    [SerializeField] private float wallJumpX; // Lực nhảy theo phương ngang khi bám tường
    [SerializeField] private float wallJumpY; // Lực nhảy theo phương dọc khi bám tường

    [Header("Lớp vật lý")]
    [SerializeField] private LayerMask groundLayer; // Lớp vật lý của mặt đất
    [SerializeField] private LayerMask wallLayer; // Lớp vật lý của tường

    [Header("Âm thanh")]
    [SerializeField] private AudioClip jumpSound; // Âm thanh nhảy

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown; // Thời gian chờ sau khi nhảy khỏi tường
    private float horizontalInput; // Giá trị nhập từ bàn phím (trái/phải)

    private void Awake()
    {
        // Lấy component Rigidbody2D, Animator và BoxCollider2D từ nhân vật
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    [System.Obsolete]
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Lấy giá trị nhập từ bàn phím (A/D hoặc phím mũi tên)

        // Xoay nhân vật khi di chuyển trái/phải
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Cập nhật trạng thái hoạt ảnh
        anim.SetBool("run", horizontalInput != 0); // Nếu có di chuyển thì bật animation chạy
        anim.SetBool("grounded", isGrounded()); // Cập nhật animation nhảy/rơi

        // Nhảy khi nhấn phím Space
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Điều chỉnh độ cao của bước nhảy (Nhả phím Space sẽ làm chậm tốc độ lên cao)
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        // Kiểm tra nếu nhân vật đang bám vào tường
        if (onWall())
        {
            body.gravityScale = 0; // Tắt trọng lực để giữ nhân vật bám tường
            body.velocity = Vector2.zero; // Dừng di chuyển rơi xuống
        }
        else
        {
            body.gravityScale = 7; // Bật lại trọng lực khi không bám tường
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); // Điều khiển di chuyển ngang

            if (isGrounded()) // Nếu nhân vật đang đứng trên đất
            {
                coyoteCounter = coyoteTime; // Reset thời gian treo trên không (coyote time)
                jumpCounter = extraJumps; // Reset số lần nhảy trên không
            }
            else
            {
                coyoteCounter -= Time.deltaTime; // Giảm thời gian coyote khi không chạm đất
            }
        }
    }

    [System.Obsolete]
    private void Jump()
    {
        // Nếu không thể nhảy (hết coyote time, không bám tường và không còn nhảy bổ sung) thì không làm gì cả
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return; 

        // Phát âm thanh nhảy
        SoundManager.instance.PlaySound(jumpSound);

        if (onWall()) // Nếu đang bám tường, thực hiện nhảy tường
            WallJump();
        else
        {
            if (isGrounded()) // Nếu đang đứng trên đất, thực hiện nhảy bình thường
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                if (coyoteCounter > 0) // Nếu vẫn còn thời gian coyote, thực hiện nhảy
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) // Nếu có lượt nhảy bổ sung, thực hiện nhảy và giảm lượt nhảy
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            // Đặt lại coyoteCounter để tránh nhảy liên tục
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        // Đẩy nhân vật ra xa khỏi tường khi nhảy
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool isGrounded()
    {
        // Kiểm tra xem nhân vật có đang đứng trên đất không bằng BoxCast
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        // Kiểm tra xem nhân vật có đang bám vào tường không bằng BoxCast
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        // Chỉ có thể tấn công khi không di chuyển, đang đứng trên đất và không bám vào tường
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
