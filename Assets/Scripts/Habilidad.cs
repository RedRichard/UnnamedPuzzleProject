using System.Collections.Generic;
using UnityEngine;

public class Habilidad : MonoBehaviour {
    public List<GameObject> personajes = new List<GameObject>();
    public List<Vector3> casillas = new List<Vector3>();
    Vector3 posicion,sigcasilla;
    string habilidad;
    public void Awake()
    {
        habilidad = gameObject.GetComponent<Personajes>().habilidad;
    }
    public bool Swap()
    {
        posicion = gameObject.transform.position;
        posicion+= new Vector3(0, 1);
        LanzarRayoSwap();
        posicion += new Vector3(0, -2);
        LanzarRayoSwap();
        posicion += new Vector3(1, 1);
        LanzarRayoSwap();
        posicion += new Vector3(-2, 0);
        LanzarRayoSwap();
        if (personajes.Count > 0)
        {
            return true;
        }
        return false;
    }
    bool Smite()
    {
        posicion = gameObject.transform.position;
        posicion += new Vector3(0, 1);
        sigcasilla = posicion + new Vector3(0, 2);
        LanzarRayoCasilla();
        posicion += new Vector3(0, -2);
        sigcasilla = posicion + new Vector3(0, -2);
        LanzarRayoCasilla();
        posicion += new Vector3(1, 1);
        sigcasilla = posicion + new Vector3(2, 0);
        LanzarRayoCasilla();
        posicion += new Vector3(-2, 0);
        sigcasilla = posicion + new Vector3(-2, 0);
        LanzarRayoCasilla();
        if (personajes.Count > 0)
        {
            return true;
        }
        return false;
    }
    bool DrawBack()
    {
        posicion = gameObject.transform.position+Vector3.up;
        sigcasilla = gameObject.transform.position + Vector3.down;
        LanzarRayoCasilla();
        posicion = gameObject.transform.position + Vector3.down;
        sigcasilla = gameObject.transform.position + Vector3.up;
        LanzarRayoCasilla();
        posicion = gameObject.transform.position + Vector3.left;
        sigcasilla = gameObject.transform.position + Vector3.right;
        LanzarRayoCasilla();
        posicion = gameObject.transform.position + Vector3.right;
        sigcasilla = gameObject.transform.position + Vector3.left;
        LanzarRayoCasilla();
        if (personajes.Count > 0)
        {
            return true;
        }
        return false;
    }
    bool Shove()
    {
        posicion = gameObject.transform.position;
        posicion += new Vector3(0, 1);
        sigcasilla = posicion + new Vector3(0, 1);
        LanzarRayoCasilla();
        posicion += new Vector3(0, -2);
        sigcasilla = posicion + new Vector3(0, -1);
        LanzarRayoCasilla();
        posicion += new Vector3(1, 1);
        sigcasilla = posicion + new Vector3(1, 0);
        LanzarRayoCasilla();
        posicion += new Vector3(-2, 0);
        sigcasilla = posicion + new Vector3(-1, 0);
        LanzarRayoCasilla();
        if (personajes.Count > 0)
        {
            return true;
        }
        return false;
    }
    public void LanzarRayoCasilla()
    {
        Ray2D ray = new Ray2D(posicion, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Player")
        {
            ray = new Ray2D(sigcasilla, Camera.main.transform.forward);
            RaycastHit2D board= Physics2D.Raycast(ray.origin, ray.direction);
            if (board.collider != null && board.transform.tag == "Board")
            {
                personajes.Add(hit2D.collider.gameObject);
                casillas.Add(sigcasilla);
            }
        }
    }
    public void LanzarRayoSwap()
    {
        Ray2D ray = new Ray2D(posicion, Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Player")
        {
                personajes.Add(hit2D.collider.gameObject);
        }
    }
    public bool ChecarRadio()
    {
        switch (habilidad)
        {
            case "Swap":
                return(Swap());
            case "Smite":
                return(Smite());
            case "Draw Back":
                return (DrawBack());
            case "Shove":
                return (Shove());
            default:
                return false;
        }
    }
    public void RealizarHabilidad(GameObject objetivo)
    {
        switch (habilidad)
        {
            case "Swap":
                posicion = gameObject.transform.position;
                gameObject.transform.parent.transform.position = objetivo.transform.parent.transform.position;
                objetivo.transform.parent.transform.position = posicion;
                break;
            case "Smite":
                objetivo.transform.parent.transform.position = casillas[personajes.IndexOf(objetivo)];
                break;
            case "Draw Back":
                objetivo.transform.parent.transform.position = casillas[personajes.IndexOf(objetivo)];
                break;
            case "Shove":
                objetivo.transform.parent.transform.position = casillas[personajes.IndexOf(objetivo)];
                break;
            default:
                break;
        }
    }
}