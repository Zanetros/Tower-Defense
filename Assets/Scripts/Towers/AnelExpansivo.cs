using System.Collections.Generic;
using UnityEngine;

public class AnelExpansivo : MonoBehaviour
{
    private float alcanceMaximo;
    private float velocidadeExpansao;
    private int dano;
    
    private List<VidaFantasma> fantasmasAtingidos = new List<VidaFantasma>();

    public void Configurar(float alcance, float velocidade, int danoRecebido)
    {
        alcanceMaximo = alcance;
        velocidadeExpansao = velocidade;
        dano = danoRecebido;
        transform.localScale = Vector3.zero; 
    }

    void Update()
    {
        // 1. Cresce a imagem da onda na tela
        transform.localScale += Vector3.one * velocidadeExpansao * Time.deltaTime;

        // 2. Calcula o tamanho atual do raio da onda 
        // (Divide por 2 porque o círculo padrão do Unity tem 1 unidade de diâmetro)
        float raioAtual = transform.localScale.x / 2f;

        // 3. O Radar: Escaneia instantaneamente tudo que está tocando no raio atual da onda
        Collider2D[] objetosAtingidos = Physics2D.OverlapCircleAll(transform.position, raioAtual);

        // 4. Verifica se algum dos objetos escaneados é um fantasma
        foreach (Collider2D colisor in objetosAtingidos)
        {
            VidaFantasma fantasma = colisor.GetComponent<VidaFantasma>();

            // Se for um fantasma e ele AINDA NÃO apanhou dessa onda...
            if (fantasma != null && !fantasmasAtingidos.Contains(fantasma))
            {
                fantasma.TomarDano(dano); // Dá o dano!
                fantasmasAtingidos.Add(fantasma); // Coloca na lista de "já atingidos"
            }
        }

        // 5. Destrói a onda quando atinge o tamanho máximo
        if (transform.localScale.x >= alcanceMaximo)
        {
            Destroy(gameObject);
        }
    }
}