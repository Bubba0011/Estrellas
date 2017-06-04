using Concepts.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	public class ConstructionQueue
	{
		private readonly List<ConstructionOrder> _orders = new List<ConstructionOrder>();

		public IEnumerable<ConstructionOrder> Items => _orders.AsReadOnly();

		public void Enqueue(ConstructionOrder order)
		{
			_orders.Add(order);
		}

		public void Enqueue(IConstructable what, int howMany)
		{
			for (int i = 0; i < howMany; i++)
			{
				Enqueue(new ConstructionOrder(what));
			}
		}

		internal void Construct(Planet planet)
		{
			while (_orders.Any())
			{
				var order = _orders.First();

				var used = order.Execute(planet.AvailableResources);
				planet.AvailableResources -= used;

				if (order.RemainingResources.IsZeroVector)
				{
					order.Construction.Deploy(planet, 1);
					_orders.RemoveAt(0);
				}
				else
				{
					return;
				}
			}
		}

		internal ConstructionReport Preview(ResourceAmountVector availableResources)
		{
			ConstructionReport report = new ConstructionReport(availableResources);

			foreach (var order in _orders)
			{				
				var consumed = order.Calculate(availableResources);
				availableResources -= consumed;

				report.AddItem(consumed);

				if (consumed.Equals(order.RemainingResources))
				{
					//order.Construction.Deploy(planet, 1);
				}
				else
				{
					break;
				}
			}

			return report;
		}
	}
}
