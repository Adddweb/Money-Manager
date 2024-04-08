using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager
{
    internal class TransactionGenerator
    {
        private static readonly Random rnd = new Random();

        public static string GenerateRandomType()
        {
            string[] types = { "Spending", "Income" };
            return types[rnd.Next(types.Length)];
        }

        public static string GenerateRandomSpendingName()
        {
            string[] spendingNames = { "Food", "Entertainment", "Transportation", "Shopping", "Utilities", "Healthcare", "Education" };
            return spendingNames[rnd.Next(spendingNames.Length)];
        }

        public static string GenerateRandomIncomeName()
        {
            string[] incomeNames = { "Salary", "Bonus", "Investment", "Gift", "Rent", "Interest", "Dividends" };
            return incomeNames[rnd.Next(incomeNames.Length)];
        }

        public static string GenerateRandomSpendingDescription()
        {
            string[] spendingDescriptions = { "Grocery shopping", "Movie tickets", "Gasoline", "Clothing", "Electricity bill", "Medical expenses", "Tuition fee" };
            return spendingDescriptions[rnd.Next(spendingDescriptions.Length)];
        }

        public static string GenerateRandomIncomeDescription()
        {
            string[] incomeDescriptions = { "Monthly salary", "Year-end bonus", "Investment return", "Gift from family", "Rental income", "Interest from savings", "Dividends from stocks" };
            return incomeDescriptions[rnd.Next(incomeDescriptions.Length)];
        }

        public static double GenerateRandomAmount()
        {
            int wholePart = rnd.Next(1, 1001);

            double fractionalPart = rnd.Next(0, 100) / 100.0;

            return wholePart + fractionalPart;
        }

        public static DateTime GenerateRandomDate()
        {
            DateTime today = DateTime.Today;
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            return new DateTime(today.Year, today.Month, rnd.Next(1, daysInMonth + 1));
        }
    }
}
