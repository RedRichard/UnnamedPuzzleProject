using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public Text text;
    GameObject gobj;
    Vector2 start, end;
    int pasos = 0;
    int maxPasos;
    float disx, disy;
    void FixedUpdate () {
        Controlar();
    }
    void Controlar()
    {
        if (Input.touchCount > 0 && gobj==null)   //controles touch, no hace nada si no detecta al menos un dedo
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Cuando detecta el dedo lanza un ray 
            {
                RaycastHit2D hit2D = GenerarRay();
                if (hit2D && hit2D.transform.tag=="Player") //Si el objeto es un personaje jugable
                {
                    gobj = hit2D.transform.gameObject;//se guarda el objeto con el que el ray choco
                    maxPasos = ObtenerMaximoPasos(gobj.transform.parent.gameObject);//dependiendo del personaje se veran el max de pasos que puede dar
                }
            }
        }
        if (Input.touchCount > 0 && gobj != null)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Mover();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && gobj != null)//Cuando el dedo se empieza a mover sobre la pantalla
            {
                Mover();
            }
        }
        int dist;
        while (pasos > maxPasos)
        {
            gobj.transform.Translate(Vector3.up);
            dist = ObtenerPasos(gobj.transform.position);
            if (dist > pasos)
            {
                gobj.transform.Translate(Vector3.down);
            }
            gobj.transform.Translate(Vector3.down);
            dist = ObtenerPasos(gobj.transform.position);
            if (dist > pasos)
            {
                gobj.transform.Translate(Vector3.up);
            }
            gobj.transform.Translate(Vector3.left);
            dist = ObtenerPasos(gobj.transform.position);
            if (dist > pasos)
            {
                gobj.transform.Translate(Vector3.right);
            }
            gobj.transform.Translate(Vector3.right);
            dist = ObtenerPasos(gobj.transform.position);
            if (dist > pasos)
            {
                gobj.transform.Translate(Vector3.left);
            }
            pasos = ObtenerPasos(gobj.transform.position);
        }
    }
    void Mover()
    {
        end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//la posicion del dedo
        end = new Vector3(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));//se redondea la posicion del dedo, para manejarlo bien con el grid
        gobj.transform.position = end;//se mueve el fantasma a la posicion del dedo
        pasos = ObtenerPasos(gobj.transform.position);//se obtienen los pasos que ha dado,
    }
    RaycastHit2D GenerarRay()
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position),-Camera.main.gameObject.transform.position);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        return hit2D;
    }
    public void MoverButton()
    {
        /* para evitar error, no se puedde acceder a su funcionalidad no hay un personaje seleccionado
         * se mueve el padre del fantasma, es decir al personaje bien iluminado a la posicion del fantasma,
         * y para no mover tambien al fantasma se reinicia su transform, asi estara justo donde este el personaje
         * y la variable de los pasos se reinicia.
         */
        if (gobj != null)
        {
            gobj.transform.parent.transform.position = gobj.transform.position;
            gobj.transform.localPosition = new Vector3();
            pasos = 0;
        }
    }
    public void CancelarButton()
    {
        if (gobj != null)
        {
            gobj.transform.localPosition = new Vector3();
            gobj = null;
            pasos = 0;
        }
    }
    public int ObtenerPasos(Vector3 pos)
    {
        int pasos;
        pos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        pasos = (int)(Mathf.Abs(pos.x) + Mathf.Abs(pos.y));
        return pasos;
    }
   public int ObtenerMaximoPasos(GameObject personaje)
    {
        switch (personaje.name)
        {
            case "Mage":
                return 5;
            default:
                return 0;

        }
    }
}
