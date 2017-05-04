using System.Collections.Generic;
using System.Linq;

namespace BuildQueueLab.Concepts
{
	/// <summary>
	/// Contains resource definitions.
	/// </summary>
	class ResourceRegistry
	{
		private readonly List<Resource> _resources = new List<Resource>();

		public readonly Resource PopulationResource;

		public int Count => _resources.Count;

		public IEnumerable<Resource> Items => _resources.AsReadOnly();

		public IEnumerable<Resource> NaturalResources => Items.Where(r => !r.IsRefined).Where(r => !r.IsTransitory);

		// ctor
		public ResourceRegistry()
		{
			PopulationResource = AddResource("Population", false, false);
		}

		internal Resource AddResource(string name, bool transitory, bool refined)
		{
			Resource resource = new Resource(name, transitory, refined);
			_resources.Add(resource);
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
