using UnityEngine;

public class TorreOnda : MonoBehaviour
{
    [Header("Atributos da Torre de Onda")]
    public float alcanceMaximo = 5f;        
    public float velocidadeDaOnda = 10f;    
    public float tempoEntrePulsos = 3f;     
    public int danoDeAtaque = 4;            

    [Header("Configurações")]
    public GameObject prefabDoAnel;         

    private float timerAtaque = 0f;

    void Update()
    {
        // Se a torre ainda está recarregando, continua somando o tempo
        if (timerAtaque < tempoEntrePulsos)
        {
            timerAtaque += Time.deltaTime;
        }
        else 
        {
            // Se a torre já recarregou, ela liga o radar para procurar alvos
            if (TemInimigoNaArea())
            {
                GerarOnda();      // Atira a onda
                timerAtaque = 0f; // Zera o tempo para começar a recarregar de novo
            }
        }
    }

    // Função do Radar
    bool TemInimigoNaArea()
    {
        // Cria uma zona de detecção invisível do tamanho exato em que a onda vai crescer
        // Usamos alcanceMaximo / 2f porque a escala total 3D no Unity representa o diâmetro
        Collider2D[] objetosNoRadar = Physics2D.OverlapCircleAll(transform.position, alcanceMaximo / 2f);

        // Passa por todos os objetos que estão dentro dessa área
        foreach (Collider2D colisor in objetosNoRadar)
        {
            // Verifica se o objeto detectado é um inimigo (possui o script de vida)
            if (colisor.GetComponent<VidaFantasma>() != null)
            {
                return true; // Retorna verdadeiro na mesma hora, autorizando o disparo!
            }
        }

        // Se checou tudo no raio e não achou nenhum fantasma
        return false; 
    }

    void GerarOnda()
    {
        GameObject novaOnda = Instantiate(prefabDoAnel, transform.position, Quaternion.identity);

        AnelExpansivo scriptOnda = novaOnda.GetComponent<AnelExpansivo>();
        if (scriptOnda != null)
        {
            scriptOnda.Configurar(alcanceMaximo, velocidadeDaOnda, danoDeAtaque);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, alcanceMaximo / 2f); 
    }
}