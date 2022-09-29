using CrudPessoaContato.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrudPessoaContato.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //Mostrar menu para o usuário;
            
            CrudPessoaService crudPessoaService = new CrudPessoaService();
            //buscar todos
            var todos = crudPessoaService.BuscarTodos();

            foreach(var item in todos) 
            {
                var pessoa = new Pessoa()
                {
                    DataNascimento = item.DataNascimento,
                    Nome = string.IsNullOrEmpty(item.Nome) ? "TESTE NOME" : item.Nome,
                    Cpf = "000000000",
                    Idade = new Random().Next(0,100) //gera um valor aleatório para cada execução entre 0 e 100  
                };
                var endereco = new Endereco()
                {
                    Complemento = string.IsNullOrEmpty(item?.Endereco?.Complemento) ? "TESTE COMPLEMENTO" : item?.Endereco?.Complemento,
                    Numero = string.IsNullOrEmpty(item?.Endereco?.Numero) ? "TESTE NUMERO" : item?.Endereco?.Numero,
                    Rua = string.IsNullOrEmpty(item?.Endereco?.Rua) ? "TESTE RUA" : item?.Endereco?.Rua,
                };

                var telefones = new List<Telefone>();

                if (item?.Telefones?.Count > 0) 
                {
                    item?.Telefones?.ForEach(e =>
                    {
                        telefones.Add(new Telefone()
                        {
                            DDD = string.IsNullOrEmpty(e.DDD) ? "TESTE DDD" : e.DDD,
                            Numero = string.IsNullOrEmpty(e.Numero) ? "TESTE NUMERO" : e.Numero
                        });
                    });
                }
                else 
                {
                    telefones.Add(new Telefone()
                    {
                        DDD = "TESTE DDD 1",
                        Numero = "TESTE NUMERO 1"
                    });
                    telefones.Add(new Telefone()
                    {
                        DDD = "TESTE DDD 2",
                        Numero = "TESTE NUMERO 2"
                    });
                }

                crudPessoaService.EnviarPessoa(pessoa, endereco, telefones);
            }
        }
    }
}