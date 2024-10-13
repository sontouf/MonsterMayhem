/*using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Weapon : MonoBehaviour
{
	public int id;
	public int prefabId;
	public float damage;
	public int count;
	public float speed;
	GameObject curBullet;

	float timer;
	Player player;

	private void Awake()
	{
		player = GameManager.instance.player;
	}

	void Update()
	{
		switch (id)
		{
			case 0:
				transform.Rotate(Vector3.back * speed * Time.deltaTime);
				break;
			default:
				timer += Time.deltaTime;

				if (timer > speed)
				{
					timer = 0f;
					Fire();
				}
				break;
		}

*//*		if (Input.GetButtonDown("Jump"))
		{
			LevelUp(10, 1);
		}*//*
	}

	public void LevelUp(float damage, int count, int level)
	{
		this.damage += damage;
		this.count += count;

		if (id == 0)
			Batch();
		Debug.Log("level : " + level);
		Animator anim = curBullet.GetComponent<Animator>();
		anim.SetInteger("level", level);
	}

	public void Init(SkillData data)
	{
		// Basic Set
		name = "Skill " + data.skillId;
		transform.parent = player.transform;
		transform.localPosition = Vector3.zero;


		// Property Set
		id = data.skillId;
		damage = data.baseDamage;
		count = data.baseCount;

		for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
		{
			if (data.projectiles[i] == GameManager.instance.poolManager.prefabs[i])
			{
				prefabId = i;
				break;
			}
		}
		
		switch (id)
		{
			case 0:
				speed = -150;
				Batch();
				break;

			case 1:
				speed = 0.3f;
				//Batch();
				break;

			default:
				speed = 0.3f;
				break;
		}
	}

	void Batch()
	{
		for (int index = 0; index < count; index++)
		{
			Transform bullet;
			
			if (index < transform.childCount)
			{
				bullet = transform.GetChild(index);
			} else
			{
				bullet = GameManager.instance.poolManager.GetObject(prefabId).transform;
				bullet.parent = transform;
			}
			
			bullet.parent = transform;
			bullet.localPosition = Vector3.zero;
			bullet.localRotation = Quaternion.identity;
			Vector3 rotVec = Vector3.forward * 360 * index / count;
			bullet.Rotate(rotVec);
			bullet.Translate(bullet.up * 1.5f, Space.World);

			bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -1 is infinity per
		}
	}

	void Fire()
	{
		if (!player.scanner.nearestTarget)
			return;
		Vector3 targetPos = player.scanner.nearestTarget.position;
		Vector3 dir= targetPos - transform.position;
		dir = dir.normalized;
		curBullet = GameManager.instance.poolManager.GetObject(prefabId);
		Transform bullet = curBullet.transform;
		bullet.position = transform.position;
		bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
		bullet.GetComponent<Bullet>().Init(damage, count, dir);
	}
}
*/