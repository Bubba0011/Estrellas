using Concepts.Core;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildQueueLab.UI.ViewModels
{
	class PlanetVm : ViewModelBase
	{
		private Planet _planet;

		public string Name => _planet.Name;

		public int Population => _planet.Population;

		public int PopulationCapacity => _planet.PopulationCap;

		public decimal PopulationCapPct => Math.Round(100m * Population / PopulationCapacity, 1);

		public ResourceAmountVectorWrapper AvailableResources { get; private set; }

		public ConstructionQueueVm BuildQueue { get; private set; }

		public IEnumerable<InstallationStackWrapper> Installations { get; private set; }

		// ctor
		public PlanetVm(Planet planet, Rules rules)
		{
			_planet = planet;

			var next = _planet.GetProductionPreview(rules);
			AvailableResources = new ResourceAmountVectorWrapper(_planet.AvailableResources, next);

			BuildQueue = new ConstructionQueueVm(_planet.BuildQueue);

			Installations = _planet.Installations.Items.Select(stack => new InstallationStackWrapper(stack)).ToList();
		}
	}

	class ResourceAmountVectorWrapper : ObservableObject
	{
		public IEnumerable<ResourceAmountWrapper> Items { get; private set; }

		// ctor
		public ResourceAmountVectorWrapper(ResourceAmountVector current, ResourceAmountVector next)
		{	
			Items = current.Resources
				.Select(resource => new ResourceAmountWrapper(resource, current[resource], next[resource]))
				.ToList();
		}
	}

	class ResourceAmountWrapper : ObservableObject
	{
		public string Name { get; private set; }

		public int CurrentAmount { get; private set; }

		public int NextAmount { get; private set; }

		public int Change => NextAmount - CurrentAmount;

		// ctor
		public ResourceAmountWrapper(Resource resource, int current, int next)
		{
			Name = resource.Name;
			CurrentAmount = current;
			NextAmount = next;
		}
	}

	class InstallationStackWrapper : ObservableObject
	{
		public int Count { get; private set; }

		public string Name { get; private set; }

		// ctor
		public InstallationStackWrapper(PlanetaryInstallationStack stack)
		{
			Count = stack.Count;
			Name = stack.Installation.Name;
		}
	}
}
