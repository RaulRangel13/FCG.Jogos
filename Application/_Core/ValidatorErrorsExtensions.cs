using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UTIL.Errors;

namespace Application._Core
{
    public static class ValidatorErrorsExtensions
    {

        private static readonly HashSet<string> CustomErrorCodes = new HashSet<string>(
            typeof(ErrorIssuesConst)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly) // Filtra apenas por campos const
                .Select(fi => fi.GetValue(null)?.ToString()) // Obtém o valor de cada campo
                .OfType<string>()
        );


        public static List<ErrorItemDetail> GetErrorsFromValidation(this ValidationResult result)
        {

            return result.Errors.Select(fail => {

                var issueCode = CustomErrorCodes.Contains(fail.ErrorCode) ? fail.ErrorCode : ErrorIssuesConst.ValidationError;

                return new ErrorItemDetail
                {
                    issue = issueCode,
                    description = fail.ErrorMessage,
                    value = fail.AttemptedValue?.ToString()?.StartsWith("Domain.") ?? false ? string.Empty : fail.AttemptedValue?.ToString() ?? string.Empty,
                    field = fail.FormattedMessagePlaceholderValues.ContainsKey("PropertyName") ? fail.FormattedMessagePlaceholderValues["PropertyName"]?.ToString() : fail.PropertyName
                };
            }).ToList();
        }
    }
}
