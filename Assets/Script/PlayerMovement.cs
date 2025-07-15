using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Ditambahkan untuk TextMeshProUGUI

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f; // Belum digunakan, pertimbangkan untuk aksi "Run"
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController; // Ini adalah kelas dari Input System Actions Anda

    // Untuk input dari button UI Mobile
    private float mobileInputX = 0f;
    private bool mobileJumpPressed = false;

    private Vector2 moveInputVector; // Mengganti nama dari moveInput untuk kejelasan
    private bool isJumpingState = false; // Mengganti nama dari isJumping untuk kejelasan state

    private enum MovementState { idle, walk, run, jump, fall } // Menambahkan state "run"

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    [Header("Health System")]
    public int maxHealth = 100;
    private int currentHealth;
    public TextMeshProUGUI healthText; // Pastikan ini di-assign di Inspector

    [Header("Knockback Settings")]
    [SerializeField] private float knockBackTime = 0.2f;
    [SerializeField] private float knockBackThrust = 10f;
    private bool isKnockedBack = false;
    private float knockBackEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerController = new PlayerController(); // Inisialisasi Input Actions

        currentHealth = maxHealth; // Inisialisasi health
        UpdateHealthUI();

        
        currentHealth = maxHealth;
        UpdateHealthUI();

    }

    private void OnEnable()
    {
        playerController.Enable();

        // Menggunakan lambda untuk event handling Input System
        playerController.Movement.Move.performed += ctx => moveInputVector = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled += ctx => moveInputVector = Vector2.zero;

        playerController.Movement.Jump.performed += ctx => AttemptJump();
        // Jika ada aksi Run, tambahkan di sini:
        // playerController.Movement.Run.performed += ctx => isRunning = true;
        // playerController.Movement.Run.canceled += ctx => isRunning = false;
    }

    private void OnDisable()
    {
        playerController.Disable();
        // Penting untuk melepaskan event listener untuk menghindari error
        playerController.Movement.Move.performed -= ctx => moveInputVector = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled -= ctx => moveInputVector = Vector2.zero;
        playerController.Movement.Jump.performed -= ctx => AttemptJump();
    }

    private void Update()
    {
        if (isKnockedBack) {
            if (Time.time >= knockBackEndTime) {
                isKnockedBack = false;
            }
            return; // Jangan proses input atau animasi lain saat knockback
        }

        ProcessInputs();
        UpdateAnimation(); // Update animasi bisa di Update jika tidak terlalu bergantung fisika berat
    }

    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            return; // Biarkan knockback mengatur velocity
        }
        MovePlayer();
        
        if (isKnockedBack) return;
    }

    private void ProcessInputs()
    {
        float finalHorizontalInput;

        if (Application.isMobilePlatform)
        {
            // Prioritaskan input tombol UI mobile jika ada
            if (mobileInputX != 0f) {
                finalHorizontalInput = mobileInputX;
            }
            // Jika tidak ada input tombol UI mobile, gunakan Input System (misal untuk gamepad di mobile)
            else {
                finalHorizontalInput = playerController.Movement.Move.ReadValue<Vector2>().x;
            }

            if (mobileJumpPressed) {
                AttemptJump();
                mobileJumpPressed = false; // Reset setelah diproses
            }
        }
        else // Bukan platform mobile, gunakan Input System
        {
            finalHorizontalInput = playerController.Movement.Move.ReadValue<Vector2>().x;
        }
        moveInputVector = new Vector2(finalHorizontalInput, moveInputVector.y); // Simpan Y jika ada (misal dari input system)
    }


    private void MovePlayer()
    {
        // Implementasikan penggunaan runSpeed jika diperlukan, contoh:
        // float currentSpeed = (playerController.Movement.Run.ReadValue<float>() > 0 && isGrounded()) ? runSpeed : moveSpeed;
        float currentSpeed = moveSpeed; // Default ke moveSpeed

        rb.velocity = new Vector2(moveInputVector.x * currentSpeed, rb.velocity.y);

        // Reset isJumpingState hanya saat grounded dan velocity Y mendekati 0
        if (isGrounded() && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            isJumpingState = false;
        }
    }

    private void UpdateAnimation()
    {
        MovementState state;
        float horizontalInput = moveInputVector.x; // Gunakan input yang sudah diproses

        // Cek apakah sedang lari atau jalan (jika run diimplementasikan)
        // bool isRunningAction = playerController.Movement.Run.ReadValue<float>() > 0;

        if (horizontalInput > 0f)
        {
            // state = isRunningAction && isGrounded() ? MovementState.run : MovementState.walk;
            state = MovementState.walk; // Ubah ke run jika kondisi terpenuhi
            sprite.flipX = false;
        }
        else if (horizontalInput < 0f)
        {
            // state = isRunningAction && isGrounded() ? MovementState.run : MovementState.walk;
            state = MovementState.walk; // Ubah ke run jika kondisi terpenuhi
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        // Cek apakah sedang lompat atau jatuh
        // Prioritaskan state lompat/jatuh di atas jalan/idle
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f && !isGrounded()) // Tambahkan cek !isGrounded untuk fall yang lebih akurat
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        // Jarak raycast sedikit lebih panjang untuk deteksi yang lebih baik
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private void AttemptJump()
    {
        if (isGrounded() && !isJumpingState) // Cek juga isJumpingState untuk mencegah double jump dari input cepat
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumpingState = true; // Set state bahwa sedang dalam proses lompat
            // Animasi lompat akan dihandle oleh UpdateAnimation berdasarkan velocity.y
        }
    }

    // Fungsi untuk input tombol UI Mobile
    public void OnMoveRightPressed(bool isPressed)
    {
        if (isPressed)
            mobileInputX = 1f;
        else if (mobileInputX == 1f) // Hanya reset jika tombol ini yang menyebabkan gerakan
            mobileInputX = 0f;
    }

    public void OnMoveLeftPressed(bool isPressed)
    {
        if (isPressed)
            mobileInputX = -1f;
        else if (mobileInputX == -1f) // Hanya reset jika tombol ini yang menyebabkan gerakan
            mobileInputX = 0f;
    }

    public void OnJumpButtonPressed() // Dipanggil dari UI Button (misal OnClick)
    {
        mobileJumpPressed = true; // Set flag, akan diproses di Update
    }

    // --- Health System ---
    public void TakeDamage(int damageAmount, Vector2 knockbackDirection)
    {
        if (isKnockedBack) return; // Sudah knockback, jangan terima damage lagi sementara

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0); // Pastikan health tidak kurang dari 0
        UpdateHealthUI();
        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else // Terapkan knockback jika masih hidup
        {
            isKnockedBack = true;
            knockBackEndTime = Time.time + knockBackTime;
            rb.velocity = Vector2.zero; // Hentikan gerakan sebelumnya
            rb.AddForce(knockbackDirection.normalized * knockBackThrust, ForceMode2D.Impulse);
            // Balikkan sprite jika knockback berlawanan dengan arah hadap
            if (knockbackDirection.x > 0 && sprite.flipX) sprite.flipX = false;
            if (knockbackDirection.x < 0 && !sprite.flipX) sprite.flipX = true;
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Pastikan health tidak lebih dari maxHealth
        UpdateHealthUI();
        Debug.Log("Player healed to: " + currentHealth);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // Tambahkan logika kematian di sini (misal: animasi mati, restart level, dll)
        // Mungkin nonaktifkan movement:
        // playerController.Disable();
        // this.enabled = false; // Nonaktifkan skrip ini
    }
}