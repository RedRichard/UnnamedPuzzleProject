using System.Collections.Generic;
using UnityEngine;

public class Personajes : PersonajeJugable {
    public Personaje mago;
    public Queue<Vector3> Lista = new Queue<Vector3>();
    public List<GameObject> enemigos = new List<GameObject>();
    public int Pasos, Ataque;
    public string enemigo,habilidad;
    public void Awake () {
        mago= new Personaje(transform.position,Pasos,Ataque,true,enemigo);
	}
   public  void Tocado() {
        if (mago.turno) {
            int contador = 0;
            MostrarPosiblesPasos(gameObject.transform.position, Lista, contador,mago.maxPasos);
            quitados++;
            if (Lista.Count > 0)
            {
                MostrarAtaque(Lista.Dequeue(), mago.rangoAtaque, Lista, contador);
            }
            else
            {
                MostrarAtaque(mago.posicion, mago.rangoAtaque, Lista, contador);
            }
        }
    }
    public bool ChecarR()
    {
        bool checar;
        mago.posicion = transform.position;
        int contador = 0;
        checar = ChecarRadio(mago.posicion,mago.rangoAtaque,Lista,contador,enemigos);
        return checar;
    }
}