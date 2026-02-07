using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Bewegung")]
    public float speed = 3f;            // Bewegungsgeschwindigkeit
    public float chaseDistance = 10f;   // Distanz, ab der Enemy den Spieler verfolgt

    [Header("Angriff")]
    public float attackDistance = 2f;   // Abstand zum Angriff
    public float attackCooldown = 1.5f; // Zeit zwischen Angriffen
    public int attackDamage = 10;       // Schaden pro Angriff

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        // Spieler automatisch finden (Tag "Player" muss gesetzt sein)
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogError("Kein Spieler gefunden! Tag 'Player' fehlt.");
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Spieler verfolgen
        if (dist < chaseDistance && dist > attackDistance)
        {
            MoveTowards(player.position);
        }

        // Angriff
        if (dist <= attackDistance && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // Blickrichtung fixen
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
    }

    void AttackPlayer()
    {
        Debug.Log("Enemy greift an! Schaden: " + attackDamage);

        // Player TakeDamage aufrufen
        player.GetComponent<FirstPersonController_HealthRegen>()?.TakeDamage(attackDamage);
    }
}
