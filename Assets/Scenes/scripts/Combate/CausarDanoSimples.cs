using UnityEngine;

// CausarDanoSimples.cs
// Garante que o objeto tenha um Collider para que a colisão funcione
[RequireComponent(typeof(Collider2D))]
public class CausarDanoSimples : MonoBehaviour
{
    // --- Configurações ---
    [Tooltip("A quantidade de dano a ser aplicada.")]
    [SerializeField]
    private int danoACausar = 10;

    [Tooltip("A tag do alvo que este objeto DEVE atingir.")]
    [SerializeField]
    private string tagDoAlvo = "Inimigo";

    [Tooltip("Se for um projétil ou ataque rápido, marque para que seja destruído após o impacto.")]
    [SerializeField]
    private bool destruirAposImpacto = true;
    
    // --- Lógica de Colisão 2D ---
    
    // Usamos OnTriggerEnter2D para colisões sem empurrão físico.
    // O Collider2D deste objeto (o que está causando o dano) deve estar marcado como "Is Trigger".
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Verifica se o objeto que colidiu tem a tag correta
        if (other.CompareTag(tagDoAlvo))
        {
            // 2. Tenta encontrar o script de vida no objeto colidido
            // Nota: Se você não usou "SistemaDeVida.cs", substitua pelo nome do seu script de vida.
            SistemaDeVida vidaDoAlvo = other.GetComponent<SistemaDeVida>();

            // 3. Se o alvo tiver o componente de vida, aplica o dano
            if (vidaDoAlvo != null)
            {
                vidaDoAlvo.ReceberDano(danoACausar);
                Debug.Log(gameObject.name + " causou " + danoACausar + " de dano em " + other.name);
                
                // 4. Se for para destruir, destrói o objeto que causou o dano
                if (destruirAposImpacto)
                {
                    DestruirEsteObjeto();
                }
            }
        }
        
        // Opcional: Destruir a bala se ela atingir algo que não seja o alvo (como uma parede)
        // Certifique-se de que a parede NÃO tem a tag do alvo e NÃO é um trigger.
        else if (destruirAposImpacto && !other.isTrigger) 
        {
            DestruirEsteObjeto();
        }
    }
    
    private void DestruirEsteObjeto()
    {
        // Aqui você pode adicionar lógica para som/efeito de impacto antes de destruir.
        Destroy(gameObject);
    }
}