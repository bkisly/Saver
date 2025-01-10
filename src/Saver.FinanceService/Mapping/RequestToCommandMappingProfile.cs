using AutoMapper;
using Saver.FinanceService.Commands;
using Saver.FinanceService.Contracts.BankAccounts;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Contracts.Transactions;

namespace Saver.FinanceService.Mapping;

public class RequestToCommandMappingProfile : Profile
{
    public RequestToCommandMappingProfile()
    {
        CreateMap<CreateManualBankAccountRequest, CreateManualBankAccountCommand>();
        CreateMap<EditManualBankAccountRequest, EditManualBankAccountCommand>();
        CreateMap<CreateExternalBankAccountRequest, CreateExternalBankAccountCommand>();

        CreateMap<CreateCategoryRequest, CreateCategoryCommand>();
        CreateMap<EditCategoryRequest, EditCategoryCommand>();

        CreateMap<CreateTransactionRequest, CreateTransactionCommand>();
        CreateMap<EditTransactionRequest, EditTransactionCommand>();
        CreateMap<CreateRecurringTransactionRequest, CreateRecurringTransactionCommand>();
    }
}