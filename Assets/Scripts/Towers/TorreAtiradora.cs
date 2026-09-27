using UnityEngine;

public class TorreAtiradora : MonoBehaviour
{
    [Header("Atributos da Torre")]
    public float alcance = 3f;           
    public float tempoEntreTiros = 1f;   
    public int danoDeAtaque = 2;         
    public float velocidadeDoTiro = 20f; // Nova variável modular no Inspector

    [Header("Configurações")]
    public GameObject prefabProjetil;    
    public Transform pontoDeDisparo;     
    public string tagInimigo = "Inimigo"; 

    private float timerTiro = 0f;

    void Update()
    {
        timerTiro += Time.deltaTime;

        if (timerTiro >= tempoEntreTiros)
        {
            Transform alvo = EncontrarAlvoMaisProximo();

            if (alvo != null)
            {
                Atirar(alvo);
                timerTiro = 0f; 
            }
        }
    }

    Transform EncontrarAlvoMaisProximo()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag(tagInimigo);
        Transform alvoMaisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector2.Distance(transform.position, inimigo.transform.position);
            
            if (distancia <= alcance && distancia < menorDistancia)
            {
                menorDistancia = distancia;
                alvoMaisProximo = inimigo.transform;
            }
        }

        return alvoMaisProximo;
    }

    void Atirar(Transform alvo)
    {
        Vector2 posicaoTiro = pontoDeDisparo != null ? pontoDeDisparo.position : transform.position;
        GameObject novoProjetil = Instantiate(prefabProjetil, posicaoTiro, Quaternion.identity);

        Projetil scriptProjetil = novoProjetil.GetComponent<Projetil>();
        if (scriptProjetil != null)
        {
            // A torre agora envia o Alvo, o Dano e a Velocidade para a bolinha
            scriptProjetil.Configurar(alvo, danoDeAtaque, velocidadeDoTiro);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcance);
    }
}