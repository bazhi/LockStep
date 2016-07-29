using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Lockstep;

namespace Lockstep.Mono
{
	public class Controller : MonoBehaviour, IPointerDownHandler
	{
		public void OnPointerDown(PointerEventData eventData)
		{
			var ray = Camera.main.ScreenPointToRay(eventData.position);
			RaycastHit hit; 
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Messenger.Broadcast(EventCmd.CMD_ChangeDestination, hit.point);
			}
		}
	}
}


