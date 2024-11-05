using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FI.AtividadeEntrevista.DML;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiario : AcessoDados
    {
        private string stringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["BancoDeDados"];
                return conn != null ? conn.ConnectionString : string.Empty;
            }
        }

        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal long Incluir(Beneficiario beneficiario)
        {
            var parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new SqlParameter("CPF", beneficiario.IdCliente));

            DataSet ds = Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Inclui beneficiarios
        /// </summary>
        /// <param name="beneficiarios">Objeto de beneficiarios</param>
        internal void Incluir(IList<Beneficiario> beneficiarios)
        {
            using (var connection = new SqlConnection(stringDeConexao))
            {
                connection.Open();

                DataTable beneficiarioTable = new DataTable();
                beneficiarioTable.Columns.Add("Nome", typeof(string));
                beneficiarioTable.Columns.Add("CPF", typeof(string));
                beneficiarioTable.Columns.Add("IdCliente", typeof(int));

                foreach (var beneficiario in beneficiarios)
                {
                    beneficiarioTable.Rows.Add(beneficiario.Nome, beneficiario.CPF, beneficiario.IdCliente);
                }

                using (var command = new SqlCommand("FI_SP_IncBeneficiarios", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter param = command.Parameters.AddWithValue("@Beneficiarios", beneficiarioTable);
                    param.SqlDbType = SqlDbType.Structured;

                    command.ExecuteNonQuery();
                }
            }
        }

        internal List<Beneficiario> Listar(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("IdCliente", idCliente));

            DataSet ds = Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> cli = Converter(ds);

            return cli;
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario cli = new Beneficiario();
                    cli.Id = row.Field<long>("Id");
                    cli.CPF = row.Field<string>("CPF");
                    cli.Nome = row.Field<string>("Nome");
                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}
