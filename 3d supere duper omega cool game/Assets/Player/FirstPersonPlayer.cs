using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController_HealthRegen : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float jumpForce = 5f;
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;

    [Header("Stamina")]
    public float maxStamina = 10f;
    public float cooldownTime = 5f; // Zeit nach leerer Stamina bevor Auffüllen startet
    private float currentStamina;
    private float cooldownTimer;
    private bool isRefilling = false;
    public Image sprintBarFill;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public Image healthBarFill;

    [Header("Health Regen")]
    public float regenInterval = 10f; // jede 10 Sekunden
    public int regenAmount = 10;
    private float regenTimer = 0f;

    private Rigidbody rb;
    private float xRotation;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        currentStamina = maxStamina;
        currentHealth = maxHealth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateUI();
    }

    void Update()
    {
        MouseLook();
        Jump();
        HandleStamina();
        HandleHealthRegen();

        // Test Damage
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(10);
    }

    void FixedUpdate()
    {
        Move();
    }

    // --- MOUSE LOOK ---
    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    // --- MOVE ---
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.forward * v + transform.right * h;
        move.Normalize();

        bool sprinting = Input.GetKey(KeyCode.LeftShift) &&
                         (v != 0 || h != 0) &&
                         currentStamina > 0 &&
                         !isRefilling;

        float speed = sprinting ? sprintSpeed : walkSpeed;

        Vector3 velocity = move * speed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    // --- JUMP ---
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    // --- STAMINA ---
    void HandleStamina()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool sprinting = Input.GetKey(KeyCode.LeftShift) && (v != 0 || h != 0);

        if (sprinting && !isRefilling)
        {
            currentStamina -= Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isRefilling = true;
                cooldownTimer = 0f;
            }
        }

        if (isRefilling)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer < cooldownTime)
            {
                currentStamina = 0;
            }
            else if (cooldownTimer >= cooldownTime && cooldownTimer <= cooldownTime * 2)
            {
                float fillProgress = (cooldownTimer - cooldownTime) / cooldownTime;
                currentStamina = Mathf.Lerp(0, maxStamina, fillProgress);
            }
            else
            {
                currentStamina = maxStamina;
                isRefilling = false;
                cooldownTimer = 0f;
            }
        }

        UpdateUI();
    }

    // --- HEALTH REGEN ---
    void HandleHealthRegen()
    {
        regenTimer += Time.deltaTime;

        if (regenTimer >= regenInterval)
        {
            currentHealth += regenAmount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            regenTimer = 0f;
            UpdateUI();
        }
    }

    // --- HEALTH ---
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0)
            Debug.Log("Player Dead");
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;

        if (sprintBarFill != null)
            sprintBarFill.fillAmount = currentStamina / maxStamina;
    }
}
