﻿using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour
{

    public float timeOut;
    //public Vector3 velocity;
    public float Speed;
    public float coolDown;
    public int numberOfBounces;
    public int characterNumber;
    public GameObject owningChar;
    public ParticleSystem onCollisionEffect;
    float currentTime;
    Vector3 lastVelocity;
    float bounciness;
	float screenWidth;
	float screenHeight;

    // Use this for initialization
    void Start()
    {
		float z = Camera.main.WorldToViewportPoint (new Vector3(0, 0, 0)).z;
		screenWidth =Mathf.Abs(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z)).x - (Camera.main.ViewportToWorldPoint(new Vector3(1, 0, z)).x));
		screenHeight =Mathf.Abs(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z)).y - (Camera.main.ViewportToWorldPoint(new Vector3(0, 1, z)).y));

        if ((owningChar.transform.position - transform.position).magnitude < 1.5f)
        {
            RaycastHit rh;
            Vector3 diff = (owningChar.transform.position - transform.position).normalized;
            if (Physics.Raycast(new Ray(transform.position + diff * 0.1f, diff), out rh, 1.2f))
            {
                if (rh.collider.gameObject != owningChar)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }
           
        }
        rigidbody.velocity *= Speed;
        currentTime = 0;
        bounciness = 1;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.gameObject.name == "Magic Missile(Clone)" 
            || this.gameObject.name == "Fireball(Clone)" 
            || this.gameObject.name == "Ricochet Magic(Clone)")
        {
            ParticleSystem g = (ParticleSystem)Instantiate(onCollisionEffect, transform.position, Quaternion.identity);
			if(this.gameObject.name=="Fireball(Clone)")
			{
				ParticleSystem f=(ParticleSystem)Instantiate (onCollisionEffect, new Vector3(transform.position.x+screenWidth, transform.position.y, transform.position.z), Quaternion.identity);
				ParticleSystem d=(ParticleSystem)Instantiate (onCollisionEffect, new Vector3(transform.position.x-screenWidth, transform.position.y, transform.position.z), Quaternion.identity);
				ParticleSystem s=(ParticleSystem)Instantiate (onCollisionEffect, new Vector3(transform.position.x, transform.position.y+screenHeight, transform.position.z), Quaternion.identity);
				ParticleSystem a=(ParticleSystem)Instantiate (onCollisionEffect, new Vector3(transform.position.x, transform.position.y-screenHeight, transform.position.z), Quaternion.identity);
			
				Destroy(f.gameObject, onCollisionEffect.duration);
				Destroy(d.gameObject, onCollisionEffect.duration);
				Destroy(s.gameObject, onCollisionEffect.duration);
				Destroy(a.gameObject, onCollisionEffect.duration);
			}
			Destroy(g.gameObject, onCollisionEffect.duration);

            //Vector3 xOffset = transform.position;
            //xOffset.x = (((Screen.width * 2) / 3) % Screen.width);
            //ParticleSystem gx = (ParticleSystem)Instantiate(onCollisionEffect, xOffset, Quaternion.identity);
            //Destroy(gx.gameObject, onCollisionEffect.duration);
            //
            //Vector3 yOffset = new Vector3();
            //yOffset.y = (Screen.height % ((Screen.height * 2) / 3));
            //ParticleSystem gy = (ParticleSystem)Instantiate(onCollisionEffect, transform.position + yOffset, Quaternion.identity);
            //Destroy(gy.gameObject, onCollisionEffect.duration);
        }
        if (this.gameObject.name=="Fireball(Clone)")
        {

            Vector2 start = new Vector2(1, 0);
            int count = 16;

            GameObject getSound=GameObject.Find("SoundManager");
            SoundManager SM=getSound.GetComponent<SoundManager>();
            SM.playFireBallExplosion();

            for(int i=0; i<count; i++)
            {
                CharacterProjectile cp = owningChar.transform.root.GetComponent<CharacterProjectile>();
                Controller con = owningChar.transform.root.GetComponent<Controller>();
                Vector3 myDir = new Vector3(start.x, start.y, 0)*.4f;
                myDir = Quaternion.Euler(0, 0, i*(360/count))*myDir;
                Magic clone = (Magic)Instantiate (owningChar.GetComponent<Character>().fireballSpawn.bullet, gameObject.transform.position + myDir, gameObject.transform.rotation);
                clone.gameObject.renderer.material = gameObject.renderer.material;
                clone.owningChar=owningChar;
                
                clone.rigidbody.velocity = new Vector2(myDir.x, myDir.y).normalized;
                //clone.rigidbody.useGravity = true;
                clone.characterNumber=con.getCharacter();
            }
        }

        Character guy = collision.collider.gameObject.GetComponent<Character>();
        if (guy != null)
        {
            if (!guy.getInvincible() && !guy.diedLastFrame)
            {
                guy.diedLastFrame = true;
                if (characterNumber == guy.getId())
                {
                    //guy.alterNumKills(-1);
                }
                else if(owningChar.transform.root.GetComponent<CharacterProjectile>().getTeamNum()!=guy.teamNum)
                {
                    Destroy(this.gameObject);
                    if(GameManager.gameType==1 || GameManager.gameType==2)
                    {
                        GameManager.getPlayer(characterNumber).alterNumKills(1);
                    }
                    else if(GameManager.gameType==3)
                    {
                        if(guy.isJuggernaut)
                        {
                            GameManager.getPlayer(characterNumber).isJuggernaut=true;
                            guy.isJuggernaut=false;
                        }
                    }
                    GameManager.randomizeSpawn(guy);
                    guy.Kill();
                }
            }
            if( guy.getInvincible())
            {
                Destroy(this.gameObject);
            }
        }

        if(collision.collider.gameObject.name != "Magic")
        {
            
            if (numberOfBounces > 0 && currentTime>0.01)
            {
                GameObject getSound=GameObject.Find("SoundManager");
                SoundManager SM=getSound.GetComponent<SoundManager>();
                SM.playRicochetteBounce();

                numberOfBounces--;
                //BOUNCE.... 
                //Manually... Something to do with dot.
            }
            else if (numberOfBounces > 0 && currentTime<0.01)
            {
                GameObject getSound=GameObject.Find("SoundManager");
                SoundManager SM=getSound.GetComponent<SoundManager>();
                SM.playRicochetteBounce();

                transform.position=new Vector3(transform.position.x, transform.position.y+0.4f, transform.position.z);
            }
            else if((numberOfBounces==0 && this.gameObject.name != "WallBullet(Clone)") || (this.gameObject.name == "WallBullet(Clone)" && !collision.collider.gameObject.GetComponent<Magic>()))
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
		if (this.gameObject.name == "WallBullet(Clone)" && col.gameObject.GetComponent<Magic>())
		{
			if(col.gameObject.name == "WallBullet(Clone)")
			{
				Destroy(this.gameObject);
			}
			Destroy(col.gameObject);
			return;	
		}
		Character guy = col.gameObject.GetComponent<Character>();
		if (guy != null && this.gameObject.name == "WallBullet(Clone)")
		{
			if (guy != null && !guy.getInvincible())
			{
				if (characterNumber == guy.getId())
				{
					//guy.alterNumKills(-1);
				}
				else if(owningChar.transform.root.GetComponent<CharacterProjectile>().getTeamNum()!=guy.teamNum)
				{
					Destroy(this.gameObject);
					if(GameManager.getPlayer(0).gameType==1 || GameManager.getPlayer(0).gameType==2)
					{
						GameManager.getPlayer(characterNumber).alterNumKills(1);
					}
					else if(GameManager.getPlayer(0).gameType==3)
					{
						if(guy.isJuggernaut)
						{
							GameManager.getPlayer(characterNumber).isJuggernaut=true;
							guy.isJuggernaut=false;
						}
					}
					GameManager.randomizeSpawn(guy);
					guy.Kill();
				}
			}
			if( guy.getInvincible())
			{
				Destroy(this.gameObject);
			}
		}
    }
    
    // Update is called once per frame
    void Update()
    {
        //this.rigidbody.transform.Translate((velocity * Speed) * Time.deltaTime);

        currentTime += Time.deltaTime;

        if (timeOut <= currentTime)
        {
            Destroy(this.gameObject);
        }
        lastVelocity = rigidbody.velocity;

    }
}
