using System;

using _CGRAPH = Constants.ScriptableObjects.Graph.Cinematics;

[Serializable]
public abstract class CinematicScript : INamedEntity
{
	public virtual string name => this.GetType().Name;
	public virtual int NodeWidth => _CGRAPH.Width.Custom;
	public virtual float LabelWidth => 0;

	public void Validate()
	{
		this.OnValidate();
	}

	protected virtual void OnValidate()
	{

	}
}
