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
        aud = GetComponent<AudioSource>();
        weaponAud = transform.GetChild(0).GetComponent<AudioSource>();
        hp = 10;
        cameraRotation = cam.transform.rotation.eulerAngles;
        if (PlayerPrefs.GetInt("endless", 0) == 1)
        {
            weapon = allWeapons[PlayerPrefs.GetInt("endlessWeapon")];
            artifact = allArtifacts[PlayerPrefs.GetInt("endlessArtifact")];
        }
        GetNewWeapon(weapon); // usually nothing
        GetNewArtifact(artifact);
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
            if (artifact.name == "Cross")
            {
                cc.enabled = false;
                transform.position = new Vector3(0, 10, 0);
                cc.enabled = true;
                maxHp = 10;
                hp = maxHp / 4;
                mainSpeed = 12;
                realGravity = 20;
                jump = true;
                GetNewArtifact(airtifact);
                return;
            }
            Time.timeScale = 0;
            died.SetActive(true);
            PlayerPrefs.SetInt("WaveNumber", FindObjectOfType<WaveScript>().wave);
        }
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        HudUpdates();
        if (Input.GetMouseButtonDown(0) && attackCooldown <= 0 && Time.timeScale != 0)
        {
            switch (weapon.name)
            {
                default: attackCooldown = 0.05f; break;
                case "Nothing": attackCooldown = 0.35f; break;
                case "Pan": attackCooldown = 0.35f; break;
                case "Spike": attackCooldown = 0.3f; break;
                case "Microphone": attackCooldown = 9f; break;
            }
            cooldownSider.maxValue = attackCooldown;
            Attack();
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
        switch (artifact.name)
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
            case "Fries and Meat":
                artifactDurability -= Time.deltaTime;
                if (artifactDurability <= 0)
                {
                    maxHp += 5;
                    hp += 5;
                    regen += 0.5f;
                    GetNewArtifact(airtifact);
                }
                break;
        }
    }

    void CameraMovement()
    {
        if (hp > 0)
        {
            if (Time.timeScale != 0)
            {
                mouseRotation = new Vector2(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"));
                cameraRotation += -mouseRotation * mouseSensitivity;
                cameraRotation = new Vector2(Mathf.Clamp(cameraRotation.x, -80, 80), cameraRotation.y);
                cam.transform.rotation = Quaternion.Euler(cameraRotation);
                cam.transform.position = transform.position + cameraOffset;
                arealight.position = cam.transform.position;
            }
        }
        else
        {
            if (possibleKriller == null)
            {
                possibleKriller = transform;
            }
            gameObject.layer = 0;
            cam.transform.LookAt(possibleKriller);
            cam.transform.position = possibleKriller.transform.position + (transform.forward * 3.5f) + (transform.up * 2.5f);
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
        if (artifact.name == "Nothing")
        {
            speed += 0.15f;
        }
        cc.Move((x + y).normalized * Time.deltaTime * speed);
        if (cc.velocity.magnitude > 0.15f && !jump)
        {
            if (footstepTimer > 0)
            {
                footstepTimer -= Time.deltaTime;
            }
            else
            {
                FootstepSound();
                footstepTimer = 0.4f - (mainSpeed / 130);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    footstepTimer = 0.3f - (mainSpeed / 120);
                }
            }
        }
    }

    void FootstepSound()
    {
        aud.pitch = Random.Range(0.8f, 1.2f);
        aud.PlayOneShot(walk, 0.3f);
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
        if (weapon.name == "Crossbow")
        {
            weaponDurabilitySlider.maxValue += weaponDurabilityBonus * 4;
        }
        weaponDurabilitySlider.value = durability;

        artifactImage.sprite = artifact.sprite;
        artifactDurabilitySlider.maxValue = artifact.durability;
        artifactDurabilitySlider.value = artifactDurability;

        cooldownSider.value = attackCooldown;
    }

    void GetNewWeapon(Weapon weaponCollect)
    {
        if (weaponCollect == null)
        {
            weaponCollect = air;
        }
        weapon = weaponCollect;
        durability = weapon.durability + weaponDurabilityBonus;
        if (weapon.name == "Crossbow")
        {
            durability += weaponDurabilityBonus * 4;
        }
    }
    void GetNewArtifact(Artifact artifacte)
    {
        if (artifacte == null)
        {
            artifacte = airtifact;
        }
        artifact = artifacte;
        artifactDurability = artifacte.durability + weaponDurabilityBonus;
    }

    void Attack()
    {
        weaponAnim.SetTrigger(weapon.name);

        if (weapon.name == "Microphone")
        {
            singing = 1;
            Invoke(nameof(NotSinging), 9);
        }
        else if (weapon.name != "Crossbow")
        {
            weaponAud.PlayOneShot(woosh);
        }
        else
        {
            weaponAud.PlayOneShot(crossbow[Random.Range(0, crossbow.Length)]);
        }

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3.25f) && hit.transform.gameObject.GetComponent<ProjectileScript>() != null)
        {
            if (!hit.transform.gameObject.GetComponent<ProjectileScript>().player)
            {
                print("You parried him dhar mann");
                hit.transform.gameObject.GetComponent<ProjectileScript>().Parry();
                weaponAud.PlayOneShot(parry[0]);
                if (Random.Range(1, 28) == 5)
                {
                    weaponAud.PlayOneShot(parry[1]);
                }
                durability -= 2;
                if (durability <= 0)
                {
                    GetNewWeapon(air);
                }
            }
        }
        if (!weapon.ranged)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, weapon.range) && hit.transform.gameObject.GetComponent<EnemyScript>() != null)
            {
                print("i hit him dhar mann");
                hit.transform.gameObject.GetComponent<EnemyScript>().health -= Mathf.RoundToInt(weapon.damage + weaponDamageBonus);
                weaponAud.PlayOneShot(impact);
                if (artifact.name == "Bloodthirst Spike")
                {
                    hp += Mathf.RoundToInt((weapon.damage + weaponDamageBonus) / 5);
                    if (Random.Range(1, 10) == 4)
                    {
                        maxHp++;
                    }
                    artifactDurability--;
                }
                if (hit.transform.gameObject.GetComponent<EnemyScript>().enemy.enemyName == artifact.enemyEffective)
                {
                    hit.transform.gameObject.GetComponent<EnemyScript>().health -= Mathf.RoundToInt(9 + weaponDamageBonus);
                    artifactDurability--;
                }
                if (artifact.name == "Lighter")
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

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5) && hit.transform.gameObject.GetComponent<WeaponPickup>() != null)
        {
            GetNewWeapon(hit.transform.gameObject.GetComponent<WeaponPickup>().me);
            Destroy(hit.transform.gameObject);
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5) && hit.transform.gameObject.GetComponent<ArtifactPickup>() != null)
        {
            GetNewArtifact(hit.transform.gameObject.GetComponent<ArtifactPickup>().me);
            Destroy(hit.transform.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "Enemy(Clone)" && other.gameObject.GetComponent<EnemyScript>() != null)
        {
            hp -= other.gameObject.GetComponent<EnemyScript>().damage * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            possibleKriller = other.transform;
        }
    }

    void NotSinging()
    {
        singing = 0;
    }

    public IEnumerator Cobweb()
    {
        if (artifact.name != "Cobweb")
        {
            mainSpeed /= 1.3f;
            yield return new WaitForSeconds(2);
            mainSpeed *= 1.3004f;
        }
    }

    public void Stop()
    {
        playerEnding = GameObject.Find("PlayerEnding");
        GetComponent<SpriteRenderer>().enabled = false;
        playerEnding.transform.SetPositionAndRotation(transform.position, transform.rotation);
        Destroy(gameObject); // NOOOOOOO
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
    public Artifact artifact;

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

    public Image artifactImage;
    public Slider artifactDurabilitySlider;
    float cobwebCooldown;

    public int singing;

    public Slider cooldownSider;

    public Transform possibleKriller;

    public Transform arealight;

    AudioSource aud;

    public AudioSource weaponAud;
    
    public AudioClip walk;
    [SerializeField]
    float footstepTimer;

    public AudioClip woosh;
    public AudioClip impact;
    public AudioClip[] parry;
    public AudioClip[] crossbow;

    public Weapon[] allWeapons;
    public Artifact[] allArtifacts;

    public GameObject playerEnding;
}
