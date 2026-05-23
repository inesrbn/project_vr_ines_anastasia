using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovementRigidbody : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 120f;
    public AudioClip footstepLoopClip;

    private Animator animator;
    private Rigidbody rb;
    private AudioSource audioSource;

    private Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        rb.freezeRotation = true;

        audioSource.clip = footstepLoopClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveInput.y += 1;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveInput.y -= 1;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput.x -= 1;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput.x += 1;

        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        float moveZ = moveInput.y;
        float rotate = moveInput.x;

        // Rotation
        if (Mathf.Abs(rotate) > 0.1f)
        {
            transform.Rotate(Vector3.up * rotate * rotationSpeed * Time.fixedDeltaTime);
        }

        // Déplacement
        Vector3 moveDirection = transform.forward * moveZ;
        Vector3 velocity = moveDirection * moveSpeed;

        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        // Animation
        bool isWalking = Mathf.Abs(moveZ) > 0.1f;

        if (animator != null)
            animator.SetBool("isWalking", isWalking);

        // Audio footsteps
        if (audioSource != null && footstepLoopClip != null)
        {
            if (isWalking)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();
            }
        }
    }
}