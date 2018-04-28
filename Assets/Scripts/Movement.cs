using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public Text text;
    GameObject gObj, gObjParent;
    float pasos;
    int maxPasos,quitados,puestos;
    Vector3 posInicial;
    Queue<Vector3> Lista = new Queue<Vector3>();
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
                    int contador = 0;
                    MostrarPosiblesPasos(gObj.transform.position+ new Vector3(0,0,-1),Lista,contador);
                }
            }
        }
        if (Input.touchCount > 0 && gObj != null)
        {
            posInicial = gObj.transform.position;
            RaycastHit2D hit = GenerarRay();
            if (Input.GetTouch(0).phase == TouchPhase.Began &&hit.collider!=null && hit.transform.tag=="Posibles" && hit.transform.tag!="Obstacle")
            {
                Mover();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && hit.collider != null && gObj != null && hit.transform.tag== "Posibles" && hit.transform.tag != "Obstacle")//Cuando el dedo se empieza a mover sobre la pantalla
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
        if (pasos > maxPasos)
        {
            gObj.transform.position = posInicial;
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
    public void MostrarPosiblesPasos(Vector3 inicio,Queue<Vector3> Casillas, int contador)
    {
        inicio += new Vector3(0,1);
        Ray2D ray = new Ray2D(inicio, -Camera.main.gameObject.transform.position);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if(hit2D.collider != null && hit2D.transform.tag == "Board")
        {
            Casillas.Enqueue(inicio);
            var paso= Instantiate(Resources.Load("posibles"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);
            
        }
        inicio += new Vector3(0, -2);
        ray=new Ray2D(inicio, -Camera.main.gameObject.transform.position);
        hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Board")
        {
            Casillas.Enqueue(inicio);
            var paso = Instantiate(Resources.Load("posibles"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);
        }
        inicio += new Vector3(1, 1);
        ray = new Ray2D(inicio, -Camera.main.gameObject.transform.position);
        hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Board")
        {
            Casillas.Enqueue(inicio);
            var paso = Instantiate(Resources.Load("posibles"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);
        }
        inicio += new Vector3(-2,0);
        ray = new Ray2D(inicio, -Camera.main.gameObject.transform.position);
        hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit2D.collider != null && hit2D.transform.tag == "Board")
        {
            Casillas.Enqueue(inicio);
            var paso = Instantiate(Resources.Load("posibles"), inicio, Quaternion.identity, GameObject.Find("Posibles").transform);
        }
        if (quitados == puestos)
        {
            quitados = 0;
            puestos = Casillas.Count;
            contador++;
        }
        if (contador < maxPasos)
        {
            quitados++;
            MostrarPosiblesPasos(Casillas.Dequeue(), Casillas, contador);
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
