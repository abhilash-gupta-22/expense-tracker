using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.API.Data;

public static class DbInitializer
{
    /// <summary>
    /// Ensures required tables exist in the database. If tables are missing, create schema from the current model.
    /// This method uses a provider-specific check for SQLite and falls back to EnsureCreated for other providers.
    /// </summary>
    public static void EnsureTablesCreated(ExpenseTrackerDbContext db, ILogger logger)
    {
        if (db is null) throw new ArgumentNullException(nameof(db));
        if (logger is null) throw new ArgumentNullException(nameof(logger));

        try
        {
            var provider = db.Database.ProviderName ?? string.Empty;

            // If SQLite provider, query sqlite_master to check for specific tables
            if (provider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                using var conn = db.Database.GetDbConnection();
                logger.LogInformation("Using DB Connection: {Connection}", conn.ConnectionString);
                conn.Open();

                using var cmd = conn.CreateCommand();

                // list existing tables
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";
                var tables = new List<string>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }

                logger.LogInformation("Existing tables: {Tables}", string.Join(',', tables));

                var required = new[] { "Budgets", "BudgetCategories", "Expenses" };
                var missing = required.Except(tables, StringComparer.OrdinalIgnoreCase).ToList();

                logger.LogInformation("Found {Count} of required tables in the database.", required.Length - missing.Count);

                // If not all required tables are present, try to create schema from model
                if (missing.Count > 0)
                {
                    logger.LogWarning("Required tables are missing: {Missing}. Attempting to create schema from the EF model using EnsureCreated().", string.Join(',', missing));
                    db.Database.EnsureCreated();

                    // re-query tables
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";
                    tables.Clear();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }

                    missing = required.Except(tables, StringComparer.OrdinalIgnoreCase).ToList();

                    if (missing.Count == 0)
                    {
                        logger.LogInformation("Database schema created from the EF model.");
                    }
                    else
                    {
                        logger.LogWarning("EnsureCreated did not create the following tables: {Missing}. Creating tables with explicit DDL.", string.Join(',', missing));

                        // create missing tables via explicit DDL (SQLite)
                        using var tx = conn.BeginTransaction();
                        using var createCmd = conn.CreateCommand();
                        createCmd.Transaction = tx;

                        // Create Budgets
                        if (missing.Contains("Budgets", StringComparer.OrdinalIgnoreCase))
                        {
                            createCmd.CommandText =
                                "CREATE TABLE IF NOT EXISTS \"Budgets\" (\n" +
                                "  \"Id\" TEXT NOT NULL PRIMARY KEY,\n" +
                                "  \"Month\" INTEGER NOT NULL,\n" +
                                "  \"Year\" INTEGER NOT NULL,\n" +
                                "  \"TotalBudget\" NUMERIC NOT NULL\n" +
                                ");";
                            createCmd.ExecuteNonQuery();
                            createCmd.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS \"IX_Budgets_Month_Year\" ON \"Budgets\" (\"Month\", \"Year\");";
                            createCmd.ExecuteNonQuery();
                        }

                        // Create BudgetCategories
                        if (missing.Contains("BudgetCategories", StringComparer.OrdinalIgnoreCase))
                        {
                            createCmd.CommandText =
                                "CREATE TABLE IF NOT EXISTS \"BudgetCategories\" (\n" +
                                "  \"Id\" TEXT NOT NULL PRIMARY KEY,\n" +
                                "  \"BudgetId\" TEXT NOT NULL,\n" +
                                "  \"Name\" TEXT NOT NULL,\n" +
                                "  \"AllocatedBudget\" NUMERIC NOT NULL,\n" +
                                "  FOREIGN KEY(\"BudgetId\") REFERENCES \"Budgets\"(\"Id\") ON DELETE CASCADE\n" +
                                ");";
                            createCmd.ExecuteNonQuery();
                            createCmd.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS \"IX_BudgetCategories_BudgetId_Name\" ON \"BudgetCategories\" (\"BudgetId\", \"Name\");";
                            createCmd.ExecuteNonQuery();
                        }

                        // Create Expenses
                        if (missing.Contains("Expenses", StringComparer.OrdinalIgnoreCase))
                        {
                            createCmd.CommandText =
                                "CREATE TABLE IF NOT EXISTS \"Expenses\" (\n" +
                                "  \"Id\" TEXT NOT NULL PRIMARY KEY,\n" +
                                "  \"BudgetCategoryId\" TEXT NOT NULL,\n" +
                                "  \"BudgetCategoryName\" TEXT,\n" +
                                "  \"Amount\" NUMERIC NOT NULL,\n" +
                                "  \"ExpenseDate\" TEXT NOT NULL,\n" +
                                "  \"Remarks\" TEXT NOT NULL,\n" +
                                "  FOREIGN KEY(\"BudgetCategoryId\") REFERENCES \"BudgetCategories\"(\"Id\") ON DELETE CASCADE\n" +
                                ");";
                            createCmd.ExecuteNonQuery();
                            createCmd.CommandText = "CREATE INDEX IF NOT EXISTS \"IX_Expenses_BudgetCategoryId_ExpenseDate\" ON \"Expenses\" (\"BudgetCategoryId\", \"ExpenseDate\");";
                            createCmd.ExecuteNonQuery();
                        }

                        tx.Commit();

                        logger.LogInformation("Explicit DDL executed to create missing tables.");
                    }
                }
                else
                {
                    logger.LogInformation("All required tables already exist.");
                }
            }
            else
            {
                // For other providers, try EnsureCreated which creates schema from model when possible
                logger.LogInformation("Non-SQLite provider detected ({Provider}). Ensuring database is created from model.", provider);
                db.Database.EnsureCreated();
                logger.LogInformation("Database schema ensured/created.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while ensuring database tables exist.");
            throw;
        }
    }
}
