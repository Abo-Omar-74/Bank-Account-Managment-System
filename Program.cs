using System;
using System.Reflection.Metadata.Ecma335;
using Console = System.Console;

class Program
{


    static void Main()
    {
        string name;
        double Balance;
        List<BankAccount> container = new List<BankAccount>();
        try
        {
            Console.WriteLine($"Enter details for Savings Account (HolderName, Balance):");
            name = Console.ReadLine();
            Balance = double.Parse(Console.ReadLine());
            if (Balance < 0)
                throw new ArgumentException("Error ! Input a valid Positive Number ! ");
            SavingsAccount SavingsAccountUser = new SavingsAccount(name, Balance);
            container.Add(SavingsAccountUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Occured ! Transaction Suspended .");
        }

        try
        {
            Console.WriteLine($"Enter details for Checking Account (HolderName, Balance):");
            name = Console.ReadLine();
            Balance = double.Parse(Console.ReadLine());
            if (Balance < 0)
                throw new ArgumentException("Error ! Input a valid Positive Number ! ");
            CheckingAccount CheckingAccountUser = new CheckingAccount(name, Balance);
            container.Add(CheckingAccountUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Occured ! Transaction Suspended .");
        }
        try
        {
            Console.WriteLine($"Enter details for Investment Account (HolderName, Balance):");
            name = Console.ReadLine();
            Balance = double.Parse(Console.ReadLine());
            InvestmentAccount investmentAccountUser = new InvestmentAccount(name, Balance);
            container.Add(investmentAccountUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Occured ! Transaction Suspended .");
        }

        try
        {
            Console.WriteLine($"Enter details for Credit Account (HolderName, Balance, Credit Limit):");
            name = Console.ReadLine();
            Balance = double.Parse(Console.ReadLine());
            double limit = double.Parse(Console.ReadLine());
            CreditAccount creditAccountUser = new CreditAccount(name, Balance, limit);
            container.Add(creditAccountUser);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Occured ! Transaction Suspended .");
        }

        if (container.Count > 0)
        {
            foreach (BankAccount item in container)
            {
                try
                {
                    Console.WriteLine($"Account Type: {item.GetType().Name}");
                    Console.WriteLine($"AccountHolder: {item.AccountHolderName}");
                    Console.WriteLine($"Account {item.AccountNumber} balance: E£{(item.Balance).ToString("F2")}");
                    Console.WriteLine($"Please enter amount deposited:");
                    double val = double.Parse(Console.ReadLine());
                    if (val < 0) throw new ArgumentException("Error ! Input a valid Positive Number ! ");
                    item.Deposit(val);
                    Console.WriteLine($"Please enter amount withdrawn:");
                    val = double.Parse(Console.ReadLine());
                    if (val < 0) throw new ArgumentException("Error ! Input a valid Positive Number ! ");
                    item.Withdraw(val);
                    if (item is IInterestEarning)
                    {
                        Console.WriteLine($"Calculating interest between two dates");
                        Console.WriteLine($"Please insert start date: (dd/MM/yyyy)");
                        string tempDate = Console.ReadLine();
                        DateTime startDate = DateTime.ParseExact(tempDate, "dd/MM/yyyy", null);
                        Console.WriteLine($"Please insert end date: (dd/MM/yyyy)");
                        tempDate = Console.ReadLine();
                        DateTime endDate = DateTime.ParseExact(tempDate, "dd/MM/yyyy", null);
                        IInterestEarning obj = (IInterestEarning)item;
                        obj.CalculateTotalInterest(startDate, endDate);
                        if (item is InvestmentAccount)
                        {
                            obj.ApplyInterest();
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured ! Transaction Suspended .");
                    continue;
                }
            }
        }



    }

    abstract class BankAccount
    {
        public string AccountNumber { get; init; }
        public string AccountHolderName { get; init; }
        public double Balance { get; set; }


        public void Deposit(double val)
        {
            Balance += val;
            Console.WriteLine($"Deposited E£{val.ToString("F2")} into account {AccountNumber}. New balance: E£{Balance.ToString("F2")}");
        }

        public virtual void Withdraw(double val)
        {

            if (Balance >= val)
            {
                Balance -= val;
                Console.WriteLine($"Withdrawn E£{val.ToString("F2")} from account {AccountNumber}. New balance: E£{Balance.ToString("F2")}");

            }
            else Console.WriteLine($"Insufficient funds in account {AccountNumber}.");



        }

        public void CheckBalance()
        {
            Console.WriteLine($"The Balance now is : E£{Balance.ToString("F2")}");
        }

        public BankAccount(string accountHolderName, double balance)
        {
            Balance = balance;
            AccountHolderName = accountHolderName;
            AccountNumber = $"{this.GetType().Name[0..3].ToUpper()}{new Random().Next(1000000, 10000000)}";
        }
    }
    class SavingsAccount : BankAccount, IInterestEarning
    {
        public SavingsAccount(string accountHolderName, double balance) : base(accountHolderName, balance) { }
        private double _interestPercentage = 0.03;
        public double CalculateInterest() => _interestPercentage * Balance;

        public void ApplyInterest()
        {
            Console.WriteLine($"Interest earned on investment account {AccountNumber}: E£{(_interestPercentage * Balance).ToString("F2")} New balance: E£{(Balance + (_interestPercentage * Balance)).ToString("F2")}");
            Balance += CalculateInterest();
        }


    }

    class CheckingAccount : BankAccount
    {
        public CheckingAccount(string accountHolderName, double balance) : base(accountHolderName, balance) { }
    }

    class InvestmentAccount : BankAccount, IInterestEarning
    {
        private double _interestPercentage = 0.05;

        public InvestmentAccount(string accountHolderName, double balance) : base(accountHolderName, balance) { }

        public double CalculateInterest() => _interestPercentage * Balance;

        public void ApplyInterest()
        {
            Console.WriteLine($"Interest earned on investment account {AccountNumber}: E£{(_interestPercentage * Balance).ToString("F2")} New balance: E£{(Balance + (_interestPercentage * Balance)).ToString("F2")}");
            Balance += CalculateInterest();
        }
    }

    class CreditAccount : BankAccount
    {
        private double _limit;

        public CreditAccount(string accountHolderName, double balance, double limit) : base(accountHolderName, balance)
        {
            _limit = limit;
        }
        public override void Withdraw(double val)
        {


            if (Balance >= val)
            {
                Balance -= val;
                Console.WriteLine($"Withdrawn E£{val.ToString("F2")} from account {AccountNumber} using credit. Balance: E£{Balance.ToString("F2")}, Credit: E£{(_limit).ToString("F2")}");
            }
            else if (_limit >= val)
            {
                _limit -= val;
                Console.WriteLine($"Withdrawn E£{val.ToString("F2")} from account {AccountNumber} using credit. Balance: E£{Balance.ToString("F2")}, Credit: E£{(_limit).ToString("F2")}");
            }
            else Console.WriteLine($"limit reached in account {AccountNumber}.!");

        }

    }

}

public interface IInterestEarning
{
    double CalculateInterest();
    void ApplyInterest();

}

public static class InterestExtensions
{
    public static void CalculateTotalInterest(this IInterestEarning interest, DateTime startDate, DateTime endDate)
    {
        double months = (int)endDate.Subtract(startDate).Days / (365.25 / 12);
        int averageMonths = (int)months;
        Console.Write($"Total interest earned in {averageMonths} months: ");
        Console.WriteLine($"E£{(interest.CalculateInterest() * averageMonths).ToString("F2")}");
    }
}
