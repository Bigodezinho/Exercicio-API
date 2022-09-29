using CrudPessoaContato.Client.Dtos;
using CrudPessoaContato.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CrudPessoaContato.Client
{
    public class CrudPessoaService
    {
        public List<PessoaDto> BuscarTodos() 
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            //Busca todos os clientes dentro da api;
            try
            {
                //monta a request para a api;
                response = httpClient.GetAsync("https://localhost:44317/pessoa/buscartodos").Result;
                response.EnsureSuccessStatusCode();

                var resultado = response.Content.ReadAsStringAsync().Result;

                //converte os dados recebidos e retorna eles como objetos do C#;
                var objetoDesserializado = JsonConvert.DeserializeObject<List<PessoaDto>>(resultado);

                return objetoDesserializado;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
        public void EnviarPessoa(Pessoa pessoa, Endereco endereco, List<Telefone> telefones) 
        {
            //recebe os dados para enviar para a API cria a viewModel que será enviada;
            var viewModel = new 
            {
                pessoa, 
                endereco,
                telefones
            };

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            //converte o objeto em um JSON 
            var json = JsonConvert.SerializeObject(viewModel);

            try
            {
                //envia os dados para a API, convertendo em uma cadeia de string
                response = httpClient.PostAsync("https://localhost:44317/pessoa/salvar", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();
                //faz a request, envia os dados e recebe a resposta da API.
                var resultado = response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);                
            }
        }

    }
}
