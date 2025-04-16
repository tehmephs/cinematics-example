using System;

using Assets.Scripts.Data;

namespace Assets.Scripts.Attributes
{
	/// <summary>
	/// For use in the <see cref="GameData"/> global model lists.  When the collection is tagged with this attribute and the `Rebuild Enumerations` button
	/// is clicked on the <see cref="GameData"/> singleton, it will automatically build an identity enum script for the given model type based on the items in the list.  
	/// This enum value then can be used to pull the scriptable object from the core game data via it's index.  Make sure the SO names do not change without first
	/// refactoring them in code if you intend to change the name of any of them.  The order in the list does not matter.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class GenerateEnumerationAttribute : Attribute
	{
		private const string _STR_DEFAULT_PATHFORMAT = "./Assets/_Game/Scripts/Data/Enum/Identity/{0}";

		/// <summary>
		/// The name to give the enum script
		/// </summary>
		public string ClassName { get; private set; }

		/// <summary>
		/// The path to generate the script file at in the project environment.
		/// </summary>
		public string PathFormat { get; private set; }

		public GenerateEnumerationAttribute(string className, string pathFormat = _STR_DEFAULT_PATHFORMAT)
		{
			this.ClassName = className;
			this.PathFormat = pathFormat;
		}
	}
}