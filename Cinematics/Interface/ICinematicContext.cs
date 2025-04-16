using Assets.Scripts.Data;
using Assets.Scripts.Graph.Cinematics;

public interface ICinematicContext
{
	IActor GetActor(ActorId id);
	IActor GetActor(ActorIdentity id);

	IPoint GetPoint(PointId id);
	IPoint GetPoint(PointIdentity id);

	ICinematicContext Clone(CinematicsGraph graph);
	void Flush();

	void LogDebug(string message);
	void LogDebug(INamedEntity node, string message);
	void LogDebug(INamedEntity node, IActor actor, string message);

	bool ActorsAreBusy { get; }
}