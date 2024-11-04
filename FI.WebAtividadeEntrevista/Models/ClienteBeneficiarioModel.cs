using System.Collections.Generic;

namespace WebAtividadeEntrevista.Models
{
    public class ClienteBeneficiarioModel
    {
        public ClienteModel Cliente { get; set; }
        public BeneficiarioModel Beneficiario { get; set; }
        public ICollection<BeneficiarioModel> Beneficiarios { get; set; }
    }
}