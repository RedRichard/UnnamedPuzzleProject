using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorTurno : MonoBehaviour {

    public bool turnoJugadores;
    // public Touch touchScript;
    
    public void ChecarTurno() {
        var jugadores = GameObject.FindGameObjectsWithTag("Player");
        int contador = 0;
        foreach (GameObject jugador in jugadores)
        {
            if (jugador.GetComponent<Personajes>().mago.turno == false)
            {
                contador++;
                // touchScript.MostrarAe();
            }

        }
        if (contador == jugadores.Length)
        {
            turnoJugadores = false;
        }
    }
    public void RegresarJugadores()
    {
        var jugadores = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugador in jugadores)
        {
           jugador.GetComponent<Personajes>().mago.turno= true;
            turnoJugadores = true;
        }
    }
}
