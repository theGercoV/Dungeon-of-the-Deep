using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
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
    public float regenDelay = 5f;
    public float regenSpeed = 2f;
    private float currentStamina;
    private float idleTimer = 0f;
    public Image sprintBarFill;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    public Image healthBarFill;

    [Header("Health Regen")]
    public float regenDelayAfterHit = 5f;
    public float regenInterval = 1f;
    public int regenAmount = 2;

    private float healthRegenTimer = 0f;
    private float lastDamageTime;

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

        // Test Damage Taste
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(10);
    }

    void FixedUpdate()
    {
        Move();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.forward * v + transform.right * h;
        move.Normalize();

        bool sprinting = Input.GetKey(KeyCode.LeftShift) &&
                         (v != 0 || h != 0) &&
                         currentStamina > 0;

        float speed = sprinting ? sprintSpeed : walkSpeed;

        Vector3 velocity = move * speed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

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

    // ---------- STAMINA ----------
    void HandleStamina()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool sprinting = Input.GetKey(KeyCode.LeftShift) && (v != 0 || h != 0);

        if (sprinting && currentStamina > 0)
        {
            currentStamina -= Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= regenDelay && currentStamina < maxStamina)
            {
                currentStamina += regenSpeed * Time.deltaTime;
                if (currentStamina > maxStamina)
                    currentStamina = maxStamina;
            }
        }

        UpdateUI();
    }

    // ---------- HEALTH REGEN ----------
    void HandleHealthRegen()
    {
        if (Time.time - lastDamageTime < regenDelayAfterHit)
            return;

        healthRegenTimer += Time.deltaTime;

        if (healthRegenTimer >= regenInterval)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthRegenTimer = 0f;
            UpdateUI();
        }
    }

    // ---------- DAMAGE ----------
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        lastDamageTime = Time.time;
        healthRegenTimer = 0f;

        UpdateUI();

        if (currentHealth <= 0)
            Debug.Log("Player Dead");
    }

    // ---------- UI ----------
    void UpdateUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;

        if (sprintBarFill != null)
            sprintBarFill.fillAmount = currentStamina / maxStamina;
    }
}
