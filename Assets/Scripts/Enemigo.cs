using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : PadreEnemigo {
    public Enemigos enemigo;
    public Queue<Vector3> ataquezona = new Queue<Vector3>(),jugadores=new Queue<Vector3>();
    public int Ataque;
    public string Mata;
    public bool activo;
    // Use this for initialization
    void Start () {
        activo = false;
        enemigo = new Enemigos(transform.position, Ataque,Mata,activo);
        MostrarR();
    }
    public void MostrarR()
    {
        int cont = 0;
        MostrarAtaque(transform.position, enemigo.rangoAtaque, ataquezona, cont, this.gameObject);
        ataquezona.Clear();
    }
    public void Atacar()
    {
        enemigo.activo = true;
        QuitarAtacZone();
        if (ChecarR()&& enemigo.activo)
        {
            bool atacando = true;
            foreach(Vector3 jug in jugadores)
            {
                Ray2D ray = new Ray2D(jug, Camera.main.transform.forward);
                RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
                if (atacando && hit2D.collider.name == enemigo.Enemigo)
                {
                    Destroy(hit2D.collider.gameObject.transform.parent.gameObject);
                    atacando = false;
                }
            }
        }
        jugadores.Clear();
        activo = false;
    }
    public bool ChecarR()
    {
        ataquezona.Clear();
        bool checar;
        int contador = 0;
        checar = ChecarRadio(enemigo.posicion, enemigo.rangoAtaque, ataquezona, contador, jugadores);
        return checar;
    }
    public void QuitarAtacZone()
    {
        for (int cont = 0; cont < transform.childCount; cont++)
        {
            Destroy(transform.GetChild(cont).gameObject);
        }
    }
}
