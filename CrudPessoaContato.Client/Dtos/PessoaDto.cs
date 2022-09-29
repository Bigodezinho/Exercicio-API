using System;
using System.Collections.Generic;
using System.Text;

namespace CrudPessoaContato.Client.Dtos
{    
    public class PessoaDto
    {
        public int Id { get; set; }
        public string Nome{get;set;}
        public DateTime DataNascimento { get; set; }
        public EnderecoDto Endereco { get; set; }
        public List<TelefoneDto> Telefones { get; set; }
    }
}
