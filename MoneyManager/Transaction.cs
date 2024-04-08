using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace MoneyManager
{
    internal class Transaction
    {
        private int _id;
        private string _type;
        private string _name;
        private string _description;
        private double _amount;
        private DateTime _date;
        
        public Transaction()
        {

        }
        public Transaction(int id, string type, string name, string description, double amount, DateTime date)
        {
            setId(id);
            setType(type);
            setName(name);
            setDescription(description);
            setAmount(amount);
            setDate(date);
        }
        public void setId(int id) 
        {
            if(id <= 0)
            {
                throw new ArgumentException("Id <= 0");
            }
            else
            {
                _id = id;
            }
        }
        public int getId() { return _id; }
        public void setType(string type) 
        {
            if(type == null || type == "")
            {
                throw new ArgumentException("Type is null");
            }
            else
            {
                _type = type;
            }    
        }
        public string getType() 
        {
            return _type;   
        }
        public void setName(string name) 
        {
            if(name == null || name == "")
            {
                throw new ArgumentException("Name is null");
            }
            else { _name = name; }
        }
        public string getName() 
        {
            return _name;
        }
        public void setDescription(string description) 
        {
            if (description == null || description == "")
            {
                //throw new ArgumentException("Description is null");
                _description = "None";
            }
            else
            {
                _description = description;
            }
        }

        public string getDescription()
        {
            return _description;
        }

        public void setAmount(double amount)
        {
            if(amount <= 0)
            {
                throw new ArgumentException("Amount <= 0");
            }
            else
            {
                _amount = amount;
            }
        }
        public double getAmount()
        {
            return _amount;
        }

        public void setDate(DateTime date)
        {
            if(date == null)
            {
                throw new ArgumentException("Date is null");
            }
            else
            {
                _date = date;  
            }   
        }
        public DateTime getDate()
        {
            return _date;
        }

        public string Show()
        {
            return "Type: " + _type + " Name: " + _name + " Description: " + _description + " Amount: " + _amount.ToString() + " Date: " + _date.ToString();
        }
    }
}
