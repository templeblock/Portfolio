﻿using UnityEngine;
using System.Collections;

public class WallProjectile : Projectile {

	// Use this for initialization
	public override void Start () {
		coolDownTimer = bullet.coolDown;
		if(owner != null)
		{
			Material m = owner.renderer.material;	
			bullet.gameObject.renderer.material = owner.renderer.material;
			bullet.gameObject.renderer.material.color = new Color(94.0f/255.0f,12.0f/255.0f,148.0f/255.0f);
		}
	}
	
	public override int getTeamNumber()
	{
		return owner.GetComponent<Character>().teamNum;
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		coolDownTimer += Time.deltaTime;
		//Fire (new Vector2 (0.0f, 1.0f));
	}
	
	public override void Fire (Vector2 dir)
	{
		if (coolDownTimer >= bullet.coolDown) {
			GameObject getSound=GameObject.Find("SoundManager");
			SoundManager SM=getSound.GetComponent<SoundManager>();
			//Place sound play here
			Vector2 myDir = dir.normalized;
			float angle = Vector2.Dot(myDir, new Vector2(1, 0));
			print(angle);
			//angle = Mathf.Atan2(myDir.x, 1);
			angle *= Mathf.Rad2Deg;
			print(angle);
			Vector3 addPos = new Vector3();
			if(angle >= 45)
			{
				myDir = new Vector2(1.0f, 0);
				addPos.y = .5f;
			}
			else if(angle < 45 && angle >= -45 && dir.y > 0)
			{
				myDir = new Vector2(0, 1.0f);
			}
			else if(angle < -45)
			{
				myDir = new Vector2(-1.0f, 0);
				addPos.y = .5f;
			}
			else
			{
				myDir = new Vector2(0, -1.0f);
			}
			dir = myDir;
			
			Magic clone = (Magic)Instantiate (bullet, owner.transform.position + addPos + (new Vector3 (dir.normalized.x, dir.normalized.y, 0.0f) * 0.8f), owner.transform.rotation);
			clone.transform.rotation = Quaternion.AngleAxis(90, new Vector3(0, 0, dir.y));
			
			clone.renderer.material = owner.renderer.material;
            clone.renderer.material.color = owner.GetComponent<Character>().baseColor;
			clone.rigidbody.velocity = dir.normalized;
			clone.characterNumber=((Controller)owner.GetComponent("Controller")).getCharacter();
			coolDownTimer=0;
		}
	}
	
	
}
