using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Incluir(IList<Beneficiario> beneficiarios)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            cli.Incluir(beneficiarios);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<Beneficiario> Listar(long idCliente)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Listar(idCliente);
        }
    }
}
