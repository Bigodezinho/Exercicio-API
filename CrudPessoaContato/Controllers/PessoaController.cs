using CrudPessoaContato.Models;
using CrudPessoaContato.Repositories;
using CrudPessoaContato.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudPessoaContato.Controllers
{
    [Route("[controller]/[action]")]    
    [ApiController]
    public class PessoaController : ControllerBase
    {        
        private readonly PessoaRepository _pessoaRepository;

        public PessoaController() 
        {
            _pessoaRepository = new PessoaRepository();
        }       
        [HttpPost]
        public IActionResult Salvar(SalvarPessoaViewModel salvarPessoaViewModel ) 
        {
            if (salvarPessoaViewModel == null)
                return Ok("Não foram informados dados");

            if (salvarPessoaViewModel.Pessoa == null)
                return Ok("Dados da pessoa não informados.");

            if (salvarPessoaViewModel.Endereco == null)
                throw new ArgumentNullException($"campo {nameof(salvarPessoaViewModel.Endereco)} vazio ou nulo.");

            if(salvarPessoaViewModel.Telefones == null || !salvarPessoaViewModel.Telefones.Any())
                throw new ArgumentNullException($"campo {nameof(salvarPessoaViewModel.Telefones)} vazio ou nulo.");

            var resultado = _pessoaRepository.SalvarPessoa(salvarPessoaViewModel.Pessoa, 
                                                           salvarPessoaViewModel.Endereco, 
                                                           salvarPessoaViewModel.Telefones);

            if (resultado) return Ok("Pessoa cadastrada com sucesso.");

            return Ok("Houve um problema ao salvar. Pessoa não cadastrada.");
        }
        [HttpPost]
        public IActionResult BuscarPorNome(string nome) 
        {
            var resultado = _pessoaRepository.BuscarPorNome(nome);

            if (resultado == null || !resultado.Any())
                return NotFound("Nenhum registro encontrado com o nome informado.");

            return Ok(resultado);
        }
        [HttpDelete]
        public IActionResult Remover(int id)
        {
            var resultado = _pessoaRepository.RemoverPessoa(id);

            if (resultado)
                return Ok("Pessoa removida com sucesso!");
            return BadRequest("Não foi possível remover a pessoa. Contate o administrador do sistema.");
        }
        [HttpGet]
        public IActionResult BuscarPorId(int id) 
        {
            var resultado = _pessoaRepository.BuscarPorId(id);

            if (resultado == null)
                return NotFound("Nenhum registro encontrado com o código informado.");

            return Ok(resultado);
        }
        [HttpGet]
        public IActionResult BuscarTodos() 
        {
            var resultado = _pessoaRepository.BuscarTodos();

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }  
        [HttpPut]
        public IActionResult AtualizarPessoa(AtualizarPessoaViewModel atualizarPessoaViewModel) 
        {
            var res = _pessoaRepository.AtualizarPessoa(atualizarPessoaViewModel.Pessoa);

            if (res)
                return Ok("Pessoa atualizada com sucesso.");
            return BadRequest("Não foi possível atualizar os dados da pessoa. Contate o administrador.");
        }
        [HttpDelete]
        public IActionResult RemoverHd(int idPessoa) 
        {
            var res = _pessoaRepository.RemoverPessoaHardDelete(idPessoa);

            if (res)
                return Ok("Pessoa removida com sucesso.");

            return BadRequest("Não foi possível remover a pessoa. Contate o adminstrador.");
        }
    }
}
