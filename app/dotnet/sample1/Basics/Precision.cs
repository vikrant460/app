using System;
using System.Collections.Generic;
using System.Text;

namespace sample1.Basics
{
    public class PortfolioCalculator
    {
        
        public decimal CalculateTotalValue(List<Position> positions)
        {
            return positions.Sum(x => x.TotalValue);

        }
    }

    public class Position
    {
        private const int Precision = 2; 
        public string Ticker { get; private set; }
        public double Quantity { get; private set; }
        public double Price { get; private set; }
        public decimal TotalValue  => Math.Round((decimal)Quantity * (decimal)Price, Precision);
    }
}
