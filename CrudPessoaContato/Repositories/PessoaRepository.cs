using CrudPessoaContato.Dtos;
using CrudPessoaContato.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CrudPessoaContato.Repositories
{
    public class PessoaRepository
    {
        //No Senac:
        //private readonly string _connection = @"Data Source=ITELABD18\SQLEXPRESS;Initial Catalog=Cadastro;Integrated Security=True;";
        //Em casa:
        private readonly string _connection = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Cadastro;Integrated Security=True;Pooling=False";
        public bool SalvarPessoa(Pessoa pessoa, Endereco endereco, List<Telefone> telefones) 
        {
            int IdPessoaCriada = -1;
            try
            {
                var query = @"INSERT INTO Pessoa 
                              (Nome, Cpf, DataNascimento, Ativo) 
                              OUTPUT Inserted.Id
                              VALUES (@nome,@cpf,@dataNascimento, 1)";
                using (var sql = new SqlConnection(_connection))
                {
                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@nome", pessoa.Nome);
                    command.Parameters.AddWithValue("@cpf", pessoa.Cpf);                    
                    command.Parameters.AddWithValue("@dataNascimento", pessoa.DataNascimento);                    
                    command.Connection.Open();
                    IdPessoaCriada = (int)command.ExecuteScalar();
                }

                SalvarEnderecoAoSalvarPessoa(endereco, IdPessoaCriada);

                SalvarTelefoneAoSalvarUsuario(telefones, IdPessoaCriada);

                Console.WriteLine("Pessoa cadastrada com sucesso.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public void SalvarTelefoneAoSalvarUsuario(List<Telefone> telefones, int IdPessoa)
        {
            try
            {
                foreach (var telefone in telefones)
                {
                    SalvarTelefoneAoSalvarUsuario(telefone, IdPessoa);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
        public void SalvarTelefoneAoSalvarUsuario(Telefone telefone, int IdPessoa)
        {
            try
            {
                var query = @"INSERT INTO Telefone 
                              (DDD, Numero, IdPessoa)                               
                              VALUES (@ddd,@numero,@idPessoa)";
                using (var sql = new SqlConnection(_connection))
                {
                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@ddd", telefone.DDD);
                    command.Parameters.AddWithValue("@numero", telefone.Numero);
                    command.Parameters.AddWithValue("@idPessoa", IdPessoa);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Telefone cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }        
        public void SalvarTelefones(List<TelefoneDto> telefones, int IdPessoa) 
        {
            try
            {
                foreach (var telefone in telefones) 
                {
                    if (telefone.Id > 0) 
                    {
                        AtualizarTelefone(telefone);
                    }
                    else
                    {
                        SalvarTelefone(telefone, IdPessoa);
                    }                    
                }                               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);                
            }
        }
        public void SalvarTelefone(TelefoneDto telefone, int IdPessoa)
        {
            try
            {
                var query = @"INSERT INTO Telefone 
                              (DDD, Numero, IdPessoa)                               
                              VALUES (@ddd,@numero,@idPessoa)";
                using (var sql = new SqlConnection(_connection))
                {
                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@ddd", telefone.DDD);
                    command.Parameters.AddWithValue("@numero", telefone.Numero);
                    command.Parameters.AddWithValue("@idPessoa", IdPessoa);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Telefone cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
        public void SalvarEnderecoAoSalvarPessoa(Endereco endereco, int IdPessoa) 
        {
            try
            {
                var query = @"INSERT INTO Endereco 
                              (Rua, Numero, Complemento, IdPessoa)                               
                              VALUES (@rua,@numero,@complemento, @idPessoa)";
                using (var sql = new SqlConnection(_connection))
                {
                    SqlCommand command = new SqlCommand(query, sql);
                    command.Parameters.AddWithValue("@rua", endereco.Rua);
                    command.Parameters.AddWithValue("@numero", endereco.Numero);
                    command.Parameters.AddWithValue("@complemento", endereco.Complemento);
                    command.Parameters.AddWithValue("@idPessoa", IdPessoa);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Endereço cadastrado com sucesso.");                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);                
            }
        }
        public List<PessoaDto> BuscarPorNome(string nome) 
        {
            List<PessoaDto> pessoasEncontradas;
            try
            {
                var query = @"SELECT Id, Nome, Cpf, DataNascimento FROM Pessoa
                                      WHERE Nome like CONCAT('%',@nome,'%')";
                
                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new 
                    {
                        nome
                    };
                    pessoasEncontradas = connection.Query<PessoaDto>(query, parametros).ToList();
                }

                pessoasEncontradas.ForEach(e => 
                {
                    e.Endereco = BuscarEnderecoPessoa(e.Id);
                    e.Telefones = BuscarTelefonesPessoa(e.Id);
                });

                return pessoasEncontradas;

                //maneira antiga
                //using (var sql = new SqlConnection(_connection))
                //{
                //    SqlCommand command = new SqlCommand(query, sql);
                //    command.Parameters.AddWithValue("@nome", nome);
                //    command.Connection.Open();
                //    resultado = command.ExecuteReader();

                //    while (resultado.Read())
                //    {
                //        pessoas.Add(new Pessoa(resultado.GetInt32(resultado.GetOrdinal("Id")),
                //                               resultado.GetString(resultado.GetOrdinal("Nome")),
                //                               resultado.GetString(resultado.GetOrdinal("Cpf")),                                                                                              
                //                               resultado.GetDateTime(resultado.GetOrdinal("DataNascimento"))));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return null;
            }            
        }
        public PessoaDto BuscarPorId(int id)
        {
            PessoaDto pessoasEncontradas;
            try
            {
                var query = @"SELECT Id, Nome, Cpf, DataNascimento FROM Pessoa
                                      WHERE Id = @id";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        id
                    };
                    pessoasEncontradas = connection.QueryFirstOrDefault<PessoaDto>(query, parametros);
                }

                pessoasEncontradas.Endereco = BuscarEnderecoPessoa(id);
                pessoasEncontradas.Telefones = BuscarTelefonesPessoa(id);

                return pessoasEncontradas;

                //maneira antiga
                //using (var sql = new SqlConnection(_connection))
                //{
                //    SqlCommand command = new SqlCommand(query, sql);
                //    command.Parameters.AddWithValue("@nome", nome);
                //    command.Connection.Open();
                //    resultado = command.ExecuteReader();

                //    while (resultado.Read())
                //    {
                //        pessoas.Add(new Pessoa(resultado.GetInt32(resultado.GetOrdinal("Id")),
                //                               resultado.GetString(resultado.GetOrdinal("Nome")),
                //                               resultado.GetString(resultado.GetOrdinal("Cpf")),                                                                                              
                //                               resultado.GetDateTime(resultado.GetOrdinal("DataNascimento"))));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return null;
            }
        }
        private EnderecoDto BuscarEnderecoPessoa(int idPessoa) 
        {

            try 
            {
                var query = @"SELECT * FROM Endereco
                                      WHERE IdPessoa = @idPessoa";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa
                    };
                    return connection.QueryFirstOrDefault<EnderecoDto>(query, parametros);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Erro: "+ex.Message);
                return null;
            }
        }
        private List<TelefoneDto> BuscarTelefonesPessoa(int idPessoa) 
        {
            try
            {
                var query = @"SELECT * FROM Telefone
                                      WHERE IdPessoa = @idPessoa";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa
                    };
                    return connection.Query<TelefoneDto>(query, parametros).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return null;
            }

        }
        public List<PessoaDto> BuscarTodos()
        {
            List<PessoaDto> pessoasEncontradas;
            try
            {
                var query = @"SELECT Id, Nome, Cpf, DataNascimento FROM Pessoa WHERE Ativo = 1";

                using (var connection = new SqlConnection(_connection))
                {
                    pessoasEncontradas = connection.Query<PessoaDto>(query).ToList();
                }

                pessoasEncontradas.ForEach(e =>
                {
                    e.Endereco = BuscarEnderecoPessoa(e.Id);
                    e.Telefones = BuscarTelefonesPessoa(e.Id);
                });

                return pessoasEncontradas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return null;
            }
        }
        public bool RemoverPessoa(int id) 
        {
            try
            {
                var query = @"UPDATE Pessoa SET Ativo = 0 WHERE Id = @id";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        id
                    };
                   connection.Execute(query, parametros);
                }

                return true;                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverPessoaHardDelete(int id) 
        {
            try
            {

                var res = RemoverEnderecoHardDelete(id);

                if (res) 
                {
                    res = RemoverTelefonesHardDelete(id);

                    if(res)
                    {
                        var query = @"DELETE Pessoa WHERE Id = @id";

                        using (var connection = new SqlConnection(_connection))
                        {
                            var parametros = new
                            {
                                id
                            };
                            connection.Query<PessoaDto>(query, parametros);
                        }
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverTelefonesPessoa(int idPessoa) 
        {
            try
            {
                var query = @"UPDATE Telefone SET Ativo = 0 WHERE IdPessoa = @idPessoa";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverTelefonesHardDelete(int idPessoa) 
        {
            try
            {
                var query = @"DELETE FROM Telefone WHERE IdPessoa = @idPessoa";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverTelefone(int idPessoa, int idTelefone) 
        {
            try
            {
                var query = @"UPDATE Telefone SET Ativo = 0 WHERE IdPessoa = @idPessoa AND Id = @idTelefone";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa,
                        idTelefone
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverTelefoneHardDelete(int idPessoa, int idTelefone) 
        {
            try
            {
                var query = @"DELETE FROM Telefone WHERE IdPessoa = @idPessoa and Id = @idTelefone";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa,
                        idTelefone
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverEndereco(int idPessoa) 
        {
            try
            {
                var query = @"UPDATE Endereco SET Ativo = 0 WHERE IdPessoa = @idPessoa";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa                        
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool RemoverEnderecoHardDelete(int idPessoa) 
        {
            try
            {
                var query = @"DELETE FROM Endereco WHERE IdPessoa = @idPessoa";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        idPessoa
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool AtualizarTelefone(TelefoneDto telefone) 
        {
            try
            {
                var query = @"UPDATE Telefone SET DDD = @ddd, Numero = @numero WHERE Id = @id";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        id = telefone.Id,
                        ddd = telefone.DDD,
                        numero = telefone.Numero
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool AtualizarEndereco(EnderecoDto endereco)
        {
            try
            {
                var query = @"UPDATE Endereco SET Rua = @rua, Numero = @numero, Complemento = @complemento WHERE Id = @id";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        id = endereco.Id,
                        rua = endereco.Rua,
                        complemento = endereco.Complemento,
                        numero = endereco.Numero                        
                    };
                    connection.Execute(query, parametros);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
        public bool AtualizarPessoa(PessoaDto pessoaDto)
        {
            try
            {
                var query = @"UPDATE Pessoa SET Nome = @nome, Cpf = @cpf, DataNascimento = @dataNascimento WHERE Id = @id";

                using (var connection = new SqlConnection(_connection))
                {
                    var parametros = new
                    {
                        id = pessoaDto.Id,
                        nome = pessoaDto.Nome,
                        cpf = pessoaDto.Cpf,
                        dataNascimento = pessoaDto.DataNascimento
                    };
                    connection.Execute(query, parametros);
                }

                if(pessoaDto.Endereco.Id > 0) 
                {
                    AtualizarEndereco(pessoaDto.Endereco);
                }
                if(pessoaDto.Telefones != null && pessoaDto.Telefones.Any())
                {
                    SalvarTelefones(pessoaDto.Telefones, pessoaDto.Id);
                }
                if (pessoaDto.Endereco.Id > 0) 
                {
                    
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }
        }
    }
}
