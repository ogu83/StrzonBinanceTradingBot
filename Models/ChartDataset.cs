namespace StrzonBinanceTradingBot.Models;

public class ChartDecimalDataSet : ChartDataset<decimal>
{
    
}

public class ChartDataset<T> where T : struct
{
    public string label { get; set; } = "Label";
    public T[] data { get; set; } = new T[] { };
    public bool fill { get; set; } = false;
    public string borderColor { get; set; } = "rgb(75, 192, 192)";
    public decimal tension { get; set; }
}
