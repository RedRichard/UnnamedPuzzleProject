using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueCamara : MonoBehaviour {

  public IEnumerator PlayAnimationsAttack(string name)
    {       
        name = string.Concat("Prefabs/Characters/Jugadores/", name,"Big");
        GameObject personaje = (GameObject) Instantiate(Resources.Load(name));
        yield return new WaitForSecondsRealtime(personaje.GetComponent<Animation>().clip.length);
        Destroy(personaje);
        GameObject.Find("MovementC").GetComponent<Touch>().atacaCamara.enabled=false;
    }
}
