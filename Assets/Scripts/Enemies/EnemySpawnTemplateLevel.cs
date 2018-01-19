using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnTemplateLevel {

	public float minElapsedTime;
	public int minSkillLevel;
	public EnemySpawnTemplate[] spawnTemplates;
}
