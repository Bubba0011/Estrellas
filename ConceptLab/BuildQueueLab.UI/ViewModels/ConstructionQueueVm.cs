using Concepts.Core;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildQueueLab.UI.ViewModels
{
	class ConstructionQueueVm : ViewModelBase
	{
		public List<BuildOrderWrapper> Items { get; private set; }

		public ConstructionQueueVm(ConstructionQueue constructionQueue)
		{
			Items = WrapOrders(constructionQueue.Items).ToList();
		}

		private IEnumerable<BuildOrderWrapper> WrapOrders(IEnumerable<ConstructionOrder> orders)
		{
			BuildOrderBatchWrapper prev = null;

			foreach (var order in orders)
			{
				if (order.ProgressPct > 0)
				{
					yield return new OngoingBuildOrderWrapper(order);
					prev = null;
				}
				else
				{
					if (prev == null || prev.Name != order.Construction.Name)
					{
						prev = new BuildOrderBatchWrapper(order);
						yield return prev;
					}
					
					prev.Count++;
				}
			}
		}
	}

	abstract class BuildOrderWrapper : ObservableObject
	{
		public string Name { get; protected set; }
	}

	class OngoingBuildOrderWrapper : BuildOrderWrapper
	{
		public double Progress { get; private set; }

		public OngoingBuildOrderWrapper(ConstructionOrder order)
		{
			Name = order.Construction.Name;
			Progress = Math.Round(100 * order.ProgressPct, 0);
		}
	}

	class BuildOrderBatchWrapper : BuildOrderWrapper
	{
		public int Count { get; internal set; }

		public BuildOrderBatchWrapper(ConstructionOrder order)
		{
			Name = order.Construction.Name;
		}
	}
}
