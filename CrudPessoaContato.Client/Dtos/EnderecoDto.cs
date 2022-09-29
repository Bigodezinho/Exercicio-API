using System;
using System.Collections.Generic;
using System.Text;

namespace CrudPessoaContato.Client.Dtos
{
    public class EnderecoDto
    {
        public int Id { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
    }
}
