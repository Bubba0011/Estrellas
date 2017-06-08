using System.Linq;

namespace Concepts.Core
{
	/// <summary>
	/// Resource definition.
	/// </summary>
	public class Resource
	{	
		public readonly string Name;

		public readonly bool IsTransitory;

		public readonly bool IsRefined;

		public readonly decimal DecayRate;
		
		public ResourceAmountVector Versor { get; internal set; }

		// ctor
		internal Resource(string name, bool transitory, bool refined, decimal decayRate)
		{			
			Name = name;
			IsTransitory = transitory;
			IsRefined = refined;
			DecayRate = decayRate;
		}
	}
}
