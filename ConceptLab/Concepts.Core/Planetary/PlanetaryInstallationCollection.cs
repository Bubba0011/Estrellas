using System.Collections.Generic;
using System.Linq;

namespace Concepts.Core
{
	public class PlanetaryInstallationCollection
	{
		private readonly List<PlanetaryInstallationStack> _stacks = new List<PlanetaryInstallationStack>();

		public IEnumerable<PlanetaryInstallationStack> Items => _stacks.AsReadOnly();

		public void Add(PlanetaryInstallation installation, int count = 1)
		{
			var stack = Items.SingleOrDefault(item => item.Installation == installation);

			if (stack == null)
			{
				stack = new PlanetaryInstallationStack(installation);
				_stacks.Add(stack);
			}

			stack.Count += count;
		}
	}
}
