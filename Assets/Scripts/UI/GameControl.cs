using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameControl : MonoBehaviour {
	
	public static GameControl control;
	public int unlockedLev;

	void Awake () {
		if(control == null){
			DontDestroyOnLoad(gameObject);
			control = this;
		}
		else if (control != this){
			Destroy(gameObject);
		}
	} 

	public void LoadScene(string sceneName){
		Application.LoadLevel(sceneName);
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData();
		data.unlockedLevels = unlockedLev;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load(){
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData) bf.Deserialize(file);
			file.Close();

			unlockedLev = data.unlockedLevels;
		}
	}
}
