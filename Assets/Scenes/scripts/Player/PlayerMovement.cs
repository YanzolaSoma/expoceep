using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Adicionado de volta

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        // === LÓGICA DE MOVIMENTO WASD ===
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }

        movement.x = moveX;
        movement.y = moveY;
        
        movement.Normalize();
        // ===================================
        
        // === LÓGICA DE ESPELHAR (SE MOVIMENTO) ===
        // Só aplicamos a virada se o script de tiro não estiver no controle
        
        // Verificamos se NENHUMA tecla de tiro está pressionada (prioridade)
        bool isShootingHorizontally = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
        
        if (!isShootingHorizontally && spriteRenderer != null)
        {
            if (movement.x > 0) // Se movendo para a direita E NÃO atirando horizontalmente
            {
                spriteRenderer.flipX = false;
            }
            else if (movement.x < 0) // Se movendo para a esquerda E NÃO atirando horizontalmente
            {
                spriteRenderer.flipX = true;
            }
        }
        // =======================================
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}