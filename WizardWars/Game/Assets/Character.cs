﻿using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
    public Projectile defaultWeapon;
    public Projectile fireballSpawn;
    public RuneRep runeRepresentation;
    public int gameType;
    public int teamNum;
	private Controller controller;
	private int numKills, playerId;
	private bool invincible;
	private static int currentId = 0;
    private float invinTime = 1;
    private float invinCountdown;
    private Vector3 spawn;
    public Color baseColor;
    private Color InvincibilityColor = new Color(1, 1, 1, 1);
    private float flickerTimer = 0;
    public bool isJuggernaut;
    public bool diedLastFrame;
    public bool isDead;

    public GameObject doorPopUp;

    public LighteningSpawn spawnEffect;

	public Utility util;

	public int getNumKills()
	{
		return numKills;
	}
	
	static public void resetId()
	{
		currentId = 0;
	}
	
	public void setSpawn(Vector3 mySpawn)
	{
		spawn = mySpawn;
	}
	
	public void setInvincible(bool val)
	{
		invincible = val;
        if (!val)
        {
            Color matCol = this.gameObject.renderer.material.color;
            this.gameObject.renderer.material.color = new Color(matCol.r, matCol.g, matCol.b, 1.0f);
        }
    }
	
	public bool getInvincible()
	{
		return invincible;
	}
	
	public void alterNumKills(int amount)
	{
		numKills += amount;
	}
	// Use this for initialization
	void Start () 
	{
        isDead = false;
        util = (Utility)Instantiate(util);
        if (util is Reflect)
        {
            ((Reflect)util).shieldPrefab.GetComponent<Shield>().characterNumber = playerId;
            ((Reflect)util).shieldPrefab.GetComponent<Shield>().materialOfParent = renderer.material;
        }
		//util = new CloneCloak ();
		//((CloneCloak)(util)).clonePrefab = clonePrefab;
		invincible = false;
		playerId = currentId++;
		controller = (Controller)this.gameObject.GetComponent("Controller");
		util.controller = controller;
		util.owningChar = this.gameObject;
		util.rigidBody = (Rigidbody)this.gameObject.GetComponent("Rigidbody");
		util.init ();
		this.gameObject.name = "player" + playerId;
        isJuggernaut = false;
		controller.setCharacter(playerId);
		float x = (Random.value * 10) - 5;
		float y = (Random.value * 10) - 5;
		float z = (Random.value * 10) - 5;
		CharacterProjectile cp = this.gameObject.GetComponent<CharacterProjectile>();
        cp.p.owner = this.gameObject;
		cp.p.bullet.characterNumber = playerId;
        defaultWeapon.owner = this.gameObject;
        runeRepresentation = (RuneRep)Instantiate(runeRepresentation);
        GameObject.DontDestroyOnLoad(runeRepresentation);
        GameObject.DontDestroyOnLoad(util);
        transform.Find("HaloPrefab").renderer.material.color = new Color(94.0f / 255.0f, 12.0f / 255.0f, 148.0f / 255.0f);
        transform.Find("HaloPrefab2").renderer.material.color = new Color(94.0f / 255.0f, 12.0f / 255.0f, 148.0f / 255.0f);
        doorPopUp = (GameObject)Instantiate(doorPopUp);
        doorPopUp.transform.position = new Vector3(500000, 500000, 500000);
	}
	
	public int getId()
	{
		return playerId;
	}
	
	public void Restart()
	{
        isDead = false;
        GameManager.randomizeSpawn(this);
		numKills = 0;
		defaultWeapon.owner = this.gameObject;
        //((Light)(this.gameObject.GetComponentInChildren<Light>())).color = new Color(defaultWeapon.runeColor.r, defaultWeapon.runeColor.g, defaultWeapon.runeColor.b);
        //this.gameObject.GetComponent<Light>().color = 
		this.gameObject.GetComponent<CharacterProjectile>().p =(Projectile)Instantiate(defaultWeapon);
        runeRepresentation.rune = new Mesh();
        this.gameObject.transform.position = spawn;
        transform.Find("HaloPrefab").renderer.material.color = new Color(94.0f / 255.0f, 12.0f / 255.0f, 148.0f / 255.0f);
        transform.Find("HaloPrefab2").renderer.material.color = new Color(94.0f / 255.0f, 12.0f / 255.0f, 148.0f / 255.0f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GameManager.gameIsStarted)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(this.transform.position, new Vector3(0, 0, 1)), out hit, 1))
            {
                doorPopUp.transform.position = transform.position + new Vector3(0, 1, 0);
            } else if (doorPopUp)
            {
                doorPopUp.transform.position = new Vector3(500000, 500000, 500000);
            }

            Behaviour check = (Behaviour)this.gameObject.GetComponent("Halo");
            if (invinCountdown >= 0)
            {
                invinCountdown -= Time.deltaTime;
                invinCountdown = (invinCountdown <= 0) ? -9000 : invinCountdown;
            } else if (invinCountdown == -9000)
            {
                invinCountdown = -.1f;
                invincible = false;
                Color matCol = this.gameObject.renderer.material.color;
                this.gameObject.renderer.material.color = new Color(matCol.r, matCol.g, matCol.b);
            }

            if (invincible)
            {
                if (flickerTimer >= .15f && gameObject.renderer.material.color == InvincibilityColor)
                {
                    flickerTimer = 0;
                    gameObject.renderer.material.color = baseColor;
                }
                if (flickerTimer >= .15f && gameObject.renderer.material.color == baseColor)
                {
                    flickerTimer = 0;
                    gameObject.renderer.material.color = InvincibilityColor;
                }
            } else
            {
                gameObject.renderer.material.color = baseColor;
            }

            flickerTimer += Time.deltaTime;

            runeRepresentation.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
            runeRepresentation.gameObject.GetComponent<MeshRenderer>().material = this.gameObject.GetComponent<MeshRenderer>().material;

            if (runeRepresentation.rune)
            {
                float scale = 0.5f;
                if (GetComponent<CharacterProjectile>().p is ZappityZapZapMissileProjectile)
                {
                    scale = 0.25f;
                } else if (GetComponent<CharacterProjectile>().p is FireBall)
                {
                    scale = 0.7f;
                }
                if (GetComponent<CharacterProjectile>().p is Fireblast)
                {
                    scale = 1.0f;
                }

                Behaviour checkHalo = (Behaviour)this.gameObject.GetComponent("Halo");

                runeRepresentation.transform.localScale = new Vector3(scale, scale, scale);
                
            }

            //util.customUpdate ();
            //util.Update ();
        }

        diedLastFrame = false;
	}

	public void Kill()
	{
        isDead = true;
        GameObject getSound = GameObject.Find("SoundManager");
        SoundManager SM = getSound.GetComponent<SoundManager>();
        //SM.playDying();

        GameManager.randomizeSpawn(this);

        //this.gameObject.transform.position = spawn;
        SM.playLightning();

        LighteningSpawn ls = (LighteningSpawn)Instantiate(spawnEffect, spawn, Quaternion.identity);
        ls.player = this.GetComponent<Character>();
        ls.GetComponentInChildren<ParticleSystem>().startColor = this.renderer.material.color;
        ls.wait = 0.0f;

        this.transform.position = new Vector3(5000, 5000, -5000);
        if (!GameObject.FindObjectOfType<MenuSystem>())
        {
            this.GetComponent<ScreenWrap>().enabled = false;
        }

        transform.Find("HaloPrefab").renderer.material.color = new Color(94.0f / 255.0f, 12.0f / 255.0f, 148.0f / 255.0f);
        transform.Find("HaloPrefab2").renderer.material.color = new Color(94.0f / 255.0f, 12.0f / 255.0f, 148.0f / 255.0f);
	}

	public void Spawn()
	{
        isDead = false;
		this.transform.position = spawn;
		this.rigidbody.isKinematic = true;
		this.rigidbody.isKinematic = false;

		defaultWeapon.owner = this.gameObject;
		//this.gameObject.GetComponentInChildren<Light>().color = new Color(defaultWeapon.runeColor.r, defaultWeapon.runeColor.g, defaultWeapon.runeColor.b);
		//this.gameObject.GetComponentInChildren<Light>().intensity = 1;
		this.gameObject.GetComponent<CharacterProjectile>().p =(Projectile)Instantiate(defaultWeapon);
		
		invinCountdown = invinTime;
		invincible = true;
		runeRepresentation.rune = new Mesh();
		gameObject.renderer.material.color = baseColor;
		
        if (!GameObject.FindObjectOfType<MenuSystem>())
        {
            this.GetComponent<ScreenWrap>().enabled = true;
        }
	}

    public void CheckForDoor()
    {
        RaycastHit hit;
        if(Physics.Raycast(new Ray(this.transform.position, new Vector3(0,0,1)), out hit, 1))
        {
            hit.collider.gameObject.GetComponent<DoorScript>().GoToLevel();
            //print(hit.collider.gameObject.GetComponent<DoorScript>().sceneName);
        }
    }
}

//Drew was here.