using CrudPessoaContato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudPessoaContato.ViewModels
{
    public class SalvarPessoaViewModel
    {
        public Pessoa Pessoa { get; set; }
        public Endereco Endereco { get; set; }
        public List<Telefone> Telefones { get; set; }
    }
}
