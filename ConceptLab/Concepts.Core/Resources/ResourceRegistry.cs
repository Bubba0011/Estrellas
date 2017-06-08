using System.Collections.Generic;
using System.Linq;

namespace Concepts.Core
{
	/// <summary>
	/// Contains resource definitions.
	/// </summary>
	public class ResourceRegistry
	{
		private readonly List<Resource> _resources = new List<Resource>();

		public Resource PopulationResource => GetResource("Population");

		public Resource FoodResource => GetResource("Food");

		public int Count => _resources.Count;

		public IEnumerable<Resource> Items => _resources.AsReadOnly();

		public IEnumerable<Resource> NaturalResources => Items.Where(r => !r.IsRefined).Where(r => !r.IsTransitory);

		// ctor
		public ResourceRegistry()
		{
			AddResource("Population", false, false);
			AddResource("Food", true, false);
		}

		internal Resource AddResource(string name, bool transitory, bool refined, decimal? decayRate = null)
		{
			decimal decay = decayRate ?? (transitory ? 1 : 0);		
			Resource resource = new Resource(name, transitory, refined, decay);

			Resource overrideTarget = GetResource(name);
			if (overrideTarget != null)
			{
				int index = _resources.IndexOf(overrideTarget);
				_resources[index] = resource;
			}
			else
			{
				_resources.Add(resource);
			}			

			return resource;
		}

		internal Resource GetResource(string name)
		{
			return _resources.SingleOrDefault(res => res.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase));
		}

		internal int IndexOfResource(Resource resource)
		{
			return _resources.IndexOf(resource);
		}

		internal ResourceAmountVector GetResourceVersor(Resource resource)
		{
			return ResourceAmountVector.CreateVersor(this, resource);
		}
	}
}
