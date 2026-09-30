using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class ListaDeTarefas : MonoBehaviour
{
    public GameObject Icone1, Icone2, Icone3, Icone4, Icone5, PrefabIcone, Tarefa1, Tarefa2, Tarefa3, Tarefa4, Tarefa5, ObjClicado, Pos1, Pos2, Pos3, Pos4, Pos5, IconeColocado;
    public bool ativar, acaoFinalizada;
    private int numeradorIconeTarefa1, numeradorIconeTarefa2, numeradorIconeTarefa3, numeradorIconeTarefa4, numeradorIconeTarefa5, timerGeral,
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
                AdicionarNovo(1); // Associa a tarefa (Por exemplo, limpar slot) para o icone da lista
            }
            else if (Tarefa2 == null)
            {
                Tarefa2 = ObjClicado;
                timer2 = timerGeral;
                AdicionarNovo(2);
            }
            else if (Tarefa3 == null)
            {
                Tarefa3 = ObjClicado;
                timer3 = timerGeral;
                AdicionarNovo(3);
            }
            else if (Tarefa4 == null)
            {
                Tarefa4 = ObjClicado;
                timer4 = timerGeral;
                AdicionarNovo(4);
            }
            else if (Tarefa5 == null)
            {
                Tarefa5 = ObjClicado;
                timer5 = timerGeral;
                AdicionarNovo(5);
            }
            ativar = false;
            
        }
    }

    public IEnumerator Atualizar()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            AtualizaLista();
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
        }catch{}
//---------Inicio do codigo do posicionamento
        valPos1 = timer1;
        valPos2 = timer2;
        valPos3 = timer3;
        valPos4 = timer4;
        valPos5 = timer5;

        float[] valores = { valPos1, valPos2, valPos3, valPos4, valPos5 };

        System.Array.Sort(valores);
        for (int i = 0; i < valores.Length; i++)
        {
            print(valores[0]);
            print(valores[1]);
            print(valores[2]);
            print(valores[3]);
            print(valores[4]);
 //--------------------------------------------------------------------------------------------------------------------------------           
            if (valores[i] == valPos1 - 1 && valPos1 != 0)
            {
                if (numeradorIconeTarefa1 == i && Icone1 != null)
                {
                    Icone1.transform.position = Pos1.transform.position;
                }
                if (numeradorIconeTarefa2 == i && Icone2 != null)
                {
                    Icone2.transform.position = Pos1.transform.position;
                }
                if (numeradorIconeTarefa3 == i && Icone3 != null)
                {
                    Icone3.transform.position = Pos1.transform.position;
                }
                if (numeradorIconeTarefa4 == i && Icone4 != null)
                {
                    Icone4.transform.position = Pos1.transform.position;
                }
                if (numeradorIconeTarefa5 == i && Icone5 != null)
                {
                    Icone5.transform.position = Pos1.transform.position;
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            if (valores[i] == valPos2 - 1 && valPos2 != 0)
            {
                if (numeradorIconeTarefa1 == i && Icone1 != null)
                {
                    Icone1.transform.position = Pos2.transform.position;
                }
                if (numeradorIconeTarefa2 == i && Icone2 != null)
                {
                    Icone2.transform.position = Pos2.transform.position;
                }
                if (numeradorIconeTarefa3 == i && Icone3 != null)
                {
                    Icone3.transform.position = Pos2.transform.position;
                }
                if (numeradorIconeTarefa4 == i && Icone4 != null)
                {
                    Icone4.transform.position = Pos2.transform.position;
                }
                if (numeradorIconeTarefa5 == i && Icone5 != null)
                {
                    Icone5.transform.position = Pos2.transform.position;
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            if (valores[i] == valPos3 - 1 && valPos3 != 0)
            {
                if (numeradorIconeTarefa1 == i && Icone1 != null)
                {
                    Icone1.transform.position = Pos3.transform.position;    
                }
                if (numeradorIconeTarefa2 == i && Icone2 != null)
                {
                    Icone2.transform.position = Pos3.transform.position;
                }
                if (numeradorIconeTarefa3 == i && Icone3 != null)
                {
                    Icone3.transform.position = Pos3.transform.position;
                }
                if (numeradorIconeTarefa4 == i && Icone4 != null)
                {
                    Icone4.transform.position = Pos3.transform.position;
                }
                if (numeradorIconeTarefa5 == i && Icone5 != null)
                {
                    Icone5.transform.position = Pos3.transform.position;
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            if (valores[i] == valPos4 - 1 && valPos4 != 0)
            {
                if (numeradorIconeTarefa1 == i && Icone1 != null)
                {
                    Icone1.transform.position = Pos4.transform.position;    
                }
                if (numeradorIconeTarefa2 == i && Icone2 != null)
                {
                    Icone2.transform.position = Pos4.transform.position;
                }
                if (numeradorIconeTarefa3 == i && Icone3 != null)
                {
                    Icone3.transform.position = Pos4.transform.position;
                }
                if (numeradorIconeTarefa4 == i && Icone4 != null)
                {
                    Icone4.transform.position = Pos4.transform.position;
                }
                if (numeradorIconeTarefa5 == i && Icone5 != null)
                {
                    Icone5.transform.position = Pos4.transform.position;
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
            if (valores[i] == valPos5 - 1 && valPos5 != 0)
            {
                if (numeradorIconeTarefa1 == i && Icone1 != null)
                {
                    Icone1.transform.position = Pos5.transform.position;
                }
                if (numeradorIconeTarefa2 == i && Icone2 != null)
                {
                    Icone2.transform.position = Pos5.transform.position;
                }
                if (numeradorIconeTarefa3 == i && Icone3 != null)
                {
                    Icone3.transform.position = Pos5.transform.position;
                }
                if (numeradorIconeTarefa4 == i && Icone4 != null)
                {
                    Icone4.transform.position = Pos5.transform.position;
                }
                if (numeradorIconeTarefa5 == i && Icone5 != null)
                {
                    Icone5.transform.position = Pos5.transform.position;
                }
            }
//--------------------------------------------------------------------------------------------------------------------------------
        }
    }
    
    public void AdicionarNovo(int numerador)
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