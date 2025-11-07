using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variável para o corpo rígido da bala
    private Rigidbody2D rb;

    // Tempo máximo que a bala pode existir (para não poluir a cena)
    public float lifespan = 2f; 
    
    // Variável para a quantidade de dano que esta bala causa
    [SerializeField]
    private int dano = 1; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("O componente Rigidbody2D está faltando na bala.");
            enabled = false;
        }
    }

    // Chamado pelo PlayerShooting para iniciar o movimento
    public void Initialize(Vector2 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifespan);
    }

    // Lida com a colisão da bala (o Collider2D da bala DEVE ser 'Is Trigger')
    private void OnTriggerEnter2D(Collider2D other)
    {
        // NOVO: 1. Filtra a tag "Player". Se for o jogador, a bala é ignorada.
        if (other.CompareTag("Player"))
        {
            // Se a bala atingir o jogador, simplesmente ignora e não faz nada.
            return;
        }
        
        // 2. Verifica se colidiu com um objeto com a tag "Inimigo"
        if (other.CompareTag("Inimigo"))
        {
            // Tenta encontrar o script de vida no objeto colidido
            SistemaDeVida vidaDoAlvo = other.GetComponent<SistemaDeVida>();

            // Se o alvo tiver o componente de vida, aplica o dano
            if (vidaDoAlvo != null)
            {
                vidaDoAlvo.ReceberDano(dano);
            }
            
            // Destrói a bala após atingir o alvo
            DestruirProjetil();
        }
        
        // 3. Verifica se colidiu com uma parede ou obstáculo
        // (Destrói a bala se for uma parede ou qualquer Collider que não seja 'Is Trigger')
        else if (other.CompareTag("Wall") || !other.isTrigger) 
        {
            DestruirProjetil();
        }
    }

    private void DestruirProjetil()
    {
        // Adicionar lógica para som/efeitos visuais de impacto aqui
        Destroy(gameObject);
    }
}