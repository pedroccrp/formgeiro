using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public static bool canControl;

    public bool right = false;

    public static GameObject player;

    private SpriteRenderer rend;
    private BoxCollider2D col;

    private Animator animator;

    public static float moveDir;

    private bool wantsToClip = false;

    public float moveSpeed, maxRotation;
    
    private void Start()
    {
        player = this.gameObject;

        canControl = true;

        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canControl)
        {
            GetInput();
        }

        if (moveDir != 0)
        {
            AudioManager.Play("walk");
            animator.SetBool("moving", true);
        }
        else
        {
            AudioManager.Stop("walk");
            animator.SetBool("moving", false);
        }
    }

    private void FixedUpdate()
    {
        Move();

        UpdateSprite();

        ClipToTop();
    }

    private void UpdateSprite()
    {
        if (moveDir > 0)
        {
            rend.flipX = right;
        }
        else if (moveDir < 0)
        {
            rend.flipX = !right;
        }
    }

    private void GetInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        wantsToClip = Input.GetButtonDown("Jump");
    }

    private void Move()
    {
        RaycastHit2D leftHitInfo, rightHitInfo;
        
        float speed = moveSpeed * moveDir;

        float originOffsetX = (rend.size.x / 2) * (col.size.x / 2) * transform.localScale.x;
        float originOffsetY = (rend.size.y / 2) * (col.size.y / 2) * transform.localScale.y;

        DoubleRaycastDown(out leftHitInfo, out rightHitInfo, originOffsetX, originOffsetY, speed);

        AdjustRays(ref leftHitInfo, ref rightHitInfo, originOffsetX, originOffsetY);

        if (leftHitInfo && rightHitInfo)
        {
            PositionOnTerrain(leftHitInfo, rightHitInfo, maxRotation, originOffsetY);
        }
    }

    private void ClipToTop()
    {
        float originOffsetX = (rend.size.x / 2) * (col.size.x / 2) * transform.localScale.x;
        float originOffsetY = (rend.size.y / 2) * (col.size.y / 2) * transform.localScale.y;

        float rayLength = 3f;
        
        if (wantsToClip)
        {
            if (!(Physics2D.Raycast(transform.position + originOffsetY * transform.up
            + originOffsetX * transform.right, transform.up, rayLength, barrierMask) && 
            Physics2D.Raycast(transform.position + originOffsetY * transform.up
            - originOffsetX * transform.right, transform.up, rayLength, barrierMask)))
            {
                RaycastHit2D leftHitInfo = Physics2D.Raycast(transform.position + originOffsetY * transform.up
                    + originOffsetX * transform.right, transform.up, rayLength, groundMask);
                RaycastHit2D rightHitInfo = Physics2D.Raycast(transform.position + originOffsetY * transform.up
                    - originOffsetX * transform.right, transform.up, rayLength, groundMask);

                if (leftHitInfo && rightHitInfo)
                {
                    PositionOnTerrain(leftHitInfo, rightHitInfo, 1000, originOffsetY);
                }
            }        
        }
    }

    public LayerMask groundMask;
    public LayerMask barrierMask;

    void PositionOnTerrain(RaycastHit2D leftHitInfo, RaycastHit2D rightHitInfo, float maxRotationDegrees, float positionOffsetY)
    {
        Vector3 averageNormal = (leftHitInfo.normal + rightHitInfo.normal) / 2;
        Vector3 averagePoint = (leftHitInfo.point + rightHitInfo.point) / 2;

        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, averageNormal);
        Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationDegrees);

        transform.rotation = Quaternion.Euler(0, 0, finalRotation.eulerAngles.z);

        transform.position = averagePoint + transform.up * positionOffsetY;
    }

    bool DoubleRaycastDown(out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo, float originOffsetX, float originOffsetY, float speed)
    {
        float rayLength = 1f;

        Vector3 nextPosition = transform.position + (transform.right * speed);

        Vector3 transformUp = transform.up;
        Vector3 transformRight = transform.right;
        
        if (!(Physics2D.Raycast(nextPosition + originOffsetY * transformUp
            + originOffsetX * transformRight, -transformUp, rayLength, barrierMask) && Physics2D.Raycast(nextPosition + originOffsetY * transformUp
            - originOffsetX * transformRight, -transformUp, rayLength, barrierMask)))
        {
            leftHitInfo = Physics2D.Raycast(nextPosition + originOffsetY * transformUp
                + originOffsetX * transformRight, -transformUp, rayLength, groundMask);
            rightHitInfo = Physics2D.Raycast(nextPosition + originOffsetY * transformUp
                - originOffsetX * transformRight, -transformUp, rayLength, groundMask);

            return leftHitInfo && rightHitInfo;
        }
        else
        {
            leftHitInfo = rightHitInfo = new RaycastHit2D();

            return false;
        }
    }

    private void AdjustRays(ref RaycastHit2D leftHitInfo, ref RaycastHit2D rightHitInfo, float originOffsetX, float originOffsetY)
    {
        float rayLength = 0.1f;

        if (moveDir > 0)
        {
            RaycastHit2D overrideLeftHitInfo;
            
            overrideLeftHitInfo = Physics2D.Raycast(transform.position + originOffsetY * transform.up + transform.right * originOffsetX, 
                transform.right, rayLength, groundMask);

            if (overrideLeftHitInfo)
            {
                leftHitInfo = overrideLeftHitInfo;
            }
        }
        else if (moveDir < 0)
        {
            RaycastHit2D overrideRightHitInfo;
            
            overrideRightHitInfo = Physics2D.Raycast(transform.position + originOffsetY * transform.up + transform.right * originOffsetX,
                transform.right, rayLength, groundMask);

            if (overrideRightHitInfo)
            {
                rightHitInfo = overrideRightHitInfo;
            }
        }
    }
}
