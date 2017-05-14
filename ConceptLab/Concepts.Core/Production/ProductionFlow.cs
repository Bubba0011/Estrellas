namespace Concepts.Core
{
	public class ProductionFlow
	{
		public readonly ResourceAmountVector Input;

		public readonly ResourceAmountVector Output;

		public ResourceAmountVector NetFlow
		{
			get { return Output - Input; }
		}

		// ctor
		public ProductionFlow(ResourceAmountVector input, ResourceAmountVector output)
		{
			Input = input;
			Output = output;
		}

		public ProductionFlow(ResourceAmountVector output)
		{
			Input = output * 0;
			Output = output;
		}

		public ProductionFlow(ResourceRegistry resources)
		{
			Input = new ResourceAmountVector(resources);
			Output = new ResourceAmountVector(resources);
		}

		public static ProductionFlow operator +(ProductionFlow lhs, ProductionFlow rhs)
		{
			return new ProductionFlow(lhs.Input + rhs.Input, lhs.Output + rhs.Output);
		}
	}
}
