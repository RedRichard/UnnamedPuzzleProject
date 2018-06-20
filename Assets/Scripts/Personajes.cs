﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes : PersonajeJugable {
    public Personaje mago;
    public Queue<Vector3> Lista = new Queue<Vector3>(),Enemigos=new Queue<Vector3>();
    public int Pasos, Ataque;
    public string enemigo;
    public void Awake () {
        mago= new Personaje(transform.position,Pasos,Ataque,true,enemigo);
	}
   public  void Tocado() {
        if (mago.turno) {
            int contador = 0;
            MostrarPosiblesPasos(mago.posicion, Lista, contador,mago.maxPasos);
            quitados++;
            MostrarAtaque(Lista.Dequeue(),mago.rangoAtaque,Lista,contador);
        }
    }
    public bool ChecarR()
    {
        bool checar;
        mago.posicion = transform.position;
        int contador = 0;
        checar = ChecarRadio(mago.posicion,mago.rangoAtaque,Lista,contador,Enemigos);
        return checar;
    }
}