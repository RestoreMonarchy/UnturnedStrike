using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows Unity events to spawn effects.
	/// </summary>
	[AddComponentMenu("Unturned/Effect Spawner")]
	public class EffectSpawner : MonoBehaviour
	{
		/// <summary>
		/// GUID of effect asset to spawn when SpawnDefaultEffect is invoked.
		/// </summary>
		public string DefaultEffectAssetGuid;

		/// <summary>
		/// If true the server will spawn the effect and replicate it to clients,
		/// otherwise clients will predict their own local copy.
		/// </summary>
		public bool AuthorityOnly;

		/// <summary>
		/// Should the RPC be called in reliable mode? Unreliable effects might not be received.
		/// </summary>
		public bool Reliable;

		/// <summary>
		/// Transform to spawn the effect at.
		/// If unset this game object's transform will be used instead.
		/// </summary>
		public Transform OverrideTransform;

		public void SpawnDefaultEffect()
		{
#if GAME
			SpawnEffect(DefaultEffectAssetGuid);
#endif
		}

		public void SpawnEffect(string guid)
		{
#if GAME
			if (AuthorityOnly && !Provider.isServer)
				return;

			System.Guid parsedGuid;
			if (!System.Guid.TryParse(guid, out parsedGuid))
			{
				UnturnedLog.warn("{0} unable to parse effect asset guid \"{1}\"", transform.GetSceneHierarchyPath(), guid);
				return;
			}

			EffectAsset asset = Assets.find(parsedGuid) as EffectAsset;
			if (asset == null)
			{
				UnturnedLog.warn("{0} unable to find effect asset with guid \"{1}\"", transform.GetSceneHierarchyPath(), guid);
				return;
			}

			TriggerEffectParameters parameters = new TriggerEffectParameters(asset);
			parameters.shouldReplicate = AuthorityOnly;
			parameters.reliable = Reliable;

			Transform targetTransform = OverrideTransform == null ? transform : OverrideTransform;
			parameters.position = targetTransform.position;
			parameters.direction = targetTransform.forward;

			EffectManager.triggerEffect(parameters);
#endif // GAME
		}
	}
}
