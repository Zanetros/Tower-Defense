using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using TMPro;

public class ListaDeTarefas : MonoBehaviour
{
    public GameObject Icone1, Icone2, Icone3, Icone4, Icone5, PrefabIcone, Tarefa1, Tarefa2, Tarefa3, Tarefa4, Tarefa5, ObjClicado, Pos1, Pos2, Pos3, Pos4, Pos5, IconeColocado,
    numeradorIconeTarefa1, numeradorIconeTarefa2, numeradorIconeTarefa3, numeradorIconeTarefa4, numeradorIconeTarefa5;
    public bool ativar, acaoFinalizada;
    private int timerGeral,
    timer1 = 0, timer2 = 0, timer3 = 0, timer4 = 0, timer5 = 0, valPos1, valPos2, valPos3, valPos4, valPos5;
    void Awake()
    {
        StartCoroutine(Atualizar());
    }

    void Update()
    {
        timerGeral = timerGeral+1;//Tirar daqui?
        if (ativar == true)
        {
            AtualizaLista();
            if (Tarefa1 == null)
            {
                Tarefa1 = ObjClicado;
                timer1 = timerGeral;
                AdicionarNovo(Tarefa1); // Associa a tarefa (Por exemplo, limpar slot) para o icone da lista
            }
            else if (Tarefa2 == null)
            {
                Tarefa2 = ObjClicado;
                timer2 = timerGeral;
                AdicionarNovo(Tarefa2);
            }
            else if (Tarefa3 == null)
            {
                Tarefa3 = ObjClicado;
                timer3 = timerGeral;
                AdicionarNovo(Tarefa3);
            }
            else if (Tarefa4 == null)
            {
                Tarefa4 = ObjClicado;
                timer4 = timerGeral;
                AdicionarNovo(Tarefa4);
            }
            else if (Tarefa5 == null)
            {
                Tarefa5 = ObjClicado;
                timer5 = timerGeral;
                AdicionarNovo(Tarefa5);
            }
            ativar = false;
            
        }
    }

