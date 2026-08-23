using System;
using System.Collections.Generic;
using System.Text;

namespace sample1.Basics
{    // You have three order types with different validation rules:
    // MarketOrder  → just needs Ticker + Quantity > 0
    // LimitOrder   → needs Ticker + Quantity > 0 + LimitPrice > 0
    // StopOrder    → needs Ticker + Quantity > 0 + StopPrice > 0 + StopPrice < CurrentPrice

    // Currently all validation is in one ugly if/else block.
    // Refactor using Strategy pattern.
    public  class RuleEngine
    {
        private readonly IPriceService _priceService;

        public RuleEngine(IPriceService priceService)
        {
            _priceService = priceService;
        }

        private bool HasTicker(IOrder order) => !string.IsNullOrEmpty(order.Ticker);
        private bool HasQuantity(IOrder order) => order.Quantity > 0;
        private bool HasLimitPrice(LimitOrder order) => order.LimitPrice > 0;   
        private bool HasStopPrice(StopOrder order) => order.StopPrice > 0;
        public bool IsValidOrder(IOrder order) => order switch
        {
            MarketOrder marketOrder => HasTicker(marketOrder) && HasQuantity(marketOrder),
            LimitOrder limitOrder => HasTicker(limitOrder) && HasQuantity(limitOrder) && HasLimitPrice(limitOrder),
            StopOrder stopOrder => HasTicker(stopOrder) && HasQuantity(stopOrder) && HasStopPrice(stopOrder) && stopOrder.StopPrice < stopOrder.CurrentPrice,
            _ => throw new ArgumentException("Unknown order type")
        };
        public List<string> FindMismatchedPositions(
            Dictionary<string, decimal> ourPositions,
            Dictionary<string, decimal> custodianPositions)
        {
            var mismatches = new List<string>();

            foreach (var ticker in ourPositions.Keys)
            {
                var custodianQuantity = custodianPositions.ContainsKey(ticker) ? custodianPositions[ticker] : 0m;
                var ourQuantity = ourPositions.ContainsKey(ticker) ? ourPositions[ticker] : 0m;
                var isMismatch = Math.Round(ourQuantity, 3) != Math.Round(custodianQuantity, 3);
                if (isMismatch)
                {
                    mismatches.Add(ticker);
                }
            }

            return mismatches;
        }
        /// <summary>
        /// This fetches prices one at a time. Refactor it to fetch all in parallel, but ensure one failed price doesn't abort the whole batch.
        /// </summary>
        /// <param name="tickers"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, decimal>> GetPricesAsync(List<string> tickers)
        {
            var prices = new Dictionary<string, decimal>();
            Lock priceLock = new();
            await Parallel.ForAsync(0, tickers.Count, async (i, _) =>
            {
                var ticker = tickers[i];
                try
                {
                    var price = await _priceService.GetCurrentPriceAsync(ticker);
                    lock (priceLock)
                    {
                        prices[ticker] = price;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error for the specific ticker and continue with others
                    Console.WriteLine(ex.ToString());
                }
            });

            return prices;
        }
    }
    public interface IPriceService
    {
        Task<decimal> GetCurrentPriceAsync(string ticker);
    }
    public interface IOrder
    {
        public string Ticker { get; set; }
        public decimal Quantity { get; set; }
    }
    public class MarketOrder : IOrder
    {
        public required string Ticker { get; set; }
        public decimal Quantity { get; set; }       
    }
    public class LimitOrder : IOrder
    {
        public required string Ticker { get; set; }
        public decimal Quantity { get; set; }
        public decimal LimitPrice { get; set; }
    }
    public class StopOrder : IOrder
    {
        public required string Ticker { get; set; }
        public decimal Quantity { get; set; }
        public decimal StopPrice { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
