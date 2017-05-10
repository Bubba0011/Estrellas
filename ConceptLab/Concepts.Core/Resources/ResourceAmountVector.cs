
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Concepts.Core
{ 
	public class ResourceAmountVector
	{
		private readonly ResourceRegistry _resources;
		private readonly int[] _amounts;

		public int this[Resource resource]
		{
			get { return _amounts[_resources.IndexOfResource(resource)]; }
			internal set { _amounts[_resources.IndexOfResource(resource)] = value; }
		}

		public bool IsZeroVector
		{
			get { return _amounts.All(amount => amount == 0); }
		}

		public int TotalAmount => _amounts.Sum();

		// ctor
		internal ResourceAmountVector(ResourceRegistry resourceRegistry)
		{
			_resources = resourceRegistry;
			_amounts = new int[resourceRegistry.Count];
		}

		private ResourceAmountVector(ResourceAmountVector source, Func<int,int> op)
		{
			_resources = source._resources;
			_amounts = source._amounts.Select(op).ToArray();
		}

		private ResourceAmountVector(ResourceAmountVector lhs, ResourceAmountVector rhs, Func<int, int, int> op)
		{
			_resources = lhs._resources;
			_amounts = lhs._amounts.Zip(rhs._amounts, op).ToArray();
		}

		internal ResourceAmountVector Filter(Func<Resource, bool> includeResource)
		{
			ResourceAmountVector result = new ResourceAmountVector(this, n => n);

			foreach (Resource res in _resources.Items.Where(res => !includeResource(res)))
			{
				result[res] = 0;
			}

			return result;
		}

		public override string ToString()
		{
			return ToString(null);
		}

		public string ToString(string format)
		{
			return format == "long" ? LongString() : VectorString();
		}

		private string VectorString()
		{
			return $"({string.Join(",", _amounts)})";
		}

		private string LongString()
		{
			var ras =_resources.Items
				.Where(res => this[res] != 0)
				.Select(res => this[res] + " " + res.Name);

			return $"({string.Join(", ", ras)})";
		}

		internal static ResourceAmountVector Initialize(ResourceRegistry resourceRegistry, IEnumerable<XElement> components)
		{
			var vector = new ResourceAmountVector(resourceRegistry);

			foreach (var xResource in components)
			{
				var resourceName = (string)xResource.Attribute("name");
				var resource = resourceRegistry.GetResource(resourceName);
				vector[resource] = (int)xResource.Attribute("amount");
			}

			return vector;
		}

		internal static ResourceAmountVector CreateVersor(ResourceRegistry resourceRegistry, Resource resource)
		{
			var vector = new ResourceAmountVector(resourceRegistry);
			vector[resource] = 1;
			return vector;
		}

		// Vector division
		public static int operator /(ResourceAmountVector lhs, ResourceAmountVector rhs)
		{
			if (rhs.IsZeroVector) return int.MaxValue;

			return new ResourceAmountVector(lhs, rhs, (l, r) => r > 0 ? l / r : int.MaxValue)._amounts.Min();
		}

		// Vector/scalar multiplication
		public static ResourceAmountVector operator *(ResourceAmountVector lhs, int rhs)
		{
			ResourceAmountVector result = new ResourceAmountVector(lhs, n => n * rhs);
			return result;
		}

		// Scalar/vector multiplication
		public static ResourceAmountVector operator *(int lhs, ResourceAmountVector rhs)
		{
			return rhs * lhs;
		}

		// Vector/scalar multiplication
		public static ResourceAmountVector operator *(ResourceAmountVector lhs, double rhs)
		{
			ResourceAmountVector result = new ResourceAmountVector(lhs, n => (int)(n * rhs));
			return result;
		}

		// Scalar/vector multiplication
		public static ResourceAmountVector operator *(double lhs, ResourceAmountVector rhs)
		{
			return rhs * lhs;
		}

		// Vector addition
		public static ResourceAmountVector operator +(ResourceAmountVector lhs, ResourceAmountVector rhs)
		{
			return new ResourceAmountVector(lhs, rhs, (l, r) => l + r);
		}

		// Vector subtraction
		public static ResourceAmountVector operator -(ResourceAmountVector lhs, ResourceAmountVector rhs)
		{
			return new ResourceAmountVector(lhs, rhs, (l, r) => l - r);
		}

		// Vector negation
		public static ResourceAmountVector operator -(ResourceAmountVector rhs)
		{
			return new ResourceAmountVector(rhs, n => -n);
		}

		/// <summary>
		/// Combines two vectors by selecting the smallest value for each component.
		/// </summary>
		public static ResourceAmountVector Minimize(ResourceAmountVector v1, ResourceAmountVector v2)
		{
			return new ResourceAmountVector(v1, v2, Math.Min);
		}		
	}
}
