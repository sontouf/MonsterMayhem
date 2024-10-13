
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : SkillProjectiles
{
	private void Start()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;

		transform.position = pos;
		StartCoroutine(DeactivateAfterTime(3f));
	}

	IEnumerator DeactivateAfterTime(float delay)
	{
		// 5�� ��ٸ���
		yield return new WaitForSeconds(delay);

		// ������Ʈ ��Ȱ��ȭ
		transform.gameObject.SetActive(false);
	}
}
