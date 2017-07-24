using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

[System.Serializable]
public class Mission {

	public int highscore;
	public int coinReward;
	public bool completedOnce;
	[SerializeField]
	public PrimaryTask primaryTask;

	public void Init(GameManager gameManager, int loadedLevel) {
		XmlDocument doc = new XmlDocument ();
		doc.Load (Application.dataPath + "/Data/Missions.xml");
		XmlNode root = doc.FirstChild;
		XmlNode currentMission = null;

		if (root.HasChildNodes) {
			for (int i = 0; i < root.ChildNodes.Count; i++) {
				if (XmlConvert.ToInt32 (root.ChildNodes [i].Attributes.Item (0).Value) == loadedLevel) {
					currentMission = root.ChildNodes [i];
					break;
				}
			}
		}
			
		highscore = XmlConvert.ToInt32 (currentMission.ChildNodes [0].InnerText);
		coinReward = XmlConvert.ToInt32 (currentMission.ChildNodes [1].InnerText);
		completedOnce = XmlConvert.ToBoolean (currentMission.ChildNodes [2].InnerText);

		primaryTask.Init (gameManager, currentMission.ChildNodes [3]);
	}

	public void Update() {
		primaryTask.Update ();
	}

	public void NotifyEnemyDestroyed() {
		primaryTask.NotifyEnemyDestroyed ();
	}
}
