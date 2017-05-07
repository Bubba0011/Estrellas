using BuildQueueLab.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildQueueLab.Concepts
{
	class ConstructionQueue
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
	}

	class ConstructionOrder
	{
		public readonly IConstructable Construction;

		public ResourceAmountVector UsedResources { get; private set; }

		public ResourceAmountVector RemainingResources => Construction.ConstructionCost - UsedResources;

		public double ProgressPct => (double)UsedResources.TotalAmount / Construction.ConstructionCost.TotalAmount;

		// ctor
		public ConstructionOrder(IConstructable what)
		{
			Construction = what;
			UsedResources = what.ConstructionCost * 0;
		}

		public ResourceAmountVector Execute(ResourceAmountVector available)
		{
			var resourcesTaken = ResourceAmountVector.Minimize(available, RemainingResources);

			if (resourcesTaken / RemainingResources != 1)
			{
				Func<Resource, bool> isMaterials = resource => !resource.IsTransitory;
				Func<Resource, bool> isLabor = resource => resource.IsTransitory;

				var materialCost = Construction.ConstructionCost.Filter(isMaterials);
				var laborCost = Construction.ConstructionCost.Filter(isLabor);

				if (!materialCost.IsZeroVector)
				{
					var materialTaken = resourcesTaken.Filter(isMaterials);
					
					// Use all available materials
					var newUsedResources = UsedResources + materialTaken;
					var usedMaterials = newUsedResources.Filter(isMaterials);

					// Limit used labor to same percentage as used materials
					var pctCompletedMaterials = (double)usedMaterials.TotalAmount / materialCost.TotalAmount;
					var laborUsedLimit = laborCost * pctCompletedMaterials;
					var canTake = laborUsedLimit - UsedResources.Filter(isLabor);
					var laborTaken = ResourceAmountVector.Minimize(available, canTake);

					resourcesTaken = materialTaken + laborTaken;
				}
			}

			UsedResources += resourcesTaken;
			return resourcesTaken;
		}
	}
}

