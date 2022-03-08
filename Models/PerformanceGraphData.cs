using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace StrzonBinanceTradingBot.Models;

public class PerformanceGraphData : ResponseViewModel
{
    //[Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public string[] labels { get; set; } = new string[] { };
    //[Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public ChartDecimalDataSet[] datasets = new ChartDecimalDataSet[] { };

    public string ChartDataJSON
    {
        get
        {
            var retval = JsonConvert.SerializeObject(new
            {
                labels,
                datasets
            });

            return retval;
        }
    }
}