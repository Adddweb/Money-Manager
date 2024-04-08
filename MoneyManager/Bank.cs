using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager
{
    internal class Bank
    {
        private double _money;
        Sql sql = new Sql("Server=192.168.30.167;Database=master;User ID=Adddriga;Password=123456;Encrypt=false;MultipleActiveResultSets=true", "Server=192.168.30.167;Database=transacsdb;User ID=Adddriga;Password=123456;Encrypt=false;MultipleActiveResultSets=true");
        public Bank() { }
        public Bank(double money) 
        {
            setMoney(money);
        }

        public void setMoney(double money) 
        {
            if(money < 0) 
            {
                throw new ArgumentOutOfRangeException("Money < 0");
            }
            else
            {
                _money = Math.Round(money, 2);
            }
        }
        public double getMoney() { return _money;}

        public async Task<Transaction> HandleNewTransaction(string type, string name, string description, double amount, DateTime date) 
        {
            double oldmoney = _money;
            if(type == "Spending")
            {
                if(amount <= _money)
                {
                    _money -= amount;
                    _money = Math.Round(_money, 2);
                }
                else
                {
                    throw new ArgumentException("Spending more then have");
                }
            }
            else if(type == "Income")
            {
                _money += amount;
                _money = Math.Round(_money, 2);
            }
            int id = await sql.Insert(type, name, description, amount, date, oldmoney, _money, 1);
            Console.WriteLine(id);
            Transaction trans = new Transaction(id, type, name, description, amount, date);
            return trans;
        }
        public async void CreateManyTransactions(int count)
        {
            for(int i = 0; i < count; i++)
            {
                string type = TransactionGenerator.GenerateRandomType();
                string name = "";
                string description = "";
                double amount = TransactionGenerator.GenerateRandomAmount();
                DateTime date = TransactionGenerator.GenerateRandomDate();
                double oldmoney = _money;
                if(amount > _money) 
                {
                    type = "Income";
                }
                if(type == "Income")
                {
                    _money += amount;
                    _money = Math.Round(_money, 2);
                    name = TransactionGenerator.GenerateRandomIncomeName();
                    description = TransactionGenerator.GenerateRandomIncomeDescription();
                }
                else 
                {
                    _money -= amount;
                    _money = Math.Round(_money, 2);
                    name = TransactionGenerator.GenerateRandomSpendingName();
                    description = TransactionGenerator.GenerateRandomSpendingDescription();
                }
                int id = await sql.Insert(type, name, description, amount, date, oldmoney, _money, 1);
            }
        }
    }
}
