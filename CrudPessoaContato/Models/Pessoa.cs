using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudPessoaContato.Models
{
    public class Pessoa
    {        
        public int Idade 
        {
            get 
            {
                return DateTime.Now.Year - DataNascimento.Year;
            } 
        }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }       
    }
}
