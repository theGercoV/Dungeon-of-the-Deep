using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;
    public float range = 2f;
    public float cooldown = 0.5f;

    public Camera playerCamera;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + cooldown;
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position,
                            playerCamera.transform.forward,
                            out hit,
                            range))
        {
            Debug.Log("Getroffen: " + hit.transform.name);

            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
