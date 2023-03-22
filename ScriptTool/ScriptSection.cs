using System;

namespace ScriptTool
{
	internal abstract class ScriptSection
	{
		public abstract override string ToString();
		public abstract bool isOptional { get; }
	}
}
