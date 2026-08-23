using System;

namespace sample1.Basics
{
    public class Trade
    {
        public string ClientId { get; set; }
        public string Ticker { get; set; }
        public decimal Quantity { get; set; }  // positive = buy, negative = sell
        public decimal Price { get; set; }
        public decimal Value => Quantity * Price;
    }

    public class Aggregator
    {
        /// <summary>
        /// Write a method that takes a flat list of trades and returns each client's net position per ticker.
        /// </summary>
        /// <param name="trades"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, decimal>> GetNetPositions(List<Trade> trades)
        {
            var result = trades
                .GroupBy(t => t.ClientId)
                .Select(g => new
                {
                    ClientId = g.Key,
                    PositionPerTicker = g.GroupBy(g => g.Ticker)
                    .ToDictionary(x => x.Key, x => x.Sum(t => t.Quantity))
                }).ToDictionary(x => x.ClientId, x => x.PositionPerTicker);
           return result;
        }

        // Current (flat fee - has a bug):
        public decimal CalculateFee(decimal aum)
        {
            return Fee(aum);
        }
        public decimal Fee(decimal aum) => aum switch 
        {
            < 0 => throw new ArgumentException("AUM cannot be negative"),
            <= 1_000_000 => aum * 0.01m,
            <= 5_000_000 => (1_000_000 * 0.01m) + ((aum - 1_000_000) * 0.0075m),
            _ => (1_000_000 * 0.01m) + (4_000_000 * 0.0075m) + ((aum - 5_000_000) * 0.005m)
        };
    }

}
