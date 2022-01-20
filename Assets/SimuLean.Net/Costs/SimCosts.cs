namespace SimuLean
{
    public class SimCosts
    {
        public const double inventoryUnitCost = 0.10;

        public const double pendingOrderUnitCost = 0.20;

        public const double purchaseCost = 10.0;

        public const double processingCost = 8;

        public const double salePrice = 100.0;

        public const double shipmentCost = 40.0;

        public const double orderCost = 30.0;

        public const double workstationsCapacityCost = 150;
        public const double assemblerCapacityCost = 200;
        public const double storeCapacityCost = 5;

        public static double totalCost = 0.0;
        public static double totalRevenue = 0.0;

        public static void AddCost(double value)
        {
            totalCost += value;
        }

        public static void AddRevenue(double value)
        {
            totalRevenue += value;
        }

        public static double GetEarnings()
        {
            return totalRevenue - totalCost;
        }

        public static void RestartEarnings()
        {
            totalCost = 0;
            totalRevenue = 0;
        }
    }
}