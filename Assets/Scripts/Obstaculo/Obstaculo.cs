using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Obstaculo : MonoBehaviour
{
    public bool ativar, desativar, estaAtivado, iniciarAtividade, atividadeIniciada; // APENAS PARA DEBUG
    public float contador, contadorAtual;
    public Rigidbody2D SpawnerTorre;

    void Start()
    {
        contadorAtual = contador;
    }

    void Update () //Mesclar com a função de quando o fantasma que limpa o campo for implementado
    {   
        if (ativar == true)
        {
            estaAtivado = true;
            GameObject.Find("Lista").GetComponent<ListaDeTarefas>().ObjClicado = gameObject;
            GameObject.Find("Lista").GetComponent<ListaDeTarefas>().ativar = true;
            ativar = false;
        }
        if(iniciarAtividade == true && atividadeIniciada == false)
        {
            Iniciar();
            atividadeIniciada = true;
        }
    }
    
    public void Iniciar()
    {
        StartCoroutine(Destruir());
    }

    public IEnumerator Destruir()
    { 
        while (contadorAtual >= 1)//Timer para ver quanto tempo falta
        {
            yield return new WaitForSeconds(1f);
            contadorAtual = contadorAtual - 1;
            if (desativar == true)
            {
            contadorAtual = 0;
            estaAtivado = false;
            }
        }
        if (desativar == false)
        {
            Instantiate(SpawnerTorre, transform.position, transform.rotation);
            GameObject.Find("Lista").GetComponent<ListaDeTarefas>().acaoFinalizada = true;
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
        else
        {
            contadorAtual = contador;
            desativar = false;
            atividadeIniciada = false;
            iniciarAtividade = false;
        }
    }
}
