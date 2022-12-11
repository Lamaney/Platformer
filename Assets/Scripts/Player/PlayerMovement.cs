using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private float horizontal;
        private bool isFacingRight = true;
      
        
        
        [FormerlySerializedAs("runnigSpeed")]
        [Header("Movement Parameters")]
        [SerializeField]  private float runningSpeed ;
        [SerializeField]  private float jumpingPower ;

        [Header("Needed Components")]
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Animator animator;


        // Update is called once per frame
        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            IsRunning();
            Jump();
            IsJumping();
            IsFalling();
            Flip();
        }

        private void FixedUpdate()
        {
            rigidbody2D.velocity = new Vector2(horizontal * runningSpeed, rigidbody2D.velocity.y);
        }


        private void Jump()
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpingPower);
            }

            if (Input.GetButtonUp("Jump") && rigidbody2D.velocity.y > 0f)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.5f);
            }
        }


        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void Flip()
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }



        private void IsRunning() => animator.SetFloat("Speed", Mathf.Abs(horizontal));

        private void IsJumping()
        {
            animator.SetBool("IsJumping", !IsGrounded());
        }

        private void IsFalling()
        {
            animator.SetBool("IsFalling", rigidbody2D.velocity.y < 0);
        }
    }
}