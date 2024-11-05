using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using FI.WebAtividadeEntrevista;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteModel _clienteModel;
        private readonly IMapper _mapper;

        public ClienteController()
        {
            ViewBag.Estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "SP", Text = "São Paulo" },
                new SelectListItem { Value = "PE", Text = "Pernambuco" }
            };

            _mapper = AutoMapperConfig.Mapper;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            var model = new ClienteBeneficiarioModel
            {
                Cliente = new ClienteModel(),
                Beneficiario = new BeneficiarioModel(),
                Beneficiarios = new List<BeneficiarioModel>()
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Incluir(ClienteBeneficiarioModel model)
        {
            if (!ValidaModel(model.Cliente))
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            var boCliente = new BoCliente();
            var boBeneficiario = new BoBeneficiario();

            if (boCliente.VerificarExistencia(model.Cliente.CPF))
            {
                Response.StatusCode = 400;
                return Json("O CPF informado já está cadastrado.");
            }

            model.Cliente.Id = boCliente.Incluir(new Cliente
            {                    
                CEP = model.Cliente.CEP,
                Cidade = model.Cliente.Cidade,
                CPF = model.Cliente.CPF,
                Email = model.Cliente.Email,
                Estado = model.Cliente.Estado,
                Logradouro = model.Cliente.Logradouro,
                Nacionalidade = model.Cliente.Nacionalidade,
                Nome = model.Cliente.Nome,
                Sobrenome = model.Cliente.Sobrenome,
                Telefone = model.Cliente.Telefone
            });

            if (!model.Beneficiarios.Any()) return Json("Cadastro efetuado com sucesso");
            
            IList<Beneficiario> beneficiarios = model.Beneficiarios
                .Where(b => b.Id == 0)
                .Select(m => _mapper.Map<Beneficiario>(m, opt =>
                {
                    opt.Items["IdCliente"] = model.Cliente.Id;
                }))
                .ToList();

            boBeneficiario.Incluir(beneficiarios);


            return Json("Cadastro efetuado com sucesso");
        }

        [HttpPost]
        public ActionResult ListarBeneficiarios(ClienteBeneficiarioModel model)
        {
            return PartialView("_ListaBeneficiarios", model.Beneficiarios);
        }

        [HttpPost]
        public JsonResult Alterar(ClienteBeneficiarioModel model)
        {
            if (!ValidaModel(model.Cliente))
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            var boCliente = new BoCliente();
            var boBeneficiario = new BoBeneficiario();

            if (boCliente.VerificarExistencia(model.Cliente.CPF))
            {
                Cliente cliente = boCliente.Consultar(model.Cliente.Id);
                
                if (cliente.Id != model.Cliente.Id && cliente.CPF == model.Cliente.CPF)
                {
                    Response.StatusCode = 400;
                    return Json("O CPF informado já está cadastrado.");
                }
            }

            boCliente.Alterar(new Cliente()
            {
                Id = model.Cliente.Id,
                CEP = model.Cliente.CEP,
                Cidade = model.Cliente.Cidade,
                CPF = model.Cliente.CPF,
                Email = model.Cliente.Email,
                Estado = model.Cliente.Estado,
                Logradouro = model.Cliente.Logradouro,
                Nacionalidade = model.Cliente.Nacionalidade,
                Nome = model.Cliente.Nome,
                Sobrenome = model.Cliente.Sobrenome,
                Telefone = model.Cliente.Telefone
            });

            if (!model.Beneficiarios.Any()) return Json("Cadastro efetuado com sucesso");

            IList<Beneficiario> beneficiarios = model.Beneficiarios
                .Where(b => b.Id == 0)
                .Select(m => _mapper.Map<Beneficiario>(m, opt =>
                {
                    opt.Items["IdCliente"] = model.Cliente.Id;
                }))
                .ToList();

            boBeneficiario.Incluir(beneficiarios);

            return Json("Cadastro alterado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            var boCliente = new BoCliente();
            var boBeneficiario = new BoBeneficiario();

            Cliente cliente = boCliente.Consultar(id);
            IList<Beneficiario> beneficiarios = boBeneficiario.Listar(cliente.Id);

            IList<BeneficiarioModel> beneficiariosModel = beneficiarios.Select(m => _mapper.Map<BeneficiarioModel>(m)).ToList();

            ClienteBeneficiarioModel model = null;

            if (cliente != null)
            {
                model = new ClienteBeneficiarioModel()
                {
                    Cliente = new ClienteModel()
                    {
                        Id = cliente.Id,
                        CEP = cliente.CEP,
                        CPF = cliente.CPF,
                        Cidade = cliente.Cidade,
                        Email = cliente.Email,
                        Estado = cliente.Estado,
                        Logradouro = cliente.Logradouro,
                        Nacionalidade = cliente.Nacionalidade,
                        Nome = cliente.Nome,
                        Sobrenome = cliente.Sobrenome,
                        Telefone = cliente.Telefone
                    },
                    Beneficiario = new BeneficiarioModel(),
                    Beneficiarios = beneficiariosModel
                };
            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public bool ValidaModel<T>(T model)
        {
            var isValid = true;

            if (model != null)
            {
                var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
                var validationResults = new List<ValidationResult>();

                isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            }

            return isValid;
        }
    }
}