using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Attributes
{
    public class CpfAttribute : ValidationAttribute, IClientValidatable
    {
        public CpfAttribute() : base("{0} é inválido.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) => 
            IsCpfValid(value as string)
                ? ValidationResult.Success
                : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(null),
                ValidationType = "cpf"
            };
        }


        private static bool IsCpfValid(string cpf)
        {
            // Verifica se o valor está presente
            if (string.IsNullOrWhiteSpace(cpf))
                return true;

            cpf = cpf.Replace(".", "").Replace("-", "");

            // Verifica se todos os dígitos são iguais
            var mesmosNumeros = new Regex(@"^(\d)\1{10}$");

            // Valida se o CPF possui 11 dígitos e se todos os números não são iguais
            if (cpf.Length != 11 || mesmosNumeros.IsMatch(cpf))
                return false;

            // Valida os dígitos verificadores
            for (var i = 9; i < 11; i++)
            {
                var soma = 0;
                for (var j = 0; j < i; j++)
                {
                    soma += int.Parse(cpf[j].ToString()) * (i + 1 - j);
                }
                int restante = soma % 11;
                restante = restante < 2 ? 0 : 11 - restante;

                if (restante != int.Parse(cpf[i].ToString()))
                    return false;
            }

            return true;
        }
    }
}
