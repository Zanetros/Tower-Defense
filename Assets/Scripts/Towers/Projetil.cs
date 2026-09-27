using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float tempoDeVida = 3f; 
    
    private int danoDoTiro;
    private float velocidade; // Removido o "public" para ser controlada pela Torre
    private Vector2 direcao; 

    // A função agora recebe as 3 informações
    public void Configurar(Transform alvoRecebido, int danoRecebido, float velocidadeRecebida)
    {
        danoDoTiro = danoRecebido;
        velocidade = velocidadeRecebida; // Grava a velocidade ditada pela Torre
        
        direcao = (alvoRecebido.position - transform.position).normalized;
        Destroy(gameObject, tempoDeVida); 
    }

    void Update()
    {
        float distanciaAAndar = velocidade * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distanciaAAndar);

        if (hit.collider != null)
        {
            VidaFantasma fantasma = hit.collider.GetComponent<VidaFantasma>();

            if (fantasma != null)
            {
                fantasma.TomarDano(danoDoTiro); 
                Destroy(gameObject);            
                return;                         
            }
        }

        transform.Translate(direcao * distanciaAAndar);
    }
}