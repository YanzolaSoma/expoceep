using UnityEngine;
using System.Collections.Generic;

public class GeradorDeMapa : MonoBehaviour
{
    [SerializeField] private GameObject prefabSalaBase;
    [SerializeField] private int maxSalas = 15;
    [Range(0f, 1f)] [SerializeField] private float chanceDeExpandir = 0.65f;

    private Dictionary<Vector2Int, Sala> salasGeradas = new Dictionary<Vector2Int, Sala>();
    private Queue<Sala> salasParaExpandir = new Queue<Sala>();
    private int contadorDeSalas = 0;

    private void Start()
    {
        GerarMapa();
    }

    private void GerarMapa()
    {
        if (prefabSalaBase == null)
        {
            Debug.LogError("Prefab da sala não está atribuído!");
            return;
        }

        // Cria a primeira sala no centro
        Sala primeira = CriarSala(Vector2Int.zero, Vector3.zero);
        salasParaExpandir.Enqueue(primeira);

        // Expande o mapa até o limite
        while (salasParaExpandir.Count > 0 && contadorDeSalas < maxSalas)
        {
            Sala atual = salasParaExpandir.Dequeue();
            Expandir(atual);
        }

        Debug.Log($"Geração concluída: {contadorDeSalas} salas criadas.");
    }

    private Sala CriarSala(Vector2Int gradePos, Vector3 mundoPos)
    {
        GameObject obj = Instantiate(prefabSalaBase, mundoPos, Quaternion.identity, transform);
        Sala nova = obj.GetComponent<Sala>();
        nova.indiceDeGrade = gradePos;

        salasGeradas.Add(gradePos, nova);
        contadorDeSalas++;
        return nova;
    }

    private void Expandir(Sala sala)
    {
        foreach (Direcao dir in System.Enum.GetValues(typeof(Direcao)))
        {
            if (contadorDeSalas >= maxSalas)
                break;

            if (Random.value > chanceDeExpandir)
                continue;

            Vector2Int novaGrade = sala.indiceDeGrade + DirecaoParaVetor(dir);
            if (salasGeradas.ContainsKey(novaGrade))
            {
                ConectarSalas(sala, dir, salasGeradas[novaGrade]);
                continue;
            }

            Vector3 offset = DirecaoParaOffset(dir, sala.tamanhoDaSala);
            Sala novaSala = CriarSala(novaGrade, sala.transform.position + offset);
            ConectarSalas(sala, dir, novaSala);
            salasParaExpandir.Enqueue(novaSala);
        }
    }

    private void ConectarSalas(Sala a, Direcao dir, Sala b)
    {
        a.MarcarSaidaConectada(dir);
        b.MarcarSaidaConectada(DirecaoOposta(dir));
    }

    private Vector2Int DirecaoParaVetor(Direcao dir)
    {
        return dir switch
        {
            Direcao.Norte => new Vector2Int(0, 1),
            Direcao.Sul => new Vector2Int(0, -1),
            Direcao.Leste => new Vector2Int(1, 0),
            Direcao.Oeste => new Vector2Int(-1, 0),
            _ => Vector2Int.zero
        };
    }

    private Vector3 DirecaoParaOffset(Direcao dir, Vector2 tamanho)
    {
        return dir switch
        {
            Direcao.Norte => new Vector3(0, 0, tamanho.y),
            Direcao.Sul => new Vector3(0, 0, -tamanho.y),
            Direcao.Leste => new Vector3(tamanho.x, 0, 0),
            Direcao.Oeste => new Vector3(-tamanho.x, 0, 0),
            _ => Vector3.zero
        };
    }

    private Direcao DirecaoOposta(Direcao dir)
    {
        return dir switch
        {
            Direcao.Norte => Direcao.Sul,
            Direcao.Sul => Direcao.Norte,
            Direcao.Leste => Direcao.Oeste,
            Direcao.Oeste => Direcao.Leste,
            _ => dir
        };
    }
}

public enum Direcao { Norte, Sul, Leste, Oeste }