    public IEnumerator Atualizar()
    {
        while(true)
        {
            AtualizaLista();
            yield return new WaitForSeconds(0.5f);
            if(acaoFinalizada == true)
            {
                yield return new WaitForSeconds(0.2f);
                ativar = true;
                acaoFinalizada = false;
            }
        }
    }
    public void AtualizaLista()
    {
        try
        {
            if (Tarefa1 == null)
            {
                timer1 = 0;
            }
            if (Tarefa2 == null)
            {
                timer2 = 0;
            }
            if (Tarefa3 == null)
            {
                timer3 = 0;
            }
            if (Tarefa4 == null)
            {
                timer4 = 0;
            }
            if (Tarefa5 == null)
            {
                timer5 = 0;
            }

            int menor = timer1;

            if (timer2 > 0 && timer2 < menor)
                menor = timer2;

            if (timer3 > 0 && timer3 < menor)
                menor = timer3;

            if (timer4 > 0 && timer4 < menor)
                menor = timer4;

            if (timer5 > 0 && timer5 < menor)
                menor = timer5;

            if (menor == timer1)
            {
                if (Tarefa1.GetComponent<Obstaculo>().iniciarAtividade == false)
                {
                    Tarefa1.GetComponent<Obstaculo>().iniciarAtividade = true;
                }
            }
            else if (menor == timer2)
            {
                if (Tarefa2.GetComponent<Obstaculo>().iniciarAtividade == false)
                {
                    Tarefa2.GetComponent<Obstaculo>().iniciarAtividade = true;
                }
            }
            else if (menor == timer3)
            {
                if (Tarefa3.GetComponent<Obstaculo>().iniciarAtividade == false)
                {
                    Tarefa3.GetComponent<Obstaculo>().iniciarAtividade = true;
                }
            }
            else if (menor == timer4)
            {
                if (Tarefa4.GetComponent<Obstaculo>().iniciarAtividade == false)
                {
                    Tarefa4.GetComponent<Obstaculo>().iniciarAtividade = true;
                }
            }
            else if (menor == timer5)
            {
                if (Tarefa5.GetComponent<Obstaculo>().iniciarAtividade == false)
                {
                    Tarefa5.GetComponent<Obstaculo>().iniciarAtividade = true;
                }
            }
        
//---------Inicio do codigo do posicionamento
            valPos1 = timer1;
            valPos2 = timer2;
            valPos3 = timer3;
            valPos4 = timer4;
            valPos5 = timer5;

            float[] valores = { valPos1, valPos2, valPos3, valPos4, valPos5 };

            System.Array.Sort(valores);

            if (valPos1 == 0) Destroy(Icone1);
            if (valPos2 == 0) Destroy(Icone2);
            if (valPos3 == 0) Destroy(Icone3);
            if (valPos4 == 0) Destroy(Icone4);
            if (valPos5 == 0) Destroy(Icone5);

            for (int i = 4; i >= 0; i--)
            {
                switch (valores[i])
                {
                    case var valor when valor == valPos1:
                        if (i == 4) Icone1.transform.position = Pos1.transform.position;
                        if (i == 3) Icone1.transform.position = Pos2.transform.position;
                        if (i == 2) Icone1.transform.position = Pos3.transform.position;
                        if (i == 1) Icone1.transform.position = Pos4.transform.position;
                        if (i == 0) Icone1.transform.position = Pos5.transform.position;
                        print(Icone1.transform.Find("Tempo_TXT").GetComponent<TMP_Text>().text);
                        Icone1.transform.Find("Tempo_TXT").GetComponent<TMP_Text>().text = numeradorIconeTarefa1.GetComponent<Obstaculo>().contadorAtual.ToString();
                        break;

                    case var valor when valor == valPos2:
                        if (i == 4) Icone2.transform.position = Pos1.transform.position;
                        if (i == 3) Icone2.transform.position = Pos2.transform.position;
                        if (i == 2) Icone2.transform.position = Pos3.transform.position;
                        if (i == 1) Icone2.transform.position = Pos4.transform.position;
                        if (i == 0) Icone2.transform.position = Pos5.transform.position;
                        break;

                    case var valor when valor == valPos3:
                        if (i == 4) Icone3.transform.position = Pos1.transform.position;
                        if (i == 3) Icone3.transform.position = Pos2.transform.position;
                        if (i == 2) Icone3.transform.position = Pos3.transform.position;
                        if (i == 1) Icone3.transform.position = Pos4.transform.position;
                        if (i == 0) Icone3.transform.position = Pos5.transform.position;
                        break;

                    case var valor when valor == valPos4:
                        if (i == 4) Icone4.transform.position = Pos1.transform.position;
                        if (i == 3) Icone4.transform.position = Pos2.transform.position;
                        if (i == 2) Icone4.transform.position = Pos3.transform.position;
                        if (i == 1) Icone4.transform.position = Pos4.transform.position;
                        if (i == 0) Icone4.transform.position = Pos5.transform.position;
                        break;

                    case var valor when valor == valPos5:
                        if (i == 4) Icone5.transform.position = Pos1.transform.position;
                        if (i == 3) Icone5.transform.position = Pos2.transform.position;
                        if (i == 2) Icone5.transform.position = Pos3.transform.position;
                        if (i == 1) Icone5.transform.position = Pos4.transform.position;
                        if (i == 0) Icone5.transform.position = Pos5.transform.position;
                        break;
                }
            }
        }catch{}
    }
    
    public void AdicionarNovo(GameObject numerador)
    {
        if (Icone1 == null && ObjClicado != IconeColocado)
        {
            Icone1 = Instantiate(PrefabIcone, new Vector3(transform.position.x , transform.position.y  - 2000f , transform.position.z), transform.rotation);
            numeradorIconeTarefa1 = numerador;// Associa a tarefa (Por exemplo, limpar slot) para o icone da lista
            IconeColocado = ObjClicado;
        }
        else if (Icone2 == null && ObjClicado != IconeColocado)
        {
            Icone2 = Instantiate(PrefabIcone, new Vector3(transform.position.x , transform.position.y  - 2000f, transform.position.z), transform.rotation);
            numeradorIconeTarefa2 = numerador;
            IconeColocado = ObjClicado;
        }
        else if (Icone3 == null && ObjClicado != IconeColocado)
        {
            Icone3 = Instantiate(PrefabIcone, new Vector3(transform.position.x , transform.position.y  - 2000f, transform.position.z), transform.rotation);
            numeradorIconeTarefa3 = numerador;
            IconeColocado = ObjClicado;
        }
        else if (Icone4 == null && ObjClicado != IconeColocado)
        {
            Icone4 = Instantiate(PrefabIcone, new Vector3(transform.position.x , transform.position.y  - 2000f, transform.position.z), transform.rotation);
            numeradorIconeTarefa4 = numerador;
            IconeColocado = ObjClicado;
        }
        else if (Icone5 == null && ObjClicado != IconeColocado)
        {
            Icone5 = Instantiate(PrefabIcone, new Vector3(transform.position.x , transform.position.y  - 2000f, transform.position.z), transform.rotation);
            numeradorIconeTarefa5 = numerador;
            IconeColocado = ObjClicado;
        }
    }
}