using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public Text text;
    GameObject gobj;
    Vector2 start, end;
    Vector3 tr,ini;
    int pasos = 0;
    int maxPasos;
    float disx, disy;
    void Update () {
        Controlar();
    }
    void Controlar()
    {
        if (Input.touchCount > 0)   //controles touch, no hace nada si no detecta al menos un dedo
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Cuando detecta el dedo lanza un ray 
            {
                RaycastHit2D hit2D = GenerarRay();
                if (hit2D && hit2D.transform.tag=="Player") //Si el objeto es un personaje jugable
                {
                    gobj = hit2D.transform.gameObject;
                    maxPasos = ObtenerMaximoPasos(gobj.transform.parent.gameObject);
                    tr = gobj.transform.position;
                    ini = hit2D.transform.parent.transform.position; //Se guarda la posicion original del jugador
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && gobj)//Cuando el dedo se empieza a mover sobre la pantalla
            {
                start = gobj.transform.position;//la posicion del fantasma
                end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//la posicion del dedo
                disx = Mathf.RoundToInt(end.x - start.x);//se verica las diferencias en las posiciones y cuando se
                disy = Mathf.RoundToInt(end.y - start.y);//acerca a uno, es cuando se mueve al siguiente tile
                if (disx == 1 || disx == -1)
                {
                    tr.x = start.x + disx;
                    gobj.transform.position = tr;
                    if (Mathf.Abs(ini.x - tr.x) > pasos - Mathf.Abs(ini.y - tr.y))
                    {
                        pasos++;
                    }
                    else
                    {
                        pasos--;
                    }
                    if (pasos > maxPasos)//si se supera el numero de pasos
                    {
                       tr.x = start.x;
                       gobj.transform.position = start;//el fantasma regresa a la posicion anterior
                       pasos--;
                    }
                }
                if (disy == 1 || disy == -1)
                {
                    tr.y = start.y + disy;
                    gobj.transform.position = tr;
                    if (Mathf.Abs(ini.y - tr.y) > pasos - Mathf.Abs(ini.x - tr.x))
                    {
                        pasos++;
                    }
                    else
                    {
                        pasos--;
                    }
                    if (pasos > maxPasos)
                    {
                        tr.y = start.y;
                        gobj.transform.position = start;
                        pasos--;
                    }
                }
                text.text = pasos.ToString();
            }
        }
      /*  if (Input.touchCount > 0 && gobj != null)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tr = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                tr = new Vector3(Mathf.RoundToInt( tr.x), Mathf.RoundToInt(tr.y), Mathf.RoundToInt(tr.z));
                tr.x -= .5f;
                tr.y -= .5f;
                gobj.transform.position = tr;
            }
        }*/
    }
    RaycastHit2D GenerarRay()
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position),-Camera.main.gameObject.transform.position);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        return hit2D;
    }
    public void Mover()
    {
        if (gobj != null)
        {
            gobj.transform.parent.transform.position = gobj.transform.position;
            gobj.transform.localPosition = new Vector3();
            pasos = 0;
        }
    }
    public void Cancelar()
    {
        if (gobj != null)
        {
            gobj.transform.localPosition = new Vector3();
            gobj = null;
            pasos = 0;
        }
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
