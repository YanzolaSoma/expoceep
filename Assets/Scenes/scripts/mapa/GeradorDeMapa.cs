using UnityEngine;
using System.Collections.Generic;

public class GeradorDeMapa : MonoBehaviour
{
    // O Prefab da sua sala base (com o script Sala.cs anexado)
    [SerializeField]
    private GameObject prefabSalaBase;

    // Rastreia onde as salas já foram colocadas
    private Dictionary<Vector2, Sala> salasGeradas = new Dictionary<Vector2, Sala>();

    // Fila de salas para serem processadas (onde podemos adicionar novos vizinhos)
    private Queue<Sala> salasParaExpandir = new Queue<Sala>();

    [Tooltip("O número máximo de salas a serem geradas.")]
    [SerializeField]
    private int maxSalas = 10;
    
    private int contadorDeSalas;

    void Start()
    {
        GerarMapaInicial();
    }

    private void GerarMapaInicial()
    {
        if (prefabSalaBase == null)
        {
            Debug.LogError("O Prefab da Sala não está definido!");
            return;
        }

        // 1. Cria a primeira sala (no centro do mundo)
        Sala primeiraSala = Instantiate(prefabSalaBase, Vector3.zero, Quaternion.identity, transform).GetComponent<Sala>();
        primeiraSala.indiceDeGrade = Vector2.zero;
        salasGeradas.Add(Vector2.zero, primeiraSala);
        salasParaExpandir.Enqueue(primeiraSala);
        contadorDeSalas = 1;

        // 2. Continua gerando até atingir o limite
        while (salasParaExpandir.Count > 0 && contadorDeSalas < maxSalas)
        {
            Sala salaAtual = salasParaExpandir.Dequeue();
            ExpandirSala(salaAtual);
        }

        Debug.Log("Geração de mapa concluída. Total de salas: " + contadorDeSalas);
    }

    private void ExpandirSala(Sala salaDeReferencia)
    {
        // Tenta gerar vizinhos em todas as 4 direções (Norte, Sul, Leste, Oeste)
        foreach (var saida in salaDeReferencia.saidas)
        {
            if (!saida.estaConectada && contadorDeSalas < maxSalas)
            {
                Vector2 novaPosicaoGrade = Vector2.zero;
                Vector3 offset = Vector3.zero;
                
                // Calcula o índice e o deslocamento da nova sala na grade
                if (saida.direcao == "Norte")
                {
                    novaPosicaoGrade = salaDeReferencia.indiceDeGrade + new Vector2(0, 1);
                    offset = new Vector3(0, 0, salaDeReferencia.tamanhoDaSala.y);
                }
                else if (saida.direcao == "Sul")
                {
                    novaPosicaoGrade = salaDeReferencia.indiceDeGrade + new Vector2(0, -1);
                    offset = new Vector3(0, 0, -salaDeReferencia.tamanhoDaSala.y);
                }
                else if (saida.direcao == "Leste")
                {
                    novaPosicaoGrade = salaDeReferencia.indiceDeGrade + new Vector2(1, 0);
                    offset = new Vector3(salaDeReferencia.tamanhoDaSala.x, 0, 0);
                }
                else if (saida.direcao == "Oeste")
                {
                    novaPosicaoGrade = salaDeReferencia.indiceDeGrade + new Vector2(-1, 0);
                    offset = new Vector3(-salaDeReferencia.tamanhoDaSala.x, 0, 0);
                }
                
                // 1. Verifica se a posição da grade já está ocupada
                if (!salasGeradas.ContainsKey(novaPosicaoGrade))
                {
                    // 2. Gera a nova sala
                    Vector3 novaPosicaoMundo = salaDeReferencia.transform.position + offset;
                    Sala novaSala = Instantiate(prefabSalaBase, novaPosicaoMundo, Quaternion.identity, transform).GetComponent<Sala>();
                    
                    novaSala.indiceDeGrade = novaPosicaoGrade;
                    salasGeradas.Add(novaPosicaoGrade, novaSala);
                    salasParaExpandir.Enqueue(novaSala);
                    contadorDeSalas++;
                    
                    // Lógica para marcar as portas como conectadas
                    ConectarSalas(salaDeReferencia, saida.direcao, novaSala);
                }
                else
                {
                    // Se já existir uma sala vizinha, apenas conecta as portas
                    Sala salaExistente = salasGeradas[novaPosicaoGrade];
                    ConectarSalas(salaDeReferencia, saida.direcao, salaExistente);
                }
            }
        }
    }

    // Marca as portas como conectadas (você implementaria a lógica visual/ativação aqui)
    private void ConectarSalas(Sala sala1, string direcao1, Sala sala2)
    {
        // Marca a saída da Sala 1 como conectada
        for (int i = 0; i < sala1.saidas.Count; i++)
        {
            if (sala1.saidas[i].direcao == direcao1)
            {
                var temp = sala1.saidas[i];
                temp.estaConectada = true;
                sala1.saidas[i] = temp;
                
                // Exemplo: Destruir a parede/porta fechada no pontoDeConexao
                // Destroy(temp.pontoDeConexao.GetComponentInChildren<Collider2D>().gameObject); 
                break;
            }
        }
        
        // Marca a saída da Sala 2 como conectada (na direção oposta)
        string direcaoOposta = GetDirecaoOposta(direcao1);
        for (int i = 0; i < sala2.saidas.Count; i++)
        {
            if (sala2.saidas[i].direcao == direcaoOposta)
            {
                var temp = sala2.saidas[i];
                temp.estaConectada = true;
                sala2.saidas[i] = temp;
                break;
            }
        }
    }

    private string GetDirecaoOposta(string direcao)
    {
        if (direcao == "Norte") return "Sul";
        if (direcao == "Sul") return "Norte";
        if (direcao == "Leste") return "Oeste";
        if (direcao == "Oeste") return "Leste";
        return "";
    }
}