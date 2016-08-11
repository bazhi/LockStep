using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public TestOBB m_Test1;
	public TestOBB m_Test2;
	// Use this for initialization
	void Start()
	{
		InvokeRepeating("TestOBB", 2.0f, 2.0f);
	}

	void TestOBB()
	{
		if (m_Test1.m_OBB.isCollision(m_Test2.m_OBB)) {
			Debug.Log("OBB Collision");
		}
		m_Test1.LogBounds();
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}
}
