using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilidadEmpujar : MonoBehaviour {
    public int casillasEmpujar;
    public Queue<GameObject> Personajes = new Queue<GameObject>();
    List<GameObject> personajes = new List<GameObject>();
    Vector3 posicion;
    public void Habilidad()
    {
        posicion = this.gameObject.transform.position;
        posicion+= new Vector3(0, 1);
        LanzarRayo();
        posicion += new Vector3(0, -2);
        LanzarRayo();
        posicion += new Vector3(1, 1);
        LanzarRayo();
        posicion += new Vector3(-2, 0);
        LanzarRayo();
    }
    public void LanzarRayo()
    {
        Ray2D ray = new Ray2D(posicion, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Player")
        {
            personajes.Add(hit2D.collider.gameObject);
        }
    }
}
