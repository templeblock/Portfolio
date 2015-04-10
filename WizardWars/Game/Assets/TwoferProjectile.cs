﻿using UnityEngine;
using System.Collections;

public class TwoferProjectile: Projectile {

	
	// Use this for initialization
	public override void Start () {
        coolDownTimer = bullet.coolDown;
        
        if (owner != null)
        {
            Material m = owner.renderer.material;
            //bullet.gameObject.renderer.material = owner.renderer.material;
        }
	}

    public override int getTeamNumber()
    {
        return owner.GetComponent<Character>().teamNum;
    }
	
	public override void Fire(Vector2 dir)
	{
		if (coolDownTimer >= bullet.coolDown) {
            GameObject getSound=GameObject.Find("SoundManager");
            SoundManager SM=getSound.GetComponent<SoundManager>();
            SM.playMagicMissileLaunch();

            Magic clone = (Magic)Instantiate (bullet, owner.transform.position + (new Vector3 (dir.normalized.x, dir.normalized.y, 0.0f) * 0.8f), owner.transform.rotation);

            clone.renderer.material = owner.renderer.material;
            //clone.renderer.material.color = new Color(94.0f/255.0f,12.0f/255.0f,148.0f/255.0f);
            clone.onCollisionEffect.startColor = owner.renderer.material.color;
            clone.GetComponentInChildren<ParticleSystem>().startColor = owner.renderer.material.color;
			clone.rigidbody.velocity = dir.normalized;
			clone.characterNumber=((Controller)owner.GetComponent("Controller")).getCharacter();

            Magic clone2 = (Magic)Instantiate (bullet, owner.transform.position - (new Vector3 (dir.normalized.x, dir.normalized.y, 0.0f) * 0.8f), owner.transform.rotation);
            
            clone2.renderer.material = owner.renderer.material;
            //clone2.renderer.material.color = new Color(94.0f/255.0f,12.0f/255.0f,148.0f/255.0f);
            clone2.onCollisionEffect.startColor = owner.renderer.material.color;
            clone2.GetComponentInChildren<ParticleSystem>().startColor = owner.renderer.material.color;
            clone2.rigidbody.velocity = -dir.normalized;
            clone2.characterNumber=((Controller)owner.GetComponent("Controller")).getCharacter();
			coolDownTimer=0;
		}
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		coolDownTimer += Time.deltaTime;
		//Fire (new Vector2 (0.0f, 1.0f));
	}
}
