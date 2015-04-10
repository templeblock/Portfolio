﻿using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

    public bool kill;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//X wrap
		if(!this.gameObject.renderer.isVisible)
		{
			TrailRenderer tr = this.gameObject.GetComponentInChildren<TrailRenderer>();
			string tag = "";
			if(tr)
			{
				tag = "TrailBuddy";
			}
			Vector3 pos = this.gameObject.transform.position;
			if(Camera.main.WorldToScreenPoint(new Vector3(pos.x-this.gameObject.renderer.bounds.size.x/2, pos.y,pos.z)).x > Screen.width)
			{
				if(tr)
				{
					foreach (Transform t in this.gameObject.transform) 
					{
						if(t.tag == tag)
						{
							t.parent = null;
						}	
					}
					//this.gameObject.transform.DetachChildren();
				}
				Vector3 WfromS = Camera.main.WorldToScreenPoint(pos);
				this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(-this.gameObject.renderer.bounds.size.x/2+this.gameObject.renderer.bounds.size.x/8, WfromS.y, WfromS.z));	
				if(tr)
				{
					GameObject tb = GameObject.FindGameObjectWithTag(tag);
					GameObject child = (GameObject)Instantiate(tb, this.gameObject.transform.position, this.gameObject.transform.rotation);
					child.transform.parent = this.gameObject.transform;
                    tb.GetComponent<TrailRenderer>().enabled = true;
				}
			}
			else if(Camera.main.WorldToScreenPoint(new Vector3(pos.x+this.gameObject.renderer.bounds.size.x/2, pos.y, pos.z)).x< 0)
			{
				if(tr)
				{
					foreach (Transform t in this.gameObject.transform) 
					{
						if(t.tag == tag)
						{
							t.parent = null;
						}	
					}
					//this.gameObject.transform.DetachChildren();
				}
				Vector3 WfromS = Camera.main.WorldToScreenPoint(pos);
				this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + this.gameObject.renderer.bounds.size.x/2-this.gameObject.renderer.bounds.size.x/8, WfromS.y, WfromS.z));				
				if(tr)
				{
					GameObject tb = GameObject.FindGameObjectWithTag(tag);
					GameObject child = (GameObject)Instantiate(tb, this.gameObject.transform.position, this.gameObject.transform.rotation);
					child.transform.parent = this.gameObject.transform;
                    tb.GetComponent<TrailRenderer>().enabled = true;
				}
			}
			pos = this.gameObject.transform.position;
			//Y wrap
			if(Camera.main.WorldToScreenPoint(new Vector3(pos.x, pos.y-this.gameObject.renderer.bounds.size.y/2, pos.z )).y > Screen.height)
			{
				if(tr)
				{
					foreach (Transform t in this.gameObject.transform) 
					{
						if(t.tag == tag)
						{
							t.parent = null;
						}	
					}
					//this.gameObject.transform.DetachChildren();
				}
				Vector3 WfromS = Camera.main.WorldToScreenPoint(pos);
				this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(WfromS.x, -this.gameObject.renderer.bounds.size.y/2+this.gameObject.renderer.bounds.size.y/8, WfromS.z));	
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, pos.z);
				if(tr)
				{
					GameObject tb = GameObject.FindGameObjectWithTag(tag);
					GameObject child = (GameObject)Instantiate(tb, this.gameObject.transform.position, this.gameObject.transform.rotation);
					child.transform.parent = this.gameObject.transform;
                    tb.GetComponent<TrailRenderer>().enabled = true;
				}
			}
			else if(Camera.main.WorldToScreenPoint(new Vector3(pos.x, pos.y-this.gameObject.renderer.bounds.size.y/2, pos.z)).y < 0)
			{
				if(tr)
				{
					foreach (Transform t in this.gameObject.transform) 
					{
						if(t.tag == tag)
						{
							t.parent = null;
						}	
					}
					//this.gameObject.transform.DetachChildren();
				}
				Vector3 WfromS = Camera.main.WorldToScreenPoint(pos);
				this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(WfromS.x, Screen.height + this.gameObject.renderer.bounds.size.y/2-this.gameObject.renderer.bounds.size.y/8, WfromS.z));	
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, pos.z);
				if(tr)
				{
					GameObject tb = GameObject.FindGameObjectWithTag(tag);
					GameObject child = (GameObject)Instantiate(tb, this.gameObject.transform.position, this.gameObject.transform.rotation);
					child.transform.parent = this.gameObject.transform;
                    tb.GetComponent<TrailRenderer>().enabled = true;
				}
            }
            
		}
	}
}
