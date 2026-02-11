namespace Bank_app.Model
{
    public class AccountClass
    {
        public int accountNumber { get; set; }
        public string accountHolderName { get; set; }
        public int currentBalance { get; set; }

        public AccountClass(int accountNumber, string accountHolderName, int currentBalance)
        {
            this.accountNumber = accountNumber;
            this.accountHolderName = accountHolderName;
            this.currentBalance = currentBalance;
        }
    }
}
