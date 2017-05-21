using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	class ProductionReport
	{
		private List<ProductionReportItem> _items = new List<ProductionReportItem>();

		public IEnumerable<ProductionReportItem> Items => _items;

		public ProductionFlow TotalFlow => _items.Select(item => item.Flow).Aggregate((f1, f2) => f1 + f2);

		public void AddItem(ProductionReportItem item)
		{
			_items.Add(item);
		}
	}

	class ProductionReportItem
	{
		public string ProducerName { get; set; }

		public ProductionFlow Flow { get; set; }
	}
}
