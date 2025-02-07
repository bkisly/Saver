﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Saver.FinanceService.Infrastructure;

#nullable disable

namespace Saver.FinanceService.Infrastructure.Migrations
{
    [DbContext(typeof(FinanceDbContext))]
    [Migration("20241028182144_InitialMigration")]
    partial class InitialMigration
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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DefaultAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DefaultAccountId")
                        .IsUnique();

                    b.ToTable("accountHolders", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountHolderId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AccountHolderId")
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

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.RecurringTransactionDefinition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Cron")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid?>("ManualBankAccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ManualBankAccountId");

                    b.ToTable("recurringTransactionDefinitions", "finance");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.TransactionModel.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

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

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.AccountHolder", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.BankAccount", "DefaultAccount")
                        .WithMany()
                        .HasForeignKey("DefaultAccountId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("DefaultAccount");
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
                        .HasForeignKey("AccountHolderId");
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.RecurringTransactionDefinition", b =>
                {
                    b.HasOne("Saver.FinanceService.Domain.AccountHolderModel.ManualBankAccount", null)
                        .WithMany("RecurringTransactions")
                        .HasForeignKey("ManualBankAccountId");

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
                                .HasPrecision(2)
                                .HasColumnType("numeric(2)");

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
                                .HasPrecision(2)
                                .HasColumnType("numeric(2)");

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
                });

            modelBuilder.Entity("Saver.FinanceService.Domain.AccountHolderModel.ManualBankAccount", b =>
                {
                    b.Navigation("RecurringTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
