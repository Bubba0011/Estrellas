using System.Linq;

namespace BuildQueueLab.Concepts
{
	/// <summary>
	/// Resource definition.
	/// </summary>
	class Resource
	{	
		public readonly string Name;

		public readonly bool IsTransitory;

		public readonly bool IsRefined;
		
		public ResourceAmountVector Versor { get; internal set; }

		// ctor
		internal Resource(string name, bool transitory, bool refined)
		{			
			Name = name;
			IsTransitory = transitory;
			IsRefined = refined;
		}
	}
}
