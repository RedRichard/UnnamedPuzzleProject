using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour {
    GameObject gObj, gObjParent,enemigoSelected;
    float camaraHeight;
    Vector3 camPos;
    public GameObject Controlador;
    bool atacando=false;
    // Update is called once per frame
    public GameObject MoverButt, CancelarButt, AtaqueBut, NoataqueBut,MostarAEbut;
    void FixedUpdate () {
        Controlador.GetComponent<ControladorTurno>().ChecarTurno();
        if (Controlador.GetComponent<ControladorTurno>().turnoJugadores)
        {
            Controlar();
        }
        else
        {
            var enemigos = FindObjectsOfType<Enemigo>();
            foreach(Enemigo enemigo in enemigos)
            {
                enemigo.Atacar();
            }
            Controlador.GetComponent<ControladorTurno>().RegresarJugadores();
        }
        if (atacando)
        {
            Atacar();
        }

    }
    void Controlar()
    {
        if (Input.touchCount > 0 && gObj == null)   //controles touch, no hace nada si no detecta al menos un dedo
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Cuando detecta el dedo lanza un ray 
            {
                RaycastHit2D hit2D = GenerarRay();
                if (hit2D && hit2D.transform.tag == "Player" && hit2D.transform.gameObject.GetComponent<Personajes>().mago.turno) //Si el objeto es un personaje jugable
                {
                    gObj = hit2D.transform.gameObject;//se guarda el objeto con el que el ray choco
                    gObjParent = hit2D.transform.parent.transform.gameObject; //Guardamos al Jugador
                    gObj.GetComponent<Personajes>().Tocado();
                    MoverButt.SetActive(true);
                    CancelarButt.SetActive(true);
                    MostarAEbut.SetActive(false);
                }
            }
        }
        if (Input.touchCount > 0 && gObj != null)
        {
            RaycastHit2D hit = GenerarRay();
            if (Input.GetTouch(0).phase == TouchPhase.Began && hit.collider != null && hit.transform.tag == "Posibles" && hit.transform.tag != "Obstacle")
            {
                Mover();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && hit.collider != null && gObj != null && hit.transform.tag == "Posibles" && hit.transform.tag != "Obstacle")//Cuando el dedo se empieza a mover sobre la pantalla
            {
                Mover();
            }
        }
    }
    void Mover()
    {
        Vector3 end;
        end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);//la posicion del dedo
        end = new Vector3(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y));//se redondea la posicion del dedo, para manejarlo bien con el grid
        gObj.transform.position = end;//se mueve el fantasma a la posicion del dedo
    }
    RaycastHit2D GenerarRay()
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Camera.main.transform.forward);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
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
            GameObject posibles = GameObject.Find("Posibles");
            for (int i = 0; i < posibles.transform.childCount; i++)
            {
                Destroy(posibles.transform.GetChild(i).gameObject);
            }
            var script= gObj.GetComponent<Personajes>();
            script.Lista.Clear();
            atacando=script.ChecarR();
            if (atacando)
            {
                camaraHeight = Camera.main.orthographicSize;
                camPos = Camera.main.transform.position;
                Camera.main.orthographicSize = 3;
                Camera.main.transform.position = new Vector3(gObj.transform.position.x, gObj.transform.position.y, Camera.main.transform.position.z);
                NoataqueBut.SetActive(true);
            }
            else
            {
                script.enemigos.Clear();
                script.Lista.Clear();
                script.mago.turno = false;
                gObj = null;
                MostarAEbut.SetActive(true);
            }
        }
        CancelarButt.SetActive(false);
        MoverButt.SetActive(false);
        var enemigos = FindObjectsOfType<Enemigo>();
        foreach (Enemigo enemigo in enemigos)
        {
            enemigo.QuitarAtacZone();
        }
    }
    public void CancelarBut()
    {
        if (gObj != null)
        {
            var script = gObj.GetComponent<Personajes>();
            script.Lista.Clear();
            gObj.transform.localPosition = new Vector3();
            gObj = null;
            GameObject posibles = GameObject.Find("Posibles");
            for (int i = 0; i < posibles.transform.childCount; i++)
            {
                Destroy(posibles.transform.GetChild(i).gameObject);
            }

        }
        MostarAEbut.SetActive(true);
        MoverButt.SetActive(false);
        CancelarButt.SetActive(false);

    }
    public void NoAtacar()
    {
        NoataqueBut.SetActive(false);
        AtaqueBut.SetActive(false);
        Camera.main.orthographicSize = camaraHeight;
        Camera.main.transform.position = camPos;
        var script = gObj.GetComponent<Personajes>();
        script.enemigos.Clear();
        script.Lista.Clear();
        script.mago.turno = false;
        gObj = null;
        MostarAEbut.SetActive(true);
        atacando = false;

    }
    public void Atacar()
    {
        var script = gObj.GetComponent<Personajes>();
        print(script.enemigos.Count);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit2D = GenerarRay();
            if (script.enemigos.Contains(hit2D.collider.gameObject))
            {
                Destroy(hit2D.collider.gameObject.transform.parent.gameObject);
                atacando = false;
                NoataqueBut.SetActive(false);
                Camera.main.orthographicSize = camaraHeight;
                Camera.main.transform.position = camPos;
                script.enemigos.Clear();
                script.Lista.Clear();
                script.mago.turno = false;
                gObj = null;
                MostarAEbut.SetActive(true);
            }
        }
    }
    public void MostrarAe()
    {
        var Enemigos = FindObjectsOfType<Enemigo>();
        foreach(Enemigo enemigo in Enemigos)
        {
            enemigo.MostrarR();
        }
    }
}
