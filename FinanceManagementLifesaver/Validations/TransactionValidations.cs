﻿using FinanceManagementLifesaver.DTO;
using FluentValidation;

namespace FinanceManagementLifesaver.Validations
{
    public class TransactionValidations : AbstractValidator<TransactionDTO>
    {
        public TransactionValidations()
        {
            RuleSet("Enums", () =>
            {
                RuleFor(t => t.TransactionType).IsInEnum().WithMessage("Keine richtiger Transaktionstyp");
            });

            RuleSet("Dates", () =>
            {
                RuleFor(t => t.Date).NotNull().WithMessage("Kein Datum angegeben");
            });
            RuleSet("Description", () =>
            {
                RuleFor(t => t.Description).MaximumLength(300).WithMessage("Beschreibung kann maximal 300 Zeichen lang sein");
            });
        }
    }
}
