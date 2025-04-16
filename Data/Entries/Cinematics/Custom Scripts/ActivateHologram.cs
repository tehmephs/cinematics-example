using System.Collections;
using Assets.Scripts.Util;
using SerializeReferenceEditor;
using UnityEngine;
using _CMATH = Constants.Math;

[SRName("Activate Hologram")]
public class ActivateHologramCinematicScript : ActorCinematicScript
{
	private const string _STR_SHVAR_GLOWSTR = "_Glow_Strength";
	private const string _STR_SHVAR_VISIBILITY = "_Visibility";

	private const float _F_SCALE_START = 0.1f;
	private const float _F_DEFAULT_DELAY = 0.5f;
	private const float _F_DEFAULT_DURATION = 1f;

	[Header("Hologram Parameters"), Space(5)]

	[Tooltip("Determines if the hologram boundaries will be activated or deactivated")]
	public bool activate = true;

	[Min(0), Tooltip("Sets a delay before the transition begins.")]
	public float delay = _F_DEFAULT_DELAY;

	[Min(0), Tooltip("Defines how long it takes for the projector to do it's scale transition.")]
	public float duration = _F_DEFAULT_DURATION;

	public override IEnumerator Run(IActor subject, ICinematicContext context)
	{
		if (subject.HasMaterial)
		{
			var material = subject.GetMaterial();
			yield return new WaitForSeconds(this.delay);

			if (this.activate)
			{
				float tracker = Time.realtimeSinceStartup;


				var transform = subject.transform;

				Vector3 targetScale = transform.localScale;

				transform.localScale = new Vector3(_F_SCALE_START, _F_SCALE_START, _F_SCALE_START);
				material.SetFloat(_STR_SHVAR_GLOWSTR, _CMATH.FloatOne);
				material.SetFloat(_STR_SHVAR_VISIBILITY, _CMATH.FloatOne);

				float segmentDuration = this.duration / 2;
				float distance = targetScale.x - transform.localScale.x;
				float distancePerSecond = distance / segmentDuration;

				while (transform.localScale.x < targetScale.x)
				{
					transform.localScale = new Vector3(transform.localScale.x + (distancePerSecond * Time.deltaTime), _F_SCALE_START, _F_SCALE_START);
					yield return null;
				}

				transform.localScale = new Vector3(targetScale.x, _F_SCALE_START, _F_SCALE_START);

				distance = targetScale.y - transform.localScale.y;
				distancePerSecond = distance / segmentDuration;

				while (transform.localScale.y < targetScale.y)
				{
					var yz = transform.localScale.y + (distancePerSecond * Time.deltaTime);
					transform.localScale = new Vector3(transform.localScale.x, yz, yz);
					yield return null;
				}

				transform.localScale = targetScale;

				float time = 0.25f;
				float delta = 0.25f;

				while (delta > 0)
				{
					material.SetFloat(_STR_SHVAR_GLOWSTR, delta / time);
					delta -= Time.deltaTime;
					yield return null;
				}

				material.SetFloat(_STR_SHVAR_GLOWSTR, _CMATH.FloatZero);

				context.LogDebug(this, subject, $"ActivateHologram::Run - completed in {(Time.realtimeSinceStartup - tracker).ToStandardFormat()} seconds");
			}
			else
			{
				float elapsed = 0;

				while (elapsed < this.duration)
				{
					material.SetFloat(_STR_SHVAR_VISIBILITY, 1 - (elapsed / duration));
					yield return null;
					elapsed += Time.deltaTime;
				}

				material.SetFloat(_STR_SHVAR_VISIBILITY, 0);
			}
		}
		else
		{
			Debug.LogWarning($"ActivateHologram::Run - no material found on actor `{subject.name}`.  The script did not run.");
		}
	}

	//public override IEnumerator Run(Directive directive)
	//{
	//	if (directive.actor.HasMaterial)
	//	{
	//		yield return new WaitForSeconds(0.5f);

	//		float tracker = Time.realtimeSinceStartup;

	//		var material = directive.actor.GetMaterial();
	//		var transform = directive.actor.transform;

	//		Vector3 targetScale = transform.localScale;

	//		transform.localScale = new Vector3(_F_SCALE_START, _F_SCALE_START, _F_SCALE_START);
	//		material.SetFloat(_STR_SHVAR_GLOWSTR, _CMATH.FloatOne);
	//		material.SetFloat(_STR_SHVAR_VISIBILITY, _CMATH.FloatOne);

	//		float distance = targetScale.x - transform.localScale.x;

	//		while (transform.localScale.x < targetScale.x)
	//		{
	//			transform.localScale = new Vector3(transform.localScale.x + (distance * Time.deltaTime * 2), _F_SCALE_START, _F_SCALE_START);
	//			yield return null;
	//		}

	//		transform.localScale = new Vector3(targetScale.x, _F_SCALE_START, _F_SCALE_START);
	//		distance = targetScale.y - transform.localScale.y;

	//		while (transform.localScale.y < targetScale.y)
	//		{
	//			var yz = transform.localScale.y + (distance * Time.deltaTime * 2);
	//			transform.localScale = new Vector3(transform.localScale.x, yz, yz);
	//			yield return null;
	//		}

	//		transform.localScale = targetScale;

	//		float time = 0.25f;
	//		float delta = 0.25f;

	//		while (delta > 0)
	//		{
	//			material.SetFloat(_STR_SHVAR_GLOWSTR, delta / time);
	//			delta -= Time.deltaTime;
	//			yield return null;
	//		}

	//		material.SetFloat(_STR_SHVAR_GLOWSTR, _CMATH.FloatZero);

	//		Debug.Log($"ActivateHologram::Run - completed in {(Time.realtimeSinceStartup - tracker).ToStandardFormat()} seconds");
	//	}
	//	else
	//	{
	//		Debug.LogWarning($"ActivateHologram::Run - no material found on actor `{directive.actor.name}`.  The script did not run.");
	//	}
	//}
}
