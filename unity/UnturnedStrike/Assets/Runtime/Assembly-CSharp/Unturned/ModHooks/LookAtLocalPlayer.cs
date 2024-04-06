using UnityEngine;

namespace SDG.Unturned
{
	public class LookAtLocalPlayer : MonoBehaviour
	{
#if GAME
		private void LateUpdate()
		{
			if (Player.player != null)
			{
				transform.LookAt(Player.player.look.aim);
			}
		}
#endif // GAME
	}
}
