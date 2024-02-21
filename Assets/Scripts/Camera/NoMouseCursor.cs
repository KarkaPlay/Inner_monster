using UnityEngine;
using System.Collections;

public class NoMouseCursor : MonoBehaviour {
	
	bool isLocked;

	void Start () {
		SetCursorLock (true);
	}

	void SetCursorLock(bool isLocked) {
		this.isLocked = isLocked;
		Screen.lockCursor = isLocked;
		Cursor.visible = !isLocked;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) { Cursor.visible = true;Cursor.lockState = CursorLockMode.None; }
			//SetCursorLock (!isLocked);
		if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }

	}

	


}﻿