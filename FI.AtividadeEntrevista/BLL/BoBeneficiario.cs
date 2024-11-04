using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Incluir(beneficiario);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiario> Listar(long idCliente)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Listar(idCliente);
        }
    }
}
