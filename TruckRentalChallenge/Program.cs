using System;

namespace TruckRentalChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = new DateTime(2019, 3, 15, 13, 21, 0);
            DateTime end = new DateTime(2019, 3, 15, 13, 40, 0);

            decimal amount = CalculateRentAmount(start, end);

            Console.WriteLine($"Amount owed {amount:C}");
            
            Console.ReadLine();
        }


        private static decimal CalculateRentAmount(DateTime startDate, DateTime endDate)
        {

            decimal output = 0;
            var rentalDuration = endDate.Subtract(startDate);

            if (rentalDuration.TotalMinutes <= Pricing.discountedStartMinutes)
            {
                output = Pricing.discountedStartPrice;
            }
            else
            {
                decimal totalCostByHourly = Pricing.discountedStartPrice +
                                            ((int)Math.Ceiling((rentalDuration.TotalMinutes - Pricing.discountedStartMinutes) / 60.0) *
                                             Pricing.hourlyRate);

                if (totalCostByHourly <= CalculateDailyRate(startDate))
                {
                    output = totalCostByHourly;
                }
                else
                {
                    var rentalDays = endDate.Date.Subtract(startDate.Date);
                    int totalRentDays = rentalDays.Days;

                    if (endDate.Hour >= 12)
                    {
                        totalRentDays += 1;
                    }

                    decimal calculatedDailyPrice = 0;

                    for (int i = 0; i < totalRentDays; i++)
                    {
                        calculatedDailyPrice += CalculateDailyRate(startDate.AddDays(i));
                    }

                    output = calculatedDailyPrice;
                }
            }

            return output;
        }

        private static decimal CalculateDailyRate(DateTime day)
        {
            decimal output = Pricing.dailyWeekdayRate;

            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
            {
                output = Pricing.dailyWeekendRate;
            }

            return output;
        }

    }

    public static class Pricing
    {
        // Simulates going out to the database, loading in these values, and then using
        // them for the lifespan of this application instance.
        public static int discountedStartMinutes = 20;
        public static decimal discountedStartPrice = 5M;
        public static decimal hourlyRate = 50M;
        public static decimal dailyWeekdayRate = 400M;
        public static decimal dailyWeekendRate = 200M;
    }
}
