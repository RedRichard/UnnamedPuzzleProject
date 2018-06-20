using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadreEnemigo : MonoBehaviour {
    public int quitados, puestos;
    public struct Enemigos
    {
        public Vector3 posicion;
        public int rangoAtaque;
        public string Enemigo;
        public bool activo;
        public Enemigos(Vector3 pos, int RangoAtaque,string enemy,bool act)
        {
            posicion = pos;
            rangoAtaque = RangoAtaque;
            Enemigo = enemy;
            activo = act;
        }
    }
    public void MostrarAtaque(Vector3 inicio, int ataque, Queue<Vector3> Casillas, int contador,GameObject enemigo)
    {
        inicio += new Vector3(0, 1);
        InstanciarAtaque(Casillas, inicio,enemigo);
        inicio += new Vector3(0, -2);
        InstanciarAtaque(Casillas, inicio,enemigo);
        inicio += new Vector3(1, 1);
        InstanciarAtaque(Casillas, inicio,enemigo);
        inicio += new Vector3(-2, 0);
        InstanciarAtaque(Casillas, inicio,enemigo);
        if (quitados == puestos)
        {
            quitados = 0;
            puestos = Casillas.Count;
            contador++;
        }
        if (contador < ataque)
        {
            quitados++;
            MostrarAtaque(Casillas.Dequeue(), ataque, Casillas, contador,enemigo);
        }
        contador = 0;
        quitados = 0;
        puestos = 0;
    }
    public void InstanciarAtaque(Queue<Vector3> Casillas, Vector3 inicio,GameObject enemigo)
    {
        Ray2D ray = new Ray2D(inicio, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null)
        {
            if (hit2D.transform.tag == "Board")
            {
                Casillas.Enqueue(inicio);
                Instantiate(Resources.Load("ataqueEn"), inicio, Quaternion.identity, enemigo.transform);
            }
            if (hit2D.transform.tag == "Player")
            {
                Instantiate(Resources.Load("ataqueEn"), inicio, Quaternion.identity, enemigo.transform);

            }
        }
    }
    public bool ChecarRadio(Vector3 inicio, int ataque, Queue<Vector3> Casillas, int contador, Queue<Vector3> jugadores)
    {
        bool prueba = false;
        inicio += new Vector3(0, 1);
        Ray2D ray = new Ray2D(inicio, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null)
        {
            Casillas.Enqueue(inicio);
            if (hit2D.transform.tag == "Player")
            {
                jugadores.Enqueue(inicio);
            }
        }
        inicio += new Vector3(0, -2);
        ray = new Ray2D(inicio, Camera.main.transform.forward);
        hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null)
        {
            Casillas.Enqueue(inicio);
            if (hit2D.transform.tag == "Player")
            {
                jugadores.Enqueue(inicio);
            }
        }
        inicio += new Vector3(1, 1);
        ray = new Ray2D(inicio, Camera.main.transform.forward);
        hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null)
        {
            Casillas.Enqueue(inicio);
            if (hit2D.transform.tag == "Player")
            {
                jugadores.Enqueue(inicio);
            }
        }
        inicio += new Vector3(-2, 0);
        ray = new Ray2D(inicio, Camera.main.transform.forward);
        hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null)
        {
            Casillas.Enqueue(inicio);
            if (hit2D.transform.tag == "Player")
            {
                jugadores.Enqueue(inicio);
            }
        }
        if (quitados == puestos)
        {
            quitados = 0;
            puestos = Casillas.Count;
            contador++;
        }
        if (contador < ataque)
        {
            quitados++;
            prueba = ChecarRadio(Casillas.Dequeue(), ataque, Casillas, contador, jugadores);
        }
        contador = 0;
        quitados = 0;
        puestos = 0;
        if (jugadores.Count > 0)
        {
            prueba = true;
        }
        return prueba;
    }
}
