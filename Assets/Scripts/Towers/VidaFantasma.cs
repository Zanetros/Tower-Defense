using UnityEngine;

public class VidaFantasma : MonoBehaviour
{
    public int vida = 10;

    public void TomarDano(int quantidade)
    {
        vida -= quantidade; // Subtrai a vida
        
        // Isso vai escrever no Console do Unity toda vez que ele apanhar
        Debug.Log("O Fantasma apanhou! Vida restante: " + vida); 

        // Se a vida for menor ou igual a zero, ele morre
        if (vida <= 0)
        {
            Debug.Log("O Fantasma morreu!");
            Destroy(gameObject); // Comando do Unity que apaga o objeto da cena e da Hierarchy
        }
    }
}