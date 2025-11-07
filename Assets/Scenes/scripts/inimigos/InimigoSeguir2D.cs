using UnityEngine;

// Garante que este script SÓ pode ser adicionado a um GameObject
// que já tenha um Rigidbody2D.
[RequireComponent(typeof(Rigidbody2D))]
public class InimigoSeguir2D : MonoBehaviour
{
    // --- Variáveis de Configuração (Aparecem no Inspector) ---

    [SerializeField]
    private Transform alvoDoJogador;

    [SerializeField]
    private float velocidade = 2.5f;

    [SerializeField]
    private float distanciaParaParar = 1.0f;

    [SerializeField]
    private bool olharParaOJogador = true;

    // Se o seu sprite de inimigo não está "virado para cima" por padrão,
    // ajuste este valor. (Ex: se ele aponta para a "direita", use 0).
    [SerializeField]
    private float anguloDeCorrecaoSprite = -90.0f;

    // --- Componentes ---
    private Rigidbody2D rb;

    // --- Lógica Interna ---

    void Start()
    {
        // Pega o componente Rigidbody2D que está no mesmo GameObject.
        rb = GetComponent<Rigidbody2D>();

        // Importante: Para um jogo top-down, não queremos que a gravidade
        // afete o inimigo.
        rb.gravityScale = 0;

        // Se o alvo não foi definido no Inspector, procura pela tag "Player".
        if (alvoDoJogador == null)
        {
            GameObject jogador = GameObject.FindGameObjectWithTag("Player");
            if (jogador != null)
            {
                alvoDoJogador = jogador.transform;
            }
            else
            {
                Debug.LogError("Inimigo 2D não conseguiu encontrar o jogador. " +
                               "Verifique a tag 'Player' ou arraste o jogador para o campo 'Alvo'.");
                enabled = false;
            }
        }
    }

    // FixedUpdate é o local correto para mexer com física (Rigidbodies),
    // pois ele roda em sincronia com o motor de física.
    void FixedUpdate()
    {
        if (alvoDoJogador == null)
        {
            // Se o jogador for destruído, para de se mover.
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // --- Lógica Principal de "Seguir" ---

        // 1. Obter posições (convertemos Vector3 para Vector2)
        Vector2 posicaoAlvo = alvoDoJogador.position;
        Vector2 posicaoAtual = rb.position; // Usamos rb.position para física

        // 2. Calcular a distância
        float distancia = Vector2.Distance(posicaoAlvo, posicaoAtual);

        // 3. Verificar se estamos longe o suficiente para mover
        if (distancia > distanciaParaParar)
        {
            // --- Movimento ---
            // Calcula o vetor de direção (um vetor de comprimento 1)
            Vector2 direcao = (posicaoAlvo - posicaoAtual).normalized;

            // Calcula a nova posição para onde devemos mover
            // Usamos Time.fixedDeltaTime por estarmos no FixedUpdate
            Vector2 novaPosicao = posicaoAtual + direcao * velocidade * Time.fixedDeltaTime;
            
            // Move o Rigidbody para a nova posição.
            // Isso é melhor que rb.velocity para um "follow" preciso
            // e que respeita colisões.
            rb.MovePosition(novaPosicao);


            // --- Rotação (Opcional) ---
            if (olharParaOJogador)
            {
                // Calcula o ângulo em radianos e converte para graus
                float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

                // Aplica a correção do sprite e define a rotação do Rigidbody
                rb.rotation = angulo + anguloDeCorrecaoSprite;
            }
        }
        else
        {
            // Estamos perto o suficiente, então paramos.
            // (Importante se o inimigo tiver "impulso")
            rb.linearVelocity = Vector2.zero;
            
            // Aqui você poderia ativar a lógica de ATAQUE.
        }
    }
}