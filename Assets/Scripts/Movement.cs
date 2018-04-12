using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public Text text;
    GameObject gobj, gObjMain;
    Vector2 start, end, currTouchPos;
    Vector3 trPos, iniPos,mo;  //Cambié ini a iniPos
    public int pasos=0, maxPasos;
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
                try{
                    if (hit2D.transform.tag=="Player" && gobj==null) //Si el objeto es un personaje jugable
                    {
                        print("Funciona");
                        gobj = hit2D.transform.gameObject;
                        trPos = gobj.transform.position;
                        iniPos = hit2D.transform.parent.transform.position; //Se guarda la posicion original del jugador
                        gObjMain = hit2D.transform.parent.transform.gameObject;
                    } else if (gobj && hit2D.transform.tag=="Board"){
                        print(hit2D.transform.tag);
                        trPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        trPos = new Vector3(Mathf.RoundToInt( trPos.x), Mathf.RoundToInt(trPos.y), Mathf.RoundToInt(trPos.z)+10f);
                        trPos.x -= .5f;
                        trPos.y -= .5f;
                        gobj.transform.position = trPos;
                        currTouchPos = gobj.transform.position;                        
                    }
                } catch (System.Exception e){
                    // gobj.transform.position = iniPos;
                    // gobj = null;
                    // gObjMain = null;
                    // Debug.Log(e.ToString());
                }
            }
            // if (Input.GetTouch(0).phase == TouchPhase.Moved){
            //     currTouchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            // }
        }
        // if (gobj)//Cuando el dedo se empieza a mover sobre la pantalla
        // {
        //     start = gobj.transform.position;//la posicion del fantasma
        //     end = Camera.main.ScreenToWorldPoint(currTouchPos);//la posicion del dedo
        //     disx = Mathf.RoundToInt(end.x - start.x);//se verica las diferencias en las posiciones y cuando se
        //     disy = Mathf.RoundToInt(end.y - start.y);//acerca a uno, es cuando se mueve al siguiente tile
        //     if (disx == 1 || disx == -1)
        //     {
        //         trPos.x = start.x + disx;
        //         gobj.transform.position = trPos;
        //         if (Mathf.Abs(iniPos.x - trPos.x) > pasos - Mathf.Abs(iniPos.y - trPos.y))
        //         {
        //             pasos++;
        //         }
        //         else
        //         {
        //             pasos--;
        //         }
        //         if (pasos > maxPasos)//si se supera el numero de pasos
        //         {
        //             trPos.x = start.x;
        //             gobj.transform.position = start;//el fantasma regresa a la posicion anterior
        //             pasos--;
        //         }
        //     }
        //     if (disy == 1 || disy == -1)
        //     {
        //         trPos.y = start.y + disy;
        //         gobj.transform.position = trPos;
        //         if (Mathf.Abs(iniPos.y - trPos.y) > pasos - Mathf.Abs(iniPos.x - trPos.x))
        //         {
        //             pasos++;
        //         }
        //         else
        //         {
        //             pasos--;
        //         }
        //         if (pasos > maxPasos)
        //         {
        //             trPos.y = start.y;
        //             gobj.transform.position = start;
        //             pasos--;
        //         }
        //     }
        //     text.text = pasos.ToString();
        // }
    }

    RaycastHit2D GenerarRay()
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position),-Camera.main.gameObject.transform.position);
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction);
        return hit2D;
    }
    public void Mover()
    {
        if (gObjMain){
            gObjMain.transform.position = currTouchPos;
            gobj.transform.position = currTouchPos;
            gobj = null;
            gObjMain = null;
        }
        // gobj.transform.parent.transform.position = gobj.transform.position;
        // gobj.transform.localPosition = new Vector3();
    }
    public void Cancelar()
    {
        gobj.transform.localPosition = new Vector3();
        gobj = null;
    }

    void OnCollisionEnter2D (Collision2D col){
        print("Choca");
    }
}
