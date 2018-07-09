using UnityEngine;
using Pathfinding.Util;
public class Touch : MonoBehaviour {

    GameObject gObj, gObjParent,enemigoSelected;
    float camaraHeight;
    Vector3 camPos;
    public GameObject Controlador;
    bool atacando=false,radio=false;
    public GameObject MoverButt, CancelarButt, HabilidadBut, NoataqueBut,MostarAEbut,NadaButt;
    private void Awake()
    {
        camaraHeight = Camera.main.orthographicSize;
        camPos = Camera.main.transform.position;
    }
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
        if (radio)
        {
            UsarHabilidad();
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
            var astar = gObjParent.GetComponent<Pathfinding.AILerp>();
            astar.SearchPath();
            gObj.GetComponent<SpriteRenderer>().enabled = false;
            GameObject posibles = GameObject.Find("Posibles");
            for (int i = 0; i < posibles.transform.childCount; i++)
            {
                Destroy(posibles.transform.GetChild(i).gameObject);
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
    public void Setpos()
    {
        Vector3 moverPos = gObjParent.transform.position;
        moverPos.z = -1;
        gObjParent.transform.position = moverPos;
        gObj.transform.localPosition = new Vector3();
        gObj.GetComponent<SpriteRenderer>().enabled = true;
        var script = gObj.GetComponent<Personajes>();
        script.Lista.Clear();
        atacando = script.ChecarR();
        if (atacando)
        {
            Camera.main.orthographicSize = 3;
            Camera.main.transform.position = new Vector3(gObj.transform.position.x, gObj.transform.position.y, Camera.main.transform.position.z);
            NoataqueBut.SetActive(true);
        }
        var habilidad = gObj.GetComponent<Habilidad>();
        radio = habilidad.ChecarRadio();
        if (radio)
        {
            Camera.main.orthographicSize = 3;
            Camera.main.transform.position = new Vector3(gObj.transform.position.x, gObj.transform.position.y, Camera.main.transform.position.z);
            HabilidadBut.SetActive(true);
        }
        if (atacando && radio)
        {
            HabilidadBut.SetActive(false);
            NoataqueBut.SetActive(false);
            NadaButt.SetActive(true);
        }
        if (!atacando && !radio)
        {
            script.mago.turno = false;
            gObj = null;
            MostarAEbut.SetActive(true);
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
        HabilidadBut.SetActive(false);
        Camera.main.orthographicSize = camaraHeight;
        Camera.main.transform.position = camPos;
        var script = gObj.GetComponent<Personajes>();
        script.enemigos.Clear();
        script.Lista.Clear();
        var habilidad = gObj.GetComponent<Habilidad>();
        habilidad.personajes.Clear();
        habilidad.casillas.Clear();
        radio = false;
        script.mago.turno = false;
        gObj = null;
        MostarAEbut.SetActive(true);
        atacando = false;
        HabilidadBut.SetActive(false);

    }
    void Atacar()
    {
        var script = gObj.GetComponent<Personajes>();
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit2D = GenerarRay();
            if (hit2D && script.enemigos.Contains(hit2D.collider.gameObject))
            {
                Destroy(hit2D.collider.gameObject.transform.parent.gameObject);
                atacando = false;
                radio = false;
                NoataqueBut.SetActive(false);
                Camera.main.orthographicSize = camaraHeight;
                Camera.main.transform.position = camPos;
                script.enemigos.Clear();
                script.Lista.Clear();
                var habilidad = gObj.GetComponent<Habilidad>();
                habilidad.personajes.Clear();
                habilidad.casillas.Clear();
                script.mago.turno = false;
                gObj = null;
                MostarAEbut.SetActive(true);
                HabilidadBut.SetActive(false);
                NadaButt.SetActive(false);
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
    void UsarHabilidad()
    {
        var habilidad = gObj.GetComponent<Habilidad>();
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit2D = GenerarRay();
            if (hit2D && habilidad.personajes.Contains(hit2D.collider.gameObject))
            {
                habilidad.RealizarHabilidad(hit2D.collider.gameObject);
                atacando = false;
                radio = false;
                NoataqueBut.SetActive(false);
                HabilidadBut.SetActive(false);
                NadaButt.SetActive(false);
                MostarAEbut.SetActive(true);
                habilidad.personajes.Clear();
                habilidad.casillas.Clear();
                var script = gObj.GetComponent<Personajes>();
                script.enemigos.Clear();
                script.Lista.Clear();
                script.mago.turno = false;
                gObj = null;
                Camera.main.orthographicSize = camaraHeight;
                Camera.main.transform.position = camPos;
            }
        }
    }
    public void Nada()
    {
        Camera.main.orthographicSize = camaraHeight;
        Camera.main.transform.position = camPos;
        var script = gObj.GetComponent<Personajes>();
        script.enemigos.Clear();
        script.Lista.Clear();
        var habilidad = gObj.GetComponent<Habilidad>();
        habilidad.personajes.Clear();
        habilidad.casillas.Clear();
        radio = false;
        script.mago.turno = false;
        gObj = null;
        MostarAEbut.SetActive(true);
        atacando = false;
        NadaButt.SetActive(false);
    }
    public void NoHabilidad()
    {
        Camera.main.orthographicSize = camaraHeight;
        Camera.main.transform.position = camPos;
        var habilidad = gObj.GetComponent<Habilidad>();
        var script = gObj.GetComponent<Personajes>();
        habilidad.personajes.Clear();
        habilidad.casillas.Clear();
        radio = false;
        script.mago.turno = false;
        gObj = null;
        MostarAEbut.SetActive(true);
        HabilidadBut.SetActive(false);
    }
}