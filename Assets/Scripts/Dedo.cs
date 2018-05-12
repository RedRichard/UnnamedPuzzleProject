using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dedo : MonoBehaviour {
    GameObject gObj, gObjParent;
    // Update is called once per frame
    void Update () {
        Controlar();
	}
    void Controlar()
    {
        if (Input.touchCount > 0 && gObj == null)   //controles touch, no hace nada si no detecta al menos un dedo
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Cuando detecta el dedo lanza un ray 
            {
                RaycastHit2D hit2D = GenerarRay();
                if (hit2D && hit2D.transform.tag == "Player") //Si el objeto es un personaje jugable
                {
                    gObj = hit2D.transform.gameObject;//se guarda el objeto con el que el ray choco
                    gObjParent = hit2D.transform.parent.transform.gameObject; //Guardamos al Jugador
                    gObj.GetComponent<Personajes>().Mago();
                    gObj.GetComponent<Personajes>().Tocado();
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
            bool probar=script.ChecarR();
            print(probar);
            script.Enemigos.Clear();
            script.Lista.Clear();
            gObj = null;
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
    }
}
