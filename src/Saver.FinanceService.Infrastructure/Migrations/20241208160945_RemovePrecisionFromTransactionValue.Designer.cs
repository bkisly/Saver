﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Saver.FinanceService.Infrastructure;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Data.Migrations
{
    [DbContext(typeof(FinanceDbContext))]
    [Migration("20241208160945_RemovePrecisionFromTransactionValue")]
    partial class RemovePrecisionFromTransactionValue
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("finance")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Saver.EventBus.IntegrationEventLog.IntegrationEventLogEntry", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("TimesSent")
                        .HasColumnType("integer");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.HasKey("EventId");

                    b.ToTable("IntegrationEventLog", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.AccountHolder", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("accountHolders", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountHolderId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<int>("Currency")
                        .HasColumnType("integer")
                        .HasColumnName("CurrencyId");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("bankAccounts", "finance");

                    b.HasDiscriminator().HasValue("BankAccount");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountHolderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("categories", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.Currency", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("currencies", "finance");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "AED"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ARS"
                        },
                        new
                        {
                            Id = 3,
                            Name = "AUD"
                        },
                        new
                        {
                            Id = 4,
                            Name = "BGN"
                        },
                        new
                        {
                            Id = 5,
                            Name = "BRL"
                        },
                        new
                        {
                            Id = 6,
                            Name = "BSD"
                        },
                        new
                        {
                            Id = 7,
                            Name = "CAD"
                        },
                        new
                        {
                            Id = 8,
                            Name = "CHF"
                        },
                        new
                        {
                            Id = 9,
                            Name = "CLP"
                        },
                        new
                        {
                            Id = 10,
                            Name = "CNY"
                        },
                        new
                        {
                            Id = 11,
                            Name = "COP"
                        },
                        new
                        {
                            Id = 12,
                            Name = "CZK"
                        },
                        new
                        {
                            Id = 13,
                            Name = "DKK"
                        },
                        new
                        {
                            Id = 14,
                            Name = "DOP"
                        },
                        new
                        {
                            Id = 15,
                            Name = "EGP"
                        },
                        new
                        {
                            Id = 16,
                            Name = "EUR"
                        },
                        new
                        {
                            Id = 17,
                            Name = "FJD"
                        },
                        new
                        {
                            Id = 18,
                            Name = "GBP"
                        },
                        new
                        {
                            Id = 19,
                            Name = "GTQ"
                        },
                        new
                        {
                            Id = 20,
                            Name = "HKD"
                        },
                        new
                        {
                            Id = 21,
                            Name = "HRK"
                        },
                        new
                        {
                            Id = 22,
                            Name = "HUF"
                        },
                        new
                        {
                            Id = 23,
                            Name = "IDR"
                        },
                        new
                        {
                            Id = 24,
                            Name = "ILS"
                        },
                        new
                        {
                            Id = 25,
                            Name = "INR"
                        },
                        new
                        {
                            Id = 26,
                            Name = "ISK"
                        },
                        new
                        {
                            Id = 27,
                            Name = "JPY"
                        },
                        new
                        {
                            Id = 28,
                            Name = "KRW"
                        },
                        new
                        {
                            Id = 29,
                            Name = "KZT"
                        },
                        new
                        {
                            Id = 30,
                            Name = "MXN"
                        },
                        new
                        {
                            Id = 31,
                            Name = "MYR"
                        },
                        new
                        {
                            Id = 32,
                            Name = "NOK"
                        },
                        new
                        {
                            Id = 33,
                            Name = "NZD"
                        },
                        new
                        {
                            Id = 34,
                            Name = "PAB"
                        },
                        new
                        {
                            Id = 35,
                            Name = "PEN"
                        },
                        new
                        {
                            Id = 36,
                            Name = "PHP"
                        },
                        new
                        {
                            Id = 37,
                            Name = "PKR"
                        },
                        new
                        {
                            Id = 38,
                            Name = "PLN"
                        },
                        new
                        {
                            Id = 39,
                            Name = "PYG"
                        },
                        new
                        {
                            Id = 40,
                            Name = "RON"
                        },
                        new
                        {
                            Id = 41,
                            Name = "RUB"
                        },
                        new
                        {
                            Id = 42,
                            Name = "SAR"
                        },
                        new
                        {
                            Id = 43,
                            Name = "SEK"
                        },
                        new
                        {
                            Id = 44,
                            Name = "SGD"
                        },
                        new
                        {
                            Id = 45,
                            Name = "THB"
                        },
                        new
                        {
                            Id = 46,
                            Name = "TRY"
                        },
                        new
                        {
                            Id = 47,
                            Name = "TWD"
                        },
                        new
                        {
                            Id = 48,
                            Name = "UAH"
                        },
                        new
                        {
                            Id = 49,
                            Name = "USD"
                        },
                        new
                        {
                            Id = 50,
                            Name = "UYU"
                        },
                        new
                        {
                            Id = 51,
                            Name = "ZAR"
                        });
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.DefaultBankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountHolderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BankAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId")
                        .IsUnique();

                    b.HasIndex("BankAccountId");

                    b.ToTable("defaultBankAccounts", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.RecurringTransactionDefinition", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Cron")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("ManualBankAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ManualBankAccountId");

                    b.ToTable("recurringTransactionDefinitions", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.TransactionModel.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("transactions", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.ExternalBankAccount", b =>
                {
                    b.HasBaseType("Saver.FinanceService.Domain.AccountHolderModel.BankAccount");

                    b.HasDiscriminator().HasValue("ExternalBankAccount");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.ManualBankAccount", b =>
                {
                    b.HasBaseType("Saver.FinanceService.Domain.AccountHolderModel.BankAccount");

                    b.HasDiscriminator().HasValue("ManualBankAccount");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.BankAccount", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.AccountHolder", null)
                        .WithMany("Accounts")
                        .HasForeignKey("AccountHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.Category", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.AccountHolder", null)
                        .WithMany("Categories")
                        .HasForeignKey("AccountHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.DefaultBankAccount", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.AccountHolder", null)
                        .WithOne("DefaultAccount")
                        .HasForeignKey("Saver.FinanceService.Domain.AccountHolderModel.DefaultBankAccount", "AccountHolderId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.BankAccount", "BankAccount")
                        .WithMany()
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.RecurringTransactionDefinition", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.ManualBankAccount", null)
                        .WithMany("RecurringTransactions")
                        .HasForeignKey("ManualBankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Saver.FinanceService.Domain.TransactionModel.TransactionData", "TransactionData", b1 =>
                        {
                            b1.Property<Guid>("RecurringTransactionDefinitionId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("CategoryId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("character varying(256)");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric");

                            b1.HasKey("RecurringTransactionDefinitionId");

                            b1.HasIndex("CategoryId");

                            b1.ToTable("recurringTransactionDefinitions", "finance");

                            b1.HasOne("Saver.FinanceService.Domain.AccountHolderModel.Category", "Category")
                                .WithMany()
                                .HasForeignKey("CategoryId")
                                .OnDelete(DeleteBehavior.SetNull);

                            b1.WithOwner()
                                .HasForeignKey("RecurringTransactionDefinitionId");

                            b1.Navigation("Category");
                        });

                    b.Navigation("TransactionData")
                        .IsRequired();
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.TransactionModel.Transaction", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.BankAccount", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Saver.FinanceService.Domain.TransactionModel.TransactionData", "TransactionData", b1 =>
                        {
                            b1.Property<Guid>("TransactionId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("CategoryId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("character varying(256)");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric");

                            b1.HasKey("TransactionId");

                            b1.HasIndex("CategoryId");

                            b1.ToTable("transactions", "finance");

                            b1.HasOne("Saver.FinanceService.Domain.AccountHolderModel.Category", "Category")
                                .WithMany()
                                .HasForeignKey("CategoryId")
                                .OnDelete(DeleteBehavior.SetNull);

                            b1.WithOwner()
                                .HasForeignKey("TransactionId");

                            b1.Navigation("Category");
                        });

                    b.Navigation("TransactionData")
                        .IsRequired();
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.AccountHolder", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Categories");

                    b.Navigation("DefaultAccount");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.ManualBankAccount", b =>
                {
                    b.Navigation("RecurringTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
