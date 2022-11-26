using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlipAnimator;

public class Player : MonoBehaviour
{
    public Vector2 collisionOrigin;
    public Vector2 collisionSize;

    public Vector2 velocity;

    public float speed = 10.0f;
    public float friction = 1.0f;

    public float jumpHeight = 2.5f;
    public float jumpTime = 1.0f;
    public float fallGravityIncrease = 1.5f;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private float _jumpBuffer = 0.0f;
    private float _fallBuffer = 0.0f;
    private Hook _hook;

    public FlipAnimationSequence walk;
    public FlipAnimationSequence idle;
    public FlipAnimationSequence jumpUp;
    public FlipAnimationSequence jumpDown;
    public FlipAnimationPlayer player;
    public ParticleSystem particlSystem;

    void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _hook = GetComponentInChildren<Hook>();
        velocity = Vector2.zero;
        player.OnFrameUpdate += (x) => { if (x.events.Length > 0) { particlSystem.Play(); } };
    }

    // Update is called once per frame
    void Update()
    {
        if (_hook.Firing)
        {
            velocity = Vector2.zero;
        }
        else
        {

            float gravity = 2.0f * jumpHeight / (jumpTime * jumpTime);
            float jumpForce = 2.0f * jumpHeight / jumpTime;

            bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            bool jump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
            bool holdJump = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);

            if (jump)
                _jumpBuffer = 0.075f;

            bool onGround = IsHit(Physics2D.OverlapBox(
                new Vector2(transform.position.x, transform.position.y - 0.498f),
                new Vector2(0.5f, 0.05f),
                0.0f
            ));

            bool hitHead = IsHit(Physics2D.OverlapBox(
                new Vector2(transform.position.x, transform.position.y + 0.498f),
                new Vector2(0.5f, 0.05f),
                0.0f
            ));

            if (hitHead && velocity.y > 0.0f)
                velocity.y = 0.0f;

            if (onGround)
            {
                _fallBuffer = 0.1f;
            }

            int hor = (right ? 1 : 0) - (left ? 1 : 0);
            Vector2 acceleration;
            acceleration.x = hor * speed * friction;
            acceleration.x -= velocity.x * friction;

            acceleration.y = -gravity;
            if (onGround)
            {
                velocity.y = 0.0f;

                if (velocity.y < 0.0f)
                    acceleration.y *= fallGravityIncrease;

                acceleration.y = 0.0f;
            }

            if (onGround || _fallBuffer > 0.0f)
            {
                if (_jumpBuffer > 0.0f || holdJump)
                {
                    velocity.y = jumpForce;
                    _fallBuffer = 0.0f;
                }
            }

            _jumpBuffer -= Time.deltaTime;
            _fallBuffer -= Time.deltaTime;
            velocity += acceleration * Time.deltaTime;

            if (onGround)
            {
                if (left || right)
                {
                    PlayAnimation(walk);
                }
                else
                {
                    PlayAnimation(idle);
                }
            }
            else
            {
                if (velocity.y > 0.0f)
                {
                    PlayAnimation(jumpUp);
                }
                else
                {
                    PlayAnimation(jumpDown);
                }
            }

            if(hor == -1)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else if(hor == 1)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }

        _rigidBody.velocity = velocity;
        transform.position = new Vector3(transform.position.x, transform.position.y, 2.0f);
    }

    private bool IsHit(Collider2D collider)
    {
        return !(collider == null || collider == _collider);
    }
    private void PlayAnimation(FlipAnimationSequence sequence)
    {
        if (player.Animation == null || player.Animation.Sequence != sequence)
        {
            player.Animation = new FlipAnimation(sequence, 0);
        }
    }
}
