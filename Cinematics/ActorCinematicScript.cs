using System;
using System.Collections;

[Serializable]
public abstract class ActorCinematicScript : CinematicScript
{
	public abstract IEnumerator Run(IActor subject, ICinematicContext context);
}
