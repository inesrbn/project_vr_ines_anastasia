using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovementRotation : MonoBehaviour
{
    public float rotationSpeed = 120f;
    public float moveSpeed = 3f;
    public AudioClip footstepLoopClip;

    [Header("Crisis")]
    public bool canMove = false; // ❌ par défaut : crise 1

    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.useGravity = true;


        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Préparer l’audio source
        audioSource.clip = footstepLoopClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Déterminer le mouvement selon la crise
        int crisisID = PlayerPrefs.GetInt("WarCrisisID", 0);
        canMove = (crisisID == 2);
    }

    void FixedUpdate()
    {
        float rotateInput = Input.GetAxis("Horizontal");
        float moveInput = Input.GetAxis("Vertical");

        // Rotation
        if (Mathf.Abs(rotateInput) > 0.1f)
            transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.fixedDeltaTime);

        // Déplacement seulement si autorisé
        Vector3 move = Vector3.zero;
        bool isWalking = false;

        if (canMove && Mathf.Abs(moveInput) > 0.01f)
        {
            move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
            isWalking = true;
        }

        // Animation
        if (animator != null)
            animator.SetBool("isWalking", isWalking);

        // Sons de pas
        if (audioSource != null)
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
