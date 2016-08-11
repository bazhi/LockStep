using UnityEngine;
using Lockstep;

public class TestOBB : MonoBehaviour
{
	public FixedOBB2D m_OBB = new FixedOBB2D();
	private BoxCollider m_Collider;
	public bool update = false;

	// Use this for initialization
	void Start()
	{
		m_Collider = GetComponent<BoxCollider>();
		var bounds = m_Collider.bounds;
		Debug.Log(bounds.ToString());
		m_OBB.init(bounds.center.x, bounds.center.z, bounds.extents.x, bounds.extents.z);
	}
	
	// Update is called once per frame
	void Update()
	{
		//m_OBB.init(transform.position.x, transform.position.z);
		if (update) {
			m_OBB.setRotation(transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
		}
	}

	public void LogBounds()
	{
		//Debug.Log(m_Collider.getBB2D().ToString());
	}
}
