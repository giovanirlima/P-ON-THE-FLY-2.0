using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PON_THE_FLY_2.O.Entidades;

namespace POnTheFly
{
    public class Passageiro
    {
        public static bool ReadCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        public static void CadastrarPassageiro()
        {
            BancoAeroporto caminho = new();
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string sql, cpf, nome = "", sexo;
            bool validacao = false;
            DateTime nascimento = new();
            int opcao = 0;

            Console.Clear();

            Console.WriteLine("Formulário de cadastro:\n");

            do
            {
                Console.Write("Informe seu nome completo[OBRIGATÓRIO]: ");
                try
                {
                    nome = Console.ReadLine().ToUpper();
                    validacao = false;
                }
                catch (Exception)
                {
                    Mensagem.ParametroMessage();
                    validacao = true;
                }

                if (nome.Length == 0)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nNome informado não pode ser nullo!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            do
            {
                Console.Write("Digite o numero do seu CPF[OBRIGATÓRIO]: ");
                cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);
                validacao = false;

                if (!ReadCPF(cpf))
                {
                    Console.WriteLine("\nCPF inválido!\n");
                    validacao = true;
                }

            } while (validacao);

            sql = $"SELECT * FROM Passageiro WHERE CPF = '{cpf}'";

            if (BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nPassageiro já possue um cadastro!");
                return;
            }

            do
            {
                Console.Write("Informe sua data de nascimento: ");
                try
                {
                    nascimento = DateTime.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nParametro digitado é inválido!");
                    Console.WriteLine("Formato correto: [dd/mm/yyyy]\n");
                    validacao = true;
                }

            } while (validacao);

            Console.WriteLine("\nEscolha uma das opções abaixo:");
            Console.WriteLine("\n1 - Feminino");
            Console.WriteLine("2 - Masculino");
            Console.WriteLine("3 - Prefiro não me identificar");
            do
            {
                Console.Write("\nOpção: ");
                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Mensagem.ParametroMessage();
                    validacao = true;
                }

                if (opcao < 1 || opcao > 3)
                {
                    if (!validacao)
                    {
                        Mensagem.OpcaoMessage();
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                sexo = "FEMININO";
            }
            else
            {
                if (opcao == 2)
                {
                    sexo = "MASCULINO";
                }

                else
                {
                    sexo = "INDEFINIDO";
                }
            }

            sql = $"INSERT Passageiro VALUES('{cpf}', '{nome}', '{nascimento}', '{sexo}', '{DateTime.Now.ToShortDateString()}', 'ATIVO');";

            if (BancoAeroporto.InsertDados(sql, conexao))
            {
                Mensagem.TrueCadastradoMessage();
                return;
            }

            Mensagem.FalseCadastradoMessage();
        }
        public static void EditarPassageiro()
        {
            BancoAeroporto caminho = new();
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            DateTime nascimento = new();
            string cpf, sql, nome, parametro, retorno;
            bool validacao;
            int opcao = 10;

            Console.Clear();

            Console.WriteLine("PAINEL DE EDIÇÃO\n");

            Console.Write("Informe o cpf do Passageiro: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            sql = $"SELECT * From Passageiro WHERE CPF = '{cpf}'";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("CPF informado não possue cadastro!");
                return;
            }

            do
            {
                Console.Clear();

                Console.WriteLine("Informe qual dado deseja alterar: ");
                Console.WriteLine("\n1 - Nome");
                Console.WriteLine("2 - Data de Nascimento");
                Console.WriteLine("3 - Sexo");
                Console.WriteLine("4 - Situação");
                Console.WriteLine("\n9 - Voltar ao menu anterior");
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Mensagem.ParametroMessage();
                    validacao = true;
                }

                if (opcao < 1 || opcao > 4 && opcao != 9)
                {
                    if (!validacao)
                    {
                        Mensagem.OpcaoMessage();
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 9)
            {
                Console.Write("\nAté Logo");
                return;
            }


            if (opcao == 1)
            {
                do
                {
                    Console.Write("\nInforme o nome do Passageiro: ");
                    nome = Console.ReadLine().ToUpper();
                    validacao = false;

                    if (nome.Length == 0)
                    {
                        Console.Write("\nNome não pode ser nullo!\n");
                        validacao = true;
                    }

                } while (validacao);

                sql = $"UPDATE Passageiro SET Nome = '{nome}' WHERE CPF = '{cpf}';";

                if (BancoAeroporto.UpdateDados(sql, conexao))
                {
                    Mensagem.TrueAlteradoMessage();
                    return;
                }

                Mensagem.FalseAlteradoMessage();
                return;
            }

            if (opcao == 2)
            {

                do
                {
                    Console.Write("\nInforme a data de nascimento: ");
                    try
                    {
                        nascimento = DateTime.Parse(Console.ReadLine());
                        validacao = false;
                    }
                    catch (Exception)
                    {
                        Mensagem.ParametroMessage();
                        validacao = true;
                    }

                    if (nascimento > DateTime.Now)
                    {
                        Console.Write("\nData de nascimento não pode ser futura!\n");
                        validacao = true;
                    }
                } while (validacao);

                sql = $"UPDATE Passageiro SET Nascimento = '{nascimento}' WHERE CPF = '{cpf}';";

                if (BancoAeroporto.UpdateDados(sql, conexao))
                {
                    Mensagem.TrueAlteradoMessage();
                    return;
                }

                Mensagem.FalseAlteradoMessage();
                return;

            }


            if (opcao == 3)
            {
                Console.WriteLine("\nEscolha uma das opções: ");
                Console.WriteLine("\n1 - Feminino");
                Console.WriteLine("2 - Masculino");
                Console.WriteLine("3 - Prefiro não me identificar");
                do
                {
                    Console.Write("\nOpção: ");
                    try
                    {
                        opcao = int.Parse(Console.ReadLine());
                        validacao = false;
                    }

                    catch (Exception)
                    {
                        Mensagem.ParametroMessage();
                        validacao = true;
                    }

                    if (opcao < 1 || opcao > 3)
                    {
                        if (!validacao)
                        {
                            Mensagem.OpcaoMessage();
                            validacao = true;
                        }
                    }

                } while (validacao);

                if (opcao == 1)
                {
                    sql = $"UPDATE Passageiro SET Sexo = 'FEMININO' WHERE CPF = '{cpf}';";

                    if (BancoAeroporto.UpdateDados(sql, conexao))
                    {
                        Mensagem.TrueAlteradoMessage();
                        return;
                    }

                    Mensagem.FalseAlteradoMessage();
                    return;
                }
                else
                {
                    if (opcao == 2)
                    {
                        sql = $"UPDATE Passageiro SET Sexo = 'MASCULINO' WHERE CPF = '{cpf}';";

                        if (BancoAeroporto.UpdateDados(sql, conexao))
                        {
                            Mensagem.TrueAlteradoMessage();
                            return;
                        }

                        Mensagem.FalseAlteradoMessage();
                        return;
                    }

                    else
                    {
                        sql = $"UPDATE Passageiro SET Sexo = 'INDEFINIDO' WHERE CPF = '{cpf}';";

                        if (BancoAeroporto.UpdateDados(sql, conexao))
                        {
                            Mensagem.TrueAlteradoMessage();
                            return;
                        }

                        Mensagem.FalseAlteradoMessage();
                        return;
                    }
                }
            }


            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Passageiro WHERE CPF = '{cpf}';";

            retorno = BancoAeroporto.RetornoDados(sql, conexao, parametro);

            if (retorno == "ATIVO")
            {
                Console.WriteLine("\nPassageiro está ativo!");
                Console.WriteLine("Deseja alterar a situação do passageiro para inativo: ");
                Console.WriteLine("\n1 - Sim\n2 - Não");
                do
                {
                    Console.Write("\nOpção: ");

                    try
                    {
                        opcao = int.Parse(Console.ReadLine());
                        validacao = false;
                    }

                    catch (Exception)
                    {
                        Console.WriteLine("\nParametro de dado inválido!\n");
                        validacao = true;
                    }

                    if (opcao < 1 || opcao > 2)
                    {
                        if (!validacao)
                        {
                            Console.WriteLine("\nEscolha uma das opções listadas!\n");
                            validacao = true;
                        }
                    }

                } while (validacao);

                if (opcao == 1)
                {
                    sql = $"UPDATE Passageiro SET Situacao = 'INATIVO' WHERE CPF = '{cpf}';";

                    if (BancoAeroporto.UpdateDados(sql, conexao))
                    {
                        Mensagem.TrueAlteradoMessage();
                        return;
                    }

                    Mensagem.FalseAlteradoMessage();
                    return;
                }

                Console.Write("\nAté logo");
                return;
            }


            Console.WriteLine("\nPassageiro está inativo!");
            Console.WriteLine("Deseja alterar a situação do passageiro para ativo: ");
            Console.WriteLine("\n1 - Sim\n2 - Não");
            do
            {
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("\nParametro de dado inválido!\n");
                    validacao = true;
                }

                if (opcao < 1 || opcao > 2)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nEscolha uma das opções listadas!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                sql = $"UPDATE Passageiro SET Situacao = 'ATIVO' WHERE CPF = '{cpf}';";

                if (BancoAeroporto.UpdateDados(sql, conexao))
                {
                    Mensagem.TrueAlteradoMessage();
                    return;
                }

                Mensagem.FalseAlteradoMessage();
                return;
            }

            Console.Write("\nAté logo");
            return;
        }
        public static void ImprimirPassageiro()
        {
            BancoAeroporto caminho = new();
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            SqlCommand cmd;
            int opcao = 0;
            bool validacao;
            string sql, cpf;

            Console.Clear();

            Console.WriteLine("Ola,");
            Console.WriteLine("\nEscolha a opção desejada:\n");
            Console.WriteLine("1 - Ver Passageiros Cadastrados");
            Console.WriteLine("2 - Ver um especifico");
            Console.WriteLine("\n9 - Voltar ao menu anterior");
            do
            {
                Console.Write("\nOpção: ");
                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Mensagem.ParametroMessage();
                    validacao = true;
                }

                if (opcao < 1 || opcao > 2 && opcao != 9)
                {
                    if (!validacao)
                    {
                        Mensagem.OpcaoMessage();
                        validacao = true;
                    }
                }
            } while (validacao);

            if (opcao == 1)
            {
                conexao.Open();               

                cmd = new("SELECT * FROM Passageiro;", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("PASSAGEIROS CADASTRADOS\n");
                        Console.WriteLine($"CPF: {reader.GetString(0)}");
                        Console.WriteLine($"Nome: {reader.GetString(1)}");
                        Console.WriteLine($"Data de Nascimento: {reader.GetDateTime(2).ToShortDateString()}");
                        Console.WriteLine($"Sexo: {reader.GetString(3)}");
                        Console.WriteLine($"Data Ulima Compra: R$ {reader.GetDateTime(4).ToShortDateString()}");
                        Console.WriteLine($"Situação: {reader.GetString(5)}");
                    }
                }

                Console.Write("\nPressione enter para continuar!");
                conexao.Close();
                return;
            }

