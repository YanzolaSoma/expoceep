using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 15f;
    public float fireRate = 0.3f;
    
    private float nextFireTime = 0f;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 shootDirection = Vector2.zero;

        // Lendo o Input das Setas
        if (Input.GetKey(KeyCode.UpArrow))
        {
            shootDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            shootDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            shootDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            shootDirection = Vector2.right;
        }

        // === LÓGICA DE ESPELHAR BASEADA NO TIRO (PRIORIDADE) ===
        // Se atirar para a esquerda ou direita, o sprite deve virar
        if (spriteRenderer != null && shootDirection.x != 0)
        {
            if (shootDirection.x < 0) // Esquerda
            {
                spriteRenderer.flipX = true;
            }
            else // Direita
            {
                spriteRenderer.flipX = false;
            }
        }
        // ===========================================

        // LÓGICA DE DISPARO
        if (shootDirection != Vector2.zero && Time.time > nextFireTime)
        {
            Shoot(shootDirection);
            nextFireTime = Time.time + fireRate;
        }
    }

    // ... (Método Shoot(Vector2 direction) permanece o mesmo) ...
    void Shoot(Vector2 direction)
    {
        // Adiciona um pequeno offset na direção do tiro para evitar o "flicker"
        Vector3 spawnOffset = direction * 0.15f; 
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(direction, bulletSpeed);
        }
    }
}