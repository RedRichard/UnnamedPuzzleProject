using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public Text text;
    GameObject gObj, gObjParent;
    Vector2 start, end;
    int pasos = 0;
    int maxPasos;
    float disx, disy;
    void FixedUpdate () {
        Controlar();
    }
    void Controlar()
    {
        if (Input.touchCount > 0 && gObj==null)   //controles touch, no hace nada si no detecta al menos un dedo
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Cuando detecta el dedo lanza un ray 
            {
                RaycastHit2D hit2D = GenerarRay();
                if (hit2D && hit2D.transform.tag=="Player") //Si el objeto es un personaje jugable
                {
                    gObj = hit2D.transform.gameObject;//se guarda el objeto con el que el ray choco
                    gObjParent = hit2D.transform.parent.transform.gameObject; //Guardamos al Jugador
                    maxPasos = ObtenerMaximoPasos(gObj.transform.parent.gameObject);//dependiendo del personaje se veran el max de pasos que puede dar
                }
            }
        }
        if (Input.touchCount > 0 && gObj != null)
        {
            RaycastHit2D hit = GenerarRay();
            print(hit.transform.tag);
            if (Input.GetTouch(0).phase == TouchPhase.Began && hit.transform.tag=="Board")
            {
                Mover();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && gObj != null && hit.transform.tag=="Board")//Cuando el dedo se empieza a mover sobre la pantalla
            {
                Mover();
            }
        }
        int dist;
        while (pasos > maxPasos)
        {
            gObj.transform.Translate(Vector3.up);
            dist = ObtenerPasos(gObj.transform.position);
            if (dist > pasos)
            {
                gObj.transform.Translate(Vector3.down);
            }
            gObj.transform.Translate(Vector3.down);
            dist = ObtenerPasos(gObj.transform.position);
            if (dist >= pasos)
            {
                gObj.transform.Translate(Vector3.up);
            }
            gObj.transform.Translate(Vector3.left);
            dist = ObtenerPasos(gObj.transform.position);
            if (dist > pasos)
            {
                gObj.transform.Translate(Vector3.right);
            }
            gObj.transform.Translate(Vector3.right);
            dist = ObtenerPasos(gObj.transform.position);
            if (dist >= pasos)
            {
                gObj.transform.Translate(Vector3.left);
            }
            pasos = ObtenerPasos(gObj.transform.position);
        }
        text.text = pasos.ToString();
    }
    void Mover()
    {
        end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//la posicion del dedo
        end = new Vector3(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));//se redondea la posicion del dedo, para manejarlo bien con el grid
        gObj.transform.position = end;//se mueve el fantasma a la posicion del dedo
        pasos = ObtenerPasos(gObj.transform.position);//se obtienen los pasos que ha dado,
    }
    RaycastHit2D GenerarRay()
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position),-Camera.main.gameObject.transform.position);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        return hit2D;
    }
    public void MoverBut()
    {
        /* para evitar error, no se puedde acceder a su funcionalidad no hay un personaje seleccionado
         * se mueve el padre del fantasma, es decir al personaje bien iluminado a la posicion del fantasma,
         * y para no mover tambien al fantasma se reinicia su transform, asi estara justo donde este el personaje
         * y la variable de los pasos se reinicia.
         */
        if (gObj != null)
        {
            gObjParent.transform.position = gObj.transform.position;
            gObj.transform.localPosition = new Vector3();
            pasos = 0;
        }
    }
    public void CancelarBut()
    {
        if (gObj != null)
        {
            gObj.transform.localPosition = new Vector3();
            gObj = null;
            pasos = 0;
        }
    }
    public int ObtenerPasos(Vector3 pos)
    {
        int dist;
        pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y));
        dist = (int)(Mathf.Abs(pos.x) + Mathf.Abs(pos.y));
        return dist;
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
