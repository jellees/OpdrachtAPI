using System.Diagnostics.Metrics;

namespace OpdrachtAPI;

public class OpdrachtAPIMetrics : IOpdrachtAPIMetrics
{
    private readonly Counter<int> _productsAddedCounter;

    public OpdrachtAPIMetrics(IMeterFactory meterFactory)
    {

        var meter = meterFactory.Create("OpdrachtAPIMetrics.Web");
        _productsAddedCounter = meter.CreateCounter<int>("opdrachtapi.product.added");
    }

    public void ProductAdded(int quantity)
    {
        _productsAddedCounter.Add(quantity);
    }
}
