using System;
using System.Collections;

[Serializable]
public abstract class SceneCinematicScript : CinematicScript
{
	public abstract IEnumerator Run(ICinematicContext context);
}
