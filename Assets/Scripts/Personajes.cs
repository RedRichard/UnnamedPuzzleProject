using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personajes : Movement {
    public Personaje mago;
    public Queue<Vector3> Lista = new Queue<Vector3>();
    public int Pasos, Ataque;
    public void Mago () {
        mago= new Personaje(transform.position,Pasos,Ataque,true,false);
	}
   public  void Tocado() {
        if (mago.turno) {
            mago.seleccionado = true;
            int contador = 0;
            MostrarPosiblesPasos(mago.posicion, Lista, contador,mago.maxPasos);
            quitados++;
            MostrarAtaque(Lista.Dequeue(),mago.rangoAtaque,Lista,contador);
        }
    }

}
