﻿using System.IO;
using System.Text.Json;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class TransactionService
    {
        private readonly string FilePath;
        private List<TransactionItem> transactions = new();
        public event Action OnChange;

        public IReadOnlyList<TransactionItem> Transactions => transactions;

        public TransactionService()
        {
            string directoryPath = @"C:\Users\koira\Source\Repos\C-.NET-software-application";
            FilePath = Path.Combine(directoryPath, "transactions.json");

            LoadTransactions();
        }

        public void AddTransaction(TransactionItem transaction)
        {
            var newTransaction = new TransactionItem
            {
                Date = transaction.Date,
                Title = transaction.Title,
                Type = transaction.Type,
                Amount = transaction.Amount,
                Notes = transaction.Notes
            };

            transactions.Add(newTransaction);
            SaveTransactions();
            OnChange?.Invoke();
        }

        public List<TransactionItem> GetFilteredTransactions(DateTime startDate, DateTime endDate)
        {
            return transactions
                .Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        private void LoadTransactions()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    transactions = JsonSerializer.Deserialize<List<TransactionItem>>(json) ?? new List<TransactionItem>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
                transactions = new List<TransactionItem>();
            }
        }

        private void SaveTransactions()
        {
            try
            {
                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }

        public void ClearAllTransactions()
        {
            transactions.Clear();
            SaveTransactions(); 
            OnChange?.Invoke(); 
        }
    }
}
