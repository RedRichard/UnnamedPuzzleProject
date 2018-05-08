﻿using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public int quitados, puestos;
    public struct Personaje
    {
        public Vector3 posicion;
        public int maxPasos, rangoAtaque;
        public bool turno,seleccionado;
        public Personaje(Vector3 pos,int MaxPasos, int RangoAtaque, bool Turno,bool Seleccionado)
        {
            posicion = pos;
            maxPasos = MaxPasos;
            rangoAtaque = RangoAtaque;
            turno = Turno;
            seleccionado = Seleccionado;
        }
    }
    public void MostrarPosiblesPasos(Vector3 inicio, Queue<Vector3> Casillas, int contador,int maxPasos)
    {
        inicio += new Vector3(0,1);
        InstanciarPosibles(Casillas, inicio);
        InstanciarAtaque(Casillas, inicio);
        inicio += new Vector3(0, -2);
        InstanciarPosibles(Casillas, inicio);
        InstanciarAtaque(Casillas, inicio);
        inicio += new Vector3(1, 1);
        InstanciarPosibles(Casillas, inicio);
        InstanciarAtaque(Casillas, inicio);
        inicio += new Vector3(-2,0);
        InstanciarPosibles(Casillas, inicio);
        InstanciarAtaque(Casillas, inicio);
        if (quitados == puestos)
        {
            quitados = 0;
            puestos = Casillas.Count;
            contador++;
        }
        if (contador < maxPasos)
        {
            quitados++;
            MostrarPosiblesPasos(Casillas.Dequeue(), Casillas, contador,maxPasos);
        }
        contador = 0;
    }
    public void MostrarAtaque(Vector3 inicio,int ataque, Queue<Vector3> Casillas,int contador)
    {
        inicio += new Vector3(0, 1); 
        InstanciarAtaque(Casillas, inicio);
        inicio += new Vector3(0, -2);
        InstanciarAtaque(Casillas, inicio);
        inicio += new Vector3(1, 1);
        InstanciarAtaque(Casillas, inicio);
        inicio += new Vector3(-2, 0);
        InstanciarAtaque(Casillas, inicio);
        if (quitados == puestos)
        {
            quitados = 0;
            puestos = Casillas.Count;
            contador++;
        }
        if (contador < ataque)
        {
            quitados++;
            MostrarAtaque(Casillas.Dequeue(), ataque, Casillas, contador);
        }
        contador = 0;
        quitados = 0;
        puestos = 0;

    }
    public void InstanciarPosibles(Queue<Vector3> Casillas, Vector3 inicio)
    {
        Ray2D ray = new Ray2D(inicio, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Board")
        {
            Casillas.Enqueue(inicio);
            Instantiate(Resources.Load("posibles"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);
        }
    }
    public void InstanciarAtaque(Queue<Vector3> Casillas, Vector3 inicio)
    {
        Ray2D ray = new Ray2D(inicio, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null)
        {
            if (hit2D.transform.tag == "Board")
            {
                Casillas.Enqueue(inicio);
                Instantiate(Resources.Load("Ataque"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);
            }
            if (hit2D.transform.tag == "Enemigo")
            {
                Instantiate(Resources.Load("Ataque"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);

            }
        }
    }
}