            if (opcao == 9)
            {
                Console.Write("\nAté logo!");
                return;
            }

            Console.Clear();

            Console.Write("\nInforme qual CPF do Passageiro que deseja localizar: ");
            try
            {
                cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Passageiro WHERE CPF = '{cpf}';";
                        

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nPassageiro não cadastrado!");
                return;
            }            

            conexao.Open();

            cmd = new("SELECT * FROM Passageiro WHERE CPF = '{cpf}';", conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("PASSAGEIROS CADASTRADOS\n");
                    Console.WriteLine($"CPF: {reader.GetString(0)}");
                    Console.WriteLine($"Nome: {reader.GetString(1)}");
                    Console.WriteLine($"Data de Nascimento: {reader.GetDateTime(2).ToShortDateString()}");
                    Console.WriteLine($"Sexo: {reader.GetString(3)}");
                    Console.WriteLine($"Data Ulima Compra: R$ {reader.GetDateTime(4).ToShortDateString()}");
                    Console.WriteLine($"Situação: {reader.GetString(5)}");
                }
            }

            Console.Write("\nPressione enter para continuar!");
            conexao.Close();
            return;
        }
        public static void AcessarPassageiro()
        {
            int opcao = -1;
            bool validacao;            

            do
            {
                Console.Clear();

                Console.WriteLine("ACESSAR PASSAGEIROS\n");

                Console.WriteLine("1 - Cadastrar Passageiro");                
                Console.WriteLine("2 - Editar Passageiro");
                Console.WriteLine("3 - Imprimir Passageiro");
                Console.WriteLine("\n9 - Voltar ao menu anterior");
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Mensagem.ParametroMessage();                   
                    Console.ReadKey();
                    validacao = true;
                }

                if (opcao < 0 || opcao > 4 && opcao != 9)
                {
                    if (!validacao)
                    {
                        Mensagem.OpcaoMessage();                        
                        Console.ReadKey();                        
                    }
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarPassageiro();
                        Console.ReadKey();
                        break;

                    case 2:
                        EditarPassageiro();
                        Console.ReadKey();
                        break;                   

                    case 3:
                        ImprimirPassageiro();
                        Console.ReadKey();
                        break;
                }

            } while (opcao != 9);
        }
    }
}