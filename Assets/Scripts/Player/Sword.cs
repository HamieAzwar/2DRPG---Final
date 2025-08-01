using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float swordAttackCD = 0.5f;
    [SerializeField] private WeaponInfo weaponInfo;

    private Transform weaponCollider;
    private Animator myAnimator;
    private GameObject slashAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;

        // ✅ Ensure weapon collider starts off
        if (weaponCollider != null)
            weaponCollider.gameObject.SetActive(false);
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        myAnimator.SetTrigger("Attack");

        // ✅ Only activate collider when attacking
        if (weaponCollider != null)
            weaponCollider.gameObject.SetActive(true);

        // Spawn slash animation
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    // ✅ Called via Animation Event at END of attack animation
    public void DoneAttackingAnimEvent()
    {
        if (weaponCollider != null)
            weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        if (slashAnim != null)
        {
            slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (PlayerController.Instance.FacingLeft)
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        if (slashAnim != null)
        {
            slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (PlayerController.Instance.FacingLeft)
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            if (weaponCollider != null) weaponCollider.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            if (weaponCollider != null) weaponCollider.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
