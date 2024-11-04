using FI.WebAtividadeEntrevista.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        public long IdCliente { get; set; }

        /// <summary>
        /// Cpf
        /// </summary>
        [Required]
        [Cpf(ErrorMessage = "Digite um CPF válido.")]
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }
    }
}