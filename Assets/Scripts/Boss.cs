using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public Rigidbody2D target;

    public float meleeDamage; // ���� ���� ����
    public float meleeCooldown = 5f; // ���� ���� ��Ÿ��
    public float meleeConeAngle = 90f; // ���� ���� ����
    public float meleeRange = 3f; // ���� ���� ����
    private float lastMeleeTime;

    public float aoeDamage; // ���Ÿ� ���� ���� ���� ����
    public float aoeRadius = 3f; // ���Ÿ� ���� ����
    public float aoeCooldown = 8f; // ���Ÿ� ���� ���� ��Ÿ��
    private float lastAoeTime;

    public GameObject aoeMarkerPrefab; // ���� ���� ��Ŀ ������
    public GameObject coneMarkerPrefab; // ���� ���� ��Ŀ ������
    public float warningTime = 1f; // ���� �� ��� �ð�

    public GameObject projectilePrefab; // ����ü ������
    public float projectileSpeed; // ����ü �ӵ�
    public float projectileCooldown = 6f; // ����ü ��Ÿ��
    private float lastProjectileTime;

    // ������� �����ϴ� ���� �Ѿ�
    public float projectile2Speed = 5f; // �Ѿ� �ӵ�
    public int projectile2Count = 12; // �߻��� �Ѿ� ��
    public float projectile2Cooldown = 6f;
    private float lastProjectile2Time;

    public float dashSpeedMultiplier = 3f; // ��� �� �ӵ� ����
    public float dashDuration = 0.5f; // ��� ���� �ð�
    public float dashCooldown = 10f; // ��� ��Ÿ��
    private float lastDashTime;
    private bool isDashing = false;

    public float collisionDamage = 10f; // �÷��̾�� �浹 �� ���ط�

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    Collider2D coll;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        if (!isDashing)
        {
            // �÷��̾ ���� �̵�
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }

        // ���� ���� ����
        ExecuteAttackPattern();
    }

    void LateUpdate()
    {
        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // �پ��� ���� ������ ����
    void ExecuteAttackPattern()
    {
        // ���� ���� ���� ����
        if (Time.time - lastMeleeTime >= meleeCooldown)
        {
            StartCoroutine(PerformMeleeConeAttack());
            lastMeleeTime = Time.time;
        }

        // ���Ÿ� ���� ���� ����
        if (Time.time - lastAoeTime >= aoeCooldown)
        {
            StartCoroutine(PerformAoeAttack());
            lastAoeTime = Time.time;
        }

        // ���Ÿ� ����ü ����
        if (Time.time - lastProjectileTime >= projectileCooldown)
        {
            PerformRangedProjectileAttack();
            lastProjectileTime = Time.time;
        }
        // ���� ����ü ����
        if (Time.time - lastProjectile2Time >= projectile2Cooldown)
        {
            PerformCircularProjectileAttack();
            lastProjectile2Time = Time.time;
        }

        // ��� �̵�
        if (Time.time - lastDashTime >= dashCooldown && !isDashing)
        {
            StartCoroutine(PerformDashAttack());
            lastDashTime = Time.time;
        }
    }
    // �÷��̾���� ���� ��� (���� ���� ������ ����)
    float GetAngleToPlayer()
    {
        Vector2 direction = (target.position - rigid.position).normalized;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    IEnumerator PerformMeleeConeAttack()
    {
        // ���� ��Ŀ ǥ�� (������ �ڽ����� ����)
        GameObject coneMarker = Instantiate(coneMarkerPrefab, transform.position, Quaternion.identity);
        coneMarker.transform.SetParent(transform); // ������ �θ�� ������ �Բ� �����̰� ��
        coneMarker.transform.localPosition = Vector3.zero; // ��Ŀ�� ������ ��ġ�� ��Ȯ�� �µ���

        float timer = 0f; // ��� �ð� Ÿ�̸�
        float attackAngle = 0f;

        // ��� �ð� �� ���� ��Ŀ�� ������ ���������� �÷��̾� �������� ������Ʈ
        while (timer < warningTime)
        {
            if (timer < warningTime / 2)
            {
                // �÷��̾��� ���� ��ġ�� �������� ��Ŀ ȸ��
                coneMarker.transform.rotation = Quaternion.Euler(0, 0, GetAngleToPlayer());
                attackAngle = GetAngleToPlayer(); // ������ ���� ����
            }
            timer += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        // ��� �ð��� ������ ��Ŀ ����
        Destroy(coneMarker);

        // ���� ����
        float currentAngle = GetAngleToPlayer(); // ���� ���� �÷��̾�� ���� ����
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(attackAngle, currentAngle));

        if (angleDifference <= meleeConeAngle / 2 && Vector2.Distance(target.position, rigid.position) <= meleeRange)
        {
            Player player = target.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("MeeleeConeAttackHit");
                //anim.SetTrigger("MeleeConeAttack");
                GameManager.instance.health -= meleeDamage;
            }
        }
    }

    // ���Ÿ� ���� ���� ���� �Լ�
    IEnumerator PerformAoeAttack()
    {
        Vector2 attackPos = target.transform.position;
        // ���� ��Ŀ ǥ��
        GameObject aoeMarker = Instantiate(aoeMarkerPrefab, attackPos, Quaternion.identity);
        //aoeMarker.transform.localScale = new Vector3(aoeRadius, aoeRadius, 1); // ������ ���� ũ�� ����
        yield return new WaitForSeconds(warningTime); // ��� �ð� ���

        Destroy(aoeMarker); // ��Ŀ ����

        // ���� ����
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPos, aoeRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Player player = hitCollider.GetComponent<Player>();
                if (player != null)
                {
                    Debug.Log("AoeAttackHit");
                    //player.TakeDamage(aoeDamage);
                    GameManager.instance.health -= aoeDamage;
                }
            }
        }

        //anim.SetTrigger("AoeAttack");
    }

    // ���Ÿ� ����ü ���� �Լ�
    void PerformRangedProjectileAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        Vector2 direction = (target.position - rigid.position).normalized;
        projectileRb.velocity = direction * projectileSpeed;

        //anim.SetTrigger("ProjectileAttack");
    }

    // ������� ����ü ���� �Լ�
    void PerformCircularProjectileAttack()
    {
        float angleStep = 360f / projectile2Count; // �� �Ѿ� ������ ����
        float currentAngle = 0f; // ù �Ѿ��� ���� ������

        for (int i = 0; i < projectile2Count; i++)
        {
            // ���� ������ ���� ���� ���
            float projectileDirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 direction = new Vector2(projectileDirX, projectileDirY).normalized;

            // �Ѿ� ����
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * projectile2Speed; // �Ѿ��� ����� �ӵ� ����

            currentAngle += angleStep; // ���� ����
        }
    }

    // ��� �̵�(���) �Լ�
    IEnumerator PerformDashAttack()
    {
        isDashing = true; // ��� ����
        Vector2 dashDirection = (target.position - rigid.position).normalized;
        rigid.velocity = dashDirection * speed * dashSpeedMultiplier;

        //anim.SetTrigger("DashAttack");

        yield return new WaitForSeconds(dashDuration); // ��� ���� �ð���ŭ ��ٸ�

        rigid.velocity = Vector2.zero; // ��� �� �ӵ� �ʱ�ȭ
        isDashing = false; // ��� ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            //Dead();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player") || !isLive)
            return;
        Player player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            //player.TakeDamage(collisionDamage); // �浹 �� �÷��̾�� ���� ������
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

}