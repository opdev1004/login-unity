using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class User
{
	public string id;
	public string password;

	public User(string id, string password)
	{
		this.id = id;
		this.password = password;
	}
}
