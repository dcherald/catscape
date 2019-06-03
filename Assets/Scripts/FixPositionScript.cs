using UnityEngine;
using System.Collections;

public class FixPositionScript : MonoBehaviour
{
	public bool fixX, fixY, fixZ;
	public float xPos, yPos, zPos;

	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = transform.position;
		pos.x = fixX ? xPos:pos.x;
		pos.y = fixY ? yPos:pos.y;
		pos.z = fixZ ? zPos:pos.z;
		transform.position = pos;
	}
}

