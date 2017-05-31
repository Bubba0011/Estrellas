using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	public class DistrictType
	{
		public string Name { get; set; }

		internal int Pct { get; set; }

		internal DistrictType(string name, int pct)
		{
			Name = name;
			Pct = pct;
		}
	}

	public class DistrictTypeRegistry
	{
		private Dictionary<string, DistrictType> _items = new Dictionary<string, DistrictType>(StringComparer.InvariantCultureIgnoreCase);

		public IEnumerable<DistrictType> Items => _items.Values;

		internal void Add(string name, int pct)
		{
			_items[name] = new DistrictType(name, pct);
		}
	}

	public class District
	{
		public readonly HashSet<DistrictType> Types = new HashSet<DistrictType>();

		public override string ToString()
		{
			return string.Join(", ", Types.Select(t => t.Name));
		}
	}
}
