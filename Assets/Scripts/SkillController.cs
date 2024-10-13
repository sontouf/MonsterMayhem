using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
	public Skill[] skills; // ��ų �迭 5��
	public Button[] skillButtons; // ��ų ��ư �迭 5��
	public int selectedSkillIndex = -1; // ���� ���õ� ��ų �ε���
	public int prevSelectedSkillIndex;
	
	public GameObject skillRangeInstance;

	void Start()
	{
		// ��ư Ŭ�� �̺�Ʈ ���
		for (int i = 0; i < skillButtons.Length; i++)
		{
			int index = i; // Ŭ���� ���� ����
			skillButtons[i].onClick.AddListener(() => ButtonClick(index)); // Button Component�� onclick event mapping
		}
	}

	void Update()
	{
		for (int i = 0; i < 5; i++)
		{
			if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
			{
				selectedSkillIndex = i;
				break;
			}
		} // ���⼭ ��ų�� ���õǾ� ��.
		if (selectedSkillIndex != -1 && skills[selectedSkillIndex].level != 0 && skillRangeInstance == null)
		{
			skillRangeInstance = skills[selectedSkillIndex].ShowSkillRange();
			prevSelectedSkillIndex = selectedSkillIndex;
		}
		else if (selectedSkillIndex != -1 && skills[selectedSkillIndex].level != 0 && skillRangeInstance != null)
		{
			if (prevSelectedSkillIndex != selectedSkillIndex)
			{
				skillRangeInstance.SetActive(false);
				skillRangeInstance = skills[selectedSkillIndex].ShowSkillRange();
				prevSelectedSkillIndex = selectedSkillIndex;
			}
		}
		// level 1�̻�, ��ų ������ �� ��Ȳ�� ����Ǿ�� ��.
		if (selectedSkillIndex != -1 && skills[selectedSkillIndex].level != 0 && skillRangeInstance != null && Input.GetMouseButtonDown(0)) // ��ų������ �� ���¿��� ȭ���� Ŭ���ϸ�.
		{
			skillRangeInstance.SetActive(false); // ���� ����
			skillRangeInstance = null;
			skills[selectedSkillIndex].UseSkill(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // ��ų ���
			selectedSkillIndex = -1; // ��� �� ���� �ʱ�ȭ
		}


	}

	private void ButtonClick(int skillIndex)
	{
		selectedSkillIndex = skillIndex;
		if (skills[selectedSkillIndex].level == 0)
			return;
		// ��ų ���� ǥ��
		if (skillRangeInstance == null)
			skillRangeInstance = skills[skillIndex].ShowSkillRange();
	}

}