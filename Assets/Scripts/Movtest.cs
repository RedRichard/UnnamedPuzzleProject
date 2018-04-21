using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movtest : MonoBehaviour {
    public Text text;
    GameObject gObj, gObjParent;
    float pasos;
    int maxPasos;
    bool arriba=true, abajo=true, derecha=true, izquierda=true;
    void Update () {
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
                    maxPasos = ObtenerMaximoPasos(gObjParent);//dependiendo del personaje se veran el max de pasos que puede dar
                }
            }
        }
        if (Input.touchCount > 0 && gObj != null)
        {
            RaycastHit2D hit = GenerarRay();
            if (Input.GetTouch(0).phase == TouchPhase.Began &&hit.collider!=null && hit.transform.tag=="Board" && hit.transform.tag!="Obstacle")
            {
                Mover();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && hit.collider != null && gObj != null && hit.transform.tag== "Board" && hit.transform.tag != "Obstacle")//Cuando el dedo se empieza a mover sobre la pantalla
            {
                Mover();
            }
        }
        Excedespasos();
    }
    void Mover()
    {
        Vector3 end;
        end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//la posicion del dedo
        end = new Vector3(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));//se redondea la posicion del dedo, para manejarlo bien con el grid
        gObj.transform.position = end;//se mueve el fantasma a la posicion del dedo
        pasos = ObtenerPasos(gObj);//se obtienen los pasos que ha dado,
    }
    void Excedespasos()
    {

        float dist;
        while (pasos > maxPasos)
        {
            VerEspacios();
            if (arriba)
            {
                gObj.transform.Translate(Vector3.up);
                dist = ObtenerPasos(gObj);
                if (dist >= pasos)
                {
                    gObj.transform.Translate(Vector3.down);
                }
            }
            if (abajo)
            {
                gObj.transform.Translate(Vector3.down);
                dist = ObtenerPasos(gObj);
                if (dist >= pasos)
                {
                    gObj.transform.Translate(Vector3.up);
                }
            }
            if (izquierda)
            {
                gObj.transform.Translate(Vector3.left);
                dist = ObtenerPasos(gObj);
                if (dist >= pasos)
                {
                    gObj.transform.Translate(Vector3.right);
                }
            }
            if (derecha)
            {
                gObj.transform.Translate(Vector3.right);
                dist = ObtenerPasos(gObj);
                if (dist >= pasos)
                {
                    gObj.transform.Translate(Vector3.left);
                }
            }
            
            pasos = ObtenerPasos(gObj);
        }
        text.text = pasos.ToString();
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
            Vector3 moverPos = gObj.transform.position;
            moverPos.z = -1;
            gObjParent.transform.position = moverPos;
            gObj.transform.localPosition = new Vector3();
            pasos = 0;
            gObj = null;
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
    public float ObtenerPasos(GameObject fantasma)
    {
        float distancia;
        Vector2 posInicio = fantasma.transform.parent.transform.position;
        Vector2 posFinal = fantasma.transform.position;
        distancia = Mathf.Abs(posFinal.x - posInicio.x) + Mathf.Abs(posFinal.y - posInicio.y);
        return distancia;
    }
    public void VerEspacios()
    {
        Ray2D ray = new Ray2D(gObj.transform.position, Vector2.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if(hit.collider!=null && hit.transform.tag == "Obstacle")
        {
            arriba = false;
        }
        ray = new Ray2D(gObj.transform.position, Vector2.down);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.transform.tag == "Obstacle")
        {
            abajo = false;
        }
        ray = new Ray2D(gObj.transform.position, Vector2.left);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.transform.tag == "Obstacle")
        {
            izquierda = false;
        }
        ray = new Ray2D(gObj.transform.position, Vector2.right);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.transform.tag == "Obstacle")
        {
            derecha = false;
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
