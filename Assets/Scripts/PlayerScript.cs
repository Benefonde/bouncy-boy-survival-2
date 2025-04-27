using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        mouseSensitivity = (PlayerPrefs.GetFloat("sensitivity") + 5) / 10;
        if (mouseSensitivity == 0)
        { 
            mouseSensitivity = 2;
        }
        cam = Camera.main.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cc = GetComponent<CharacterController>();
        hp = 10;
        cameraRotation = cam.transform.rotation.eulerAngles;
        GetNewWeapon(air); // nothing
        GetNewArtifact(airtifact);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        PlayerMovement();
        GravityShits();
        if (hp < maxHp)
        {
            hp += regen * Time.deltaTime;
        }
        if (hp <= 0)
        {
            Time.timeScale = 0;
            died.SetActive(true);
        }
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        HudUpdates();
        if (Input.GetMouseButtonDown(0) && attackCooldown <= 0 && Time.timeScale != 0)
        {
            Attack();
            switch (weapon.name)
            {
                default: attackCooldown = 0.05f; break;
                case "Nothing": attackCooldown = 0.35f; break;
                case "Pan": attackCooldown = 0.35f; break;
                case "Spike": attackCooldown = 0.3f; break;
                case "Microphone": attackCooldown = 9f; break;
            }
        }
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        ArtifactUpdate();
    }

    void LateUpdate()
    {
        CameraMovement();
    }

    void ArtifactUpdate()
    {
        switch (artifactEquipped.name)
        {
            case "Campfire":
                Instantiate(artifactGameObjects[0], transform.position, transform.rotation).SetActive(true);
                GetNewArtifact(airtifact);
                break;
            case "Cobweb":
                if (cobwebCooldown <= 0)
                {
                    Instantiate(artifactGameObjects[1], transform.position, transform.rotation).SetActive(true);
                    artifactDurability--;
                    if (artifactDurability <= 0)
                    {
                        GetNewArtifact(airtifact);
                    }
                    cobwebCooldown = 10;
                }
                else
                {
                    cobwebCooldown -= Time.deltaTime;
                }
                break;
        }
    }

    void CameraMovement()
    {
        if (Time.timeScale != 0)
        {
            mouseRotation = new Vector2(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"));
            cameraRotation += -mouseRotation * mouseSensitivity;
            cam.transform.rotation = Quaternion.Euler(cameraRotation);
            cam.transform.position = transform.position + cameraOffset;
        }
    }

    void PlayerMovement()
    {
        speed = mainSpeed;
        transform.rotation = Quaternion.Euler(new Vector3(0, cameraRotation.y, 0));
        Vector3 x = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 y = transform.forward * Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && !jump)
        {
            transform.position += new Vector3(0, 5, 0);
            realGravity = 10;
            jump = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mainSpeed * 1.35f;
        }
        speed *= Mathf.Clamp(hp / maxHp, 0.65f, 1);
        cc.Move((x + y).normalized * Time.deltaTime * speed);
    }

    void GravityShits()
    {
        if (!cc.isGrounded)
        {
            realGravity -= gravity * Time.deltaTime;
        }
        else if (cc.isGrounded)
        {
            realGravity = 0;
            jump = false;
        }
        Vector3 vertical = new Vector3(0, realGravity, 0);
        cc.Move(vertical * Time.deltaTime);
        if (cc.isGrounded)
        {
            realGravity = 0;
            jump = false;
        }

        if (transform.position.y <= -10)
        {
            hp /= 5;
            transform.position = new Vector3(0, 10, 0);
        }
    }

    void HudUpdates()
    {
        if (PlayerPrefs.GetInt("healthAlt", 0) == 1)
        {
            healthSlider.value = Mathf.RoundToInt(hp);
            healthSlider.maxValue = maxHp;
        }
        else
        {
            healthSlider.value = healthSlider.maxValue;
        }
        healthImage.color = new Color(1, hp / maxHp, hp / maxHp, 1);
        switch (Mathf.Round((hp / maxHp) * 4) / 4)
        {
            case 0.25f: healthImage.sprite = healthSprites[3]; break;
            case 0.5f: healthImage.sprite = healthSprites[2]; break;
            case 0.75f: healthImage.sprite = healthSprites[1]; break;
            case 1: healthImage.sprite = healthSprites[0]; break;
        }
        weaponSprite.sprite = weapon.sprite;
        weaponDamageTxt.text = (weapon.damage + weaponDamageBonus).ToString();
        weaponDurabilitySlider.maxValue = weapon.durability + weaponDurabilityBonus;
        weaponDurabilitySlider.value = durability;

        artifactImage.sprite = artifactEquipped.sprite;
        artifactDurabilitySlider.maxValue = artifactEquipped.durability;
        artifactDurabilitySlider.value = artifactDurability; 
    }

    void GetNewWeapon(Weapon weaponCollect)
    {
        weapon = weaponCollect;
        durability = weapon.durability + weaponDurabilityBonus;
    }
    void GetNewArtifact(Artifact artifact)
    {
        artifactEquipped = artifact;
        artifactDurability = artifactEquipped.durability + weaponDurabilityBonus;
    }

    void Attack()
    {
        weaponAnim.SetTrigger(weapon.name);

        if (weapon.name == "Microphone")
        {
            singing = true;
            Invoke(nameof(NotSinging), 9);
        }

        if (!weapon.ranged)
        {
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Enemy");
            if (Physics.Raycast(transform.position, transform.forward, out hit, weapon.range, mask))
            {
                print("i hit him dhar mann");
                hit.transform.gameObject.GetComponent<EnemyScript>().health -= Mathf.RoundToInt(weapon.damage + weaponDamageBonus);
                if (hit.transform.gameObject.GetComponent<EnemyScript>().enemy.name.Contains("Spiky Boy") && artifactEquipped.name == "Cross")
                {
                    hit.transform.gameObject.GetComponent<EnemyScript>().health -= Mathf.RoundToInt(9 + weaponDamageBonus);
                    artifactDurability--;
                }
                if (artifactEquipped.name == "Lighter")
                {
                    StartCoroutine(hit.transform.gameObject.GetComponent<EnemyScript>().Fire(Random.Range(6, 14)));
                    artifactDurability--;
                }
                if (artifactDurability <= 0)
                {
                    GetNewArtifact(airtifact);
                }
                if (weapon.name != "Nothing")
                {
                    durability--;
                    if (durability <= 0)
                    {
                        GetNewWeapon(air);
                    }
                }
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4) && hit.transform.gameObject.GetComponent<WeaponPickup>() != null)
            {
                GetNewWeapon(hit.transform.gameObject.GetComponent<WeaponPickup>().me);
                Destroy(hit.transform.gameObject);
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4) && hit.transform.gameObject.GetComponent<ArtifactPickup>() != null)
            {
                GetNewArtifact(hit.transform.gameObject.GetComponent<ArtifactPickup>().me);
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            Instantiate(projectiles[weapon.projectile], transform.position, transform.rotation).SetActive(true);
            durability--;
            if (durability <= 0)
            {
                GetNewWeapon(air);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "Enemy(Clone)")
        {
            hp -= other.gameObject.GetComponent<EnemyScript>().damage * Time.deltaTime;
        }
    }

    void NotSinging()
    {
        singing = false;
    }

    public IEnumerator Cobweb()
    {
        mainSpeed /= 5;
        yield return new WaitForSeconds(5);
        mainSpeed *= 5;
    }

    CharacterController cc;

    GameObject cam;

    float mouseSensitivity;

    Vector3 mouseRotation;

    Vector3 cameraRotation;
    public Vector3 cameraOffset;

    [SerializeField]
    float speed;
    public float mainSpeed;
    public float gravity;
    [SerializeField]
    float realGravity;
    [SerializeField]
    bool jump;

    public int maxHp;
    public float hp;
    public float regen;
    public Weapon weapon;

    public Slider healthSlider;
    public Image healthImage;
    public Sprite[] healthSprites;
    public GameObject died;
    public Image weaponSprite;
    public TMP_Text weaponDamageTxt;
    public Slider weaponDurabilitySlider;

    public Animator weaponAnim;
    [SerializeField]
    int durability;
    [SerializeField]
    float artifactDurability;

    public GameObject[] projectiles;
    public GameObject[] artifactGameObjects;

    public Weapon air;
    public Artifact airtifact;

    float attackCooldown;

    public int weaponDamageBonus;
    public int weaponDurabilityBonus;

    public Artifact artifactEquipped;
    public Image artifactImage;
    public Slider artifactDurabilitySlider;
    float cobwebCooldown;

    public bool singing;
}
