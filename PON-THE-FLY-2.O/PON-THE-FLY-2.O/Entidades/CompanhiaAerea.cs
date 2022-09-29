using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PON_THE_FLY_2.O.Entidades
{
    public class CompanhiaAerea
    {
        public static void CadastrarCompanhia()
        {            
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());

            bool condicaoDeSaida;
            string cnpj, razaoSocial, sql;
            DateTime dataAbertura = new();


            Console.Clear();

            Console.WriteLine("Vamos iniciar seu cadastro\n");
            Console.Write("Informe a Razão social: ");
            razaoSocial = Console.ReadLine().ToUpper();

            do
            {
                Console.Write("Informe o número do CNPJ: ");

                cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);
                condicaoDeSaida = false;

                if (!ValidarCnpj(cnpj))
                {
                    Console.WriteLine("\nCNPJ digitado é inválido!\n");
                    condicaoDeSaida = true;
                }

            } while (condicaoDeSaida);

            sql = $"SELECT * FROM CompanhiaAerea WHERE CNPJ = {cnpj}";

            if (BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCompanhia Aerea já cadastrada!\n");
                return;
            }

            do
            {
                Console.Write("Informe a Data da abertura do CNPJ - (dd/mm/aaaa): ");
                try
                {
                    dataAbertura = DateTime.Parse(Console.ReadLine());
                    condicaoDeSaida = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("\nData informado deve seguir o formato informado: (dd/mm/aa)\n");
                    condicaoDeSaida = true;
                }

                if (dataAbertura > DateTime.Now)
                {
                    Console.WriteLine("\nData não pode ser maior do que hoje!\n");
                    condicaoDeSaida = true;
                }

            } while (condicaoDeSaida);

            sql = $"INSERT CompanhiaAerea VALUES('{cnpj}', '{razaoSocial}', '{dataAbertura}', '{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortDateString()}', 'ATIVA')";

            if (BancoAeroporto.InsertDados(sql, conexao))
            {
                Mensagem.TrueAlteradoMessage();
                return;
            }

            Console.WriteLine("\nFalha ao cadastrar!");
        }
        public static void EditarCompanhia()
        {            
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            int opcao = 0;
            bool condicaoDeSaida;
            DateTime dataAbertura = new();
            string razaoSocial, cnpj, parametro, retorno, sql;

            Console.Clear();

            Console.Write("Informe o número do CNPJ: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);

            if (!ValidarCnpj(cnpj))
            {
                Console.Write("\nCNPJ digitado é inválido!");
                return;
            }

            sql = $"SELECT * FROM CompanhiaAerea WHERE CNPJ = {cnpj}";            

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("CNPJ informado não está cadastrado!");
            }

            do
            {
                Console.Clear();

                Console.WriteLine("Informe qual dado deseja editar: \n");
                Console.Write("1 - Razão social\n2 - Data de abertura do CNPJ\n3 - Situação \n\n");
                Console.Write("Opção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    condicaoDeSaida = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("\nParametro informado é inválido!");
                    Console.WriteLine("Pressione enter para continuar!");
                    Console.ReadKey();
                    condicaoDeSaida = true;
                }

                if (opcao < 1 || opcao > 3)
                {
                    if (!condicaoDeSaida)
                    {
                        Console.WriteLine("\nOpção informada é inválida!");
                        Console.WriteLine("Pressione enter para continuar!");
                        Console.ReadKey();
                        condicaoDeSaida = true;
                    }
                }

            } while (condicaoDeSaida);

            if (opcao == 1)
            {
                Console.Write("\nInforme a nova Razão Social: ");
                razaoSocial = Console.ReadLine().ToUpper();

                sql = $"UPDATE CompanhiaAerea SET RazaoSocial = '{razaoSocial}' WHERE CNPJ = '{cnpj}'";

                BancoAeroporto.UpdateDados(sql, conexao);
                return;
            }


            if (opcao == 2)
            {
                do
                {
                    Console.Write("\nInforme a nova Data de abertura do CNPJ (dd/mm/aaaa): ");

                    try
                    {
                        dataAbertura = DateTime.Parse(Console.ReadLine());
                        condicaoDeSaida = false;
                    }

                    catch (Exception)
                    {
                        Console.WriteLine("\nData informado deve seguir o formato informado: (dd/mm/aa)\n");
                        condicaoDeSaida = true;
                    }

                    if (dataAbertura > DateTime.Now)
                    {
                        if (!condicaoDeSaida)
                        {
                            Console.WriteLine("\nData de abertura não pode ser no futuro!\n");
                        }
                    }

                } while (condicaoDeSaida);

                sql = $"UPDATE CompanhiaAerea SET DataAbertura = '{dataAbertura}' WHERE CNPJ = '{cnpj}'";

                BancoAeroporto.UpdateDados(sql, conexao);

                return;
            }

            sql = $"SELECT Situacao FROM CompanhiaAerea WHERE CNPJ = '{cnpj}'";

            parametro = "Situacao";

            retorno = BancoAeroporto.RetornoDados(sql, conexao, parametro);

            if (retorno == "ATIVA")
            {
                Console.WriteLine("\nSituação desta Companhia está atualmente ATIVA!\nDeseja alterar a situação desta Companhia para INATIVA?");
                Console.Write("\n1 - Sim\n2 - Não\n\n");
                do
                {
                    Console.Write("Opção: ");
                    try
                    {
                        opcao = int.Parse(Console.ReadLine());
                        condicaoDeSaida = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nParametro de entrada inválido!\n");
                        condicaoDeSaida = true;
                    }
                    if (opcao < 1 || opcao > 2)
                    {
                        if (!condicaoDeSaida)
                        {
                            Console.WriteLine("\nOpção inválida!\n");
                        }
                    }
                } while (condicaoDeSaida);


                if (opcao == 1)
                {
                    sql = $"UPDATE CompanhiaAerea SET Situacao = 'INATIVA' WHERE CNPJ = '{cnpj}'";

                    if (BancoAeroporto.UpdateDados(sql, conexao))
                    {
                        Mensagem.TrueAlteradoMessage();
                        return;
                    }

                    Mensagem.FalseAlteradoMessage();
                    return;
                }
            }

            Console.WriteLine("\nSituação desta Companhia está atualmente INATIVA!\nDeseja alterar a situação desta Companhia para ATIVA?");
            Console.Write("\n1 - Sim\n2 - Não\n\nOpção: ");
            opcao = int.Parse(Console.ReadLine());

            if (opcao == 1)
            {
                sql = $"UPDATE CompanhiaAerea SET Situacao = 'ATIVA' WHERE CNPJ = '{cnpj}'";

                if (BancoAeroporto.UpdateDados(sql, conexao))
                {
                    Mensagem.TrueAlteradoMessage();
                    return;
                }

                Mensagem.FalseAlteradoMessage();
                return;
            }

            Console.WriteLine("\nAté logo!");
        }

        public static void ImprimirCompanhia()
        {            
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            SqlCommand cmd;
            int opcao = 0;
            bool validacao;
            string cnpj, sql;

            Console.Clear();

            Console.WriteLine("Ola,");
            Console.WriteLine("\nEscolha a opção desejada:\n");
            Console.WriteLine("1 - Ver todas companhias cadastradas");
            Console.WriteLine("2 - Ver uma especifica");
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
                    Console.WriteLine("\nParametro de dado inválido!\n");
                    validacao = true;
                }

                if (opcao < 1 || opcao > 2 && opcao != 9)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nOpção informada é inválida!");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                conexao.Open();

                cmd = new("SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ, companhiaaerea.DataAbertura, companhiaaerea.UltimoVoo, " +
                "companhiaaerea.DataCadastro, companhiaaerea.Situacao FROM CompanhiaAerea;", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("COMPANHIA\n");
                        Console.WriteLine($"RazaoSocial: {reader.GetString(0)}");
                        Console.WriteLine($"CNPJ: {reader.GetString(1)}");
                        Console.WriteLine($"Data Abertura: {reader.GetDateTime(2).ToShortDateString()}");
                        Console.WriteLine($"Último Voo: {reader.GetDateTime(3).ToShortDateString()}");
                        Console.WriteLine($"Data Cadastro: {reader.GetDateTime(4).ToShortDateString()}");
                        Console.WriteLine($"Situação:{reader.GetString(5)}");
                        Console.WriteLine();
                    }
                }

                Console.Write("Pressione enter para continuar!");
                conexao.Close();

                return;
            }

            Console.Clear();

            Console.Write("\nInforme qual o CNPJ da companhia que deseja localizar: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);

            sql = $"SELECT * FROM CompanhiaAerea WHERE CNPJ = '{cnpj}'";
              
            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nCNPJ informado não está cadastrado em nosso banco de dados!");
                Console.Write("Pressione enter apra continuar!");
                return;
            }

            conexao.Open();

            cmd = new("SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ, companhiaaerea.DataAbertura, companhiaaerea.UltimoVoo, " +
                     $"companhiaaerea.DataCadastro, companhiaaerea.Situacao FROM CompanhiaAerea WHERE companhiaaerea.CNPJ = '{cnpj}'", conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("COMPANHIA\n");
                    Console.WriteLine($"RazaoSocial: {reader.GetString(0)}");
                    Console.WriteLine($"CNPJ: {reader.GetString(1)}");
                    Console.WriteLine($"Data Abertura: {reader.GetDateTime(2).ToShortDateString()}");
                    Console.WriteLine($"Último Voo: {reader.GetDateTime(3).ToShortDateString()}");
                    Console.WriteLine($"Data Cadastro: {reader.GetDateTime(4).ToShortDateString()}");
                    Console.WriteLine($"Situação:{reader.GetString(5)}");
                    Console.WriteLine();
                }
            }

            conexao.Close();

            Console.Write("Pressione enter para continuar!");
            return;
        }
        public static bool ValidarCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma, resto;
            string digito, tempCnpj;

            //limpa caracteres especiais e deixa em minusculo
            cnpj = cnpj.ToLower().Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
            cnpj = cnpj.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
            cnpj = cnpj.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
            cnpj = cnpj.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
            cnpj = cnpj.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
            cnpj = cnpj.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
            cnpj = cnpj.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");

            // Se vazio
            if (cnpj.Length == 0)
                return false;

            //Se o tamanho for < 14 então retorna como falso
            if (cnpj.Length != 14)
                return false;

            // Caso coloque todos os numeros iguais
            switch (cnpj)
            {

                case "00000000000000":

                    return false;

                case "11111111111111":

                    return false;

                case "22222222222222":

                    return false;

                case "33333333333333":

                    return false;

                case "44444444444444":

                    return false;

                case "55555555555555":

                    return false;

                case "66666666666666":

                    return false;

                case "77777777777777":

                    return false;

                case "88888888888888":

                    return false;

                case "99999999999999":

                    return false;
            }

            tempCnpj = cnpj.Substring(0, 12);

            //cnpj é gerado a partir de uma função matemática, logo para validar, sempre irá utilizar esse calculo 
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);

        }
        public static void AcessarCompanhia()
        {            
            int opcao = 0;
            bool condicaoDeParada;


            do
            {
                Console.Clear();

                Console.WriteLine("OPÇÃO: ACESSAR COMPANHIAS\n");

                Console.WriteLine("1 - Cadastrar Companhia Aerea");
                Console.WriteLine("2 - Editar Companhia Aerea");
                Console.WriteLine("3 - Imprimir Companhia Aerea");
                Console.WriteLine("\n9 - Voltar ao menu anterior");
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    condicaoDeParada = false;
                }

                catch (Exception)
                {
                    Mensagem.ParametroMessage();                    
                    Console.ReadKey();
                    condicaoDeParada = true;
                }

                if (opcao < 1 || opcao > 3 && opcao != 9)
                {
                    if (!condicaoDeParada)
                    {
                        Mensagem.OpcaoMessage();                        
                        Console.ReadKey();                        
                    }
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarCompanhia();
                        Console.ReadKey();
                        break;

                    case 2:
                        EditarCompanhia();
                        Console.ReadKey();
                        break;

                    case 3:
                        ImprimirCompanhia();
                        Console.ReadKey();
                        break;

                    case 9:
                        Console.WriteLine("Até");
                        break;
                }

            } while (opcao != 9);
        }
    }
}
