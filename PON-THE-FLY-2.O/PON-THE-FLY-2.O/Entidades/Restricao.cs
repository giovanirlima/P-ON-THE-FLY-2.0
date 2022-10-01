using System;
using System.Data.SqlClient;
using POnTheFly;

namespace PON_THE_FLY_2.O.Entidades
{
    internal class Restricao
    {
        public static void CadastrarCPFRestrito()
        {
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string cpf, sql;

            Console.Clear();

            Console.WriteLine("TELA DE BLOQUEIO\n");

            Console.Write("Informe o CPF Restrito: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            if (!Passageiro.ReadCPF(cpf))
            {
                Console.Write("\nCPF Inválido!");
                return;
            }

            sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCPF informado já está cadastrado como restrito!");
                return;
            }

            sql = $"INSERT RestritosCPF VALUES('{cpf}')";

            if (BancoAeroporto.InsertDados(sql, conexao))
            {
                Mensagem.TrueCadastradoMessage();
                return;
            }

            Mensagem.FalseCadastradoMessage();
        }
        public static void CadastrarCNPJRestrito()
        {
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string cnpj, sql;

            Console.Clear();

            Console.WriteLine("TELA DE BLOQUEIO\n");

            Console.Write("Informe o CNPJ Restrito: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            if (!CompanhiaAerea.ValidarCnpj(cnpj))
            {
                Console.Write("\nCNPJ Inválido!");
                return;
            }

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCNPJ informado já está cadastrado como restrito!");
                return;
            }

            sql = $"INSERT RestritosCNPJ VALUES('{cnpj}')";

            if (BancoAeroporto.InsertDados(sql, conexao))
            {
                Mensagem.TrueCadastradoMessage();
                return;
            }

            Mensagem.FalseCadastradoMessage();
        }
        public static void ExcluirCPFRestrito()
        {
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string cpf, sql;

            Console.Clear();

            Console.WriteLine("TELA DE EXCLUSAO\n");

            Console.Write("Informe o CPF Restrito: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCPF informado não está cadastrado como restrito!");
                return;
            }

            sql = $"DELETE FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (BancoAeroporto.DeleteDados(sql, conexao))
            {
                Console.Write("\nExcluido com sucesso!");
                return;
            }

            Console.Write("\nFalha na exclusão!");
            return;

        }
        public static void ExcluirCNPJRestrito()
        {
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string cnpj, sql;

            Console.Clear();

            Console.WriteLine("TELA DE EXCLUSAO\n");

            Console.Write("Informe o CNPJ Restrito: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCNPJ informado não está cadastrado como restrito!");
                return;
            }

            sql = $"DELETE FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (BancoAeroporto.DeleteDados(sql, conexao))
            {
                Console.Write("\nExcluido com sucesso!");
                return;
            }

            Console.Write("\nFalha na exclusão!");
            return;

        }
        public static void ImprimirRestritos()
        {
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            SqlCommand cmd;
            string cpf, cnpj, sql;
            int opcao;
            Console.Clear();

            Console.WriteLine("PAINEL DE RESTRITOS");

            Console.WriteLine("\nEscolha uma das opções abaixo:\n");
            Console.WriteLine("1 - Ver CPF Restritos");
            Console.WriteLine("2 - Ver CPF Restrito especifico");
            Console.WriteLine("3 - Ver CNPJ Restritos");
            Console.WriteLine("4 - Ver CNPJ Restrito especifico");
            Console.WriteLine("\n9 - Voltar ao menu anterior");
            Console.Write("\nOpção:");
            try
            {
                opcao = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }
            if (opcao <1 && opcao > 4 && opcao != 9)
            {
                Mensagem.OpcaoMessage();
                return;
            }

            if (opcao == 1)
            {
                Console.Clear();

                conexao.Open();

                sql = $"SELECT * FROM RestritosCPF";

                cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("CPF RESTRITO\n");
                            Console.WriteLine($"CPF: {reader.GetString(0)}\n");
                        }
                    }
                }

                conexao.Close();
                Console.Write("Pressine enter para retornar!");
                Console.ReadKey();                
                return;
            }

            if (opcao == 2)
            {
                Console.Clear();

                Console.Write("Informe o CPF que deseja localizar: ");
                cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);
                
                Console.Clear();
                
                conexao.Open();

                sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

                cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("CPF RESTRITO\n");
                            Console.WriteLine($"CPF: {reader.GetString(0)}\n");
                        }
                    }
                }

                conexao.Close();
                Console.Write("Pressine enter para retornar!");
                Console.ReadKey();
                return;
            }

            if (opcao == 3)
            {
                Console.Clear();

                conexao.Open();

                sql = $"SELECT * FROM RestritosCNPJ";

                cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("CNPJ RESTRITO\n");
                            Console.WriteLine($"CNPJ: {reader.GetString(0)}\n");
                        }
                    }
                }

                conexao.Close();

                Console.Write("Pressine enter para retornar!");
                Console.ReadKey();
                return;
            }

            if(opcao == 9)
            {
                return;
            }

            Console.Clear();

            Console.Write("Informe o CNPJ que deseja localizar: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            Console.Clear();

            conexao.Open();

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            cmd = new(sql, conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("CPF RESTRITO\n");
                        Console.WriteLine($"CPF: {reader.GetString(0)}\n");
                    }
                }
            }

            conexao.Close();

            Console.Write("Pressine enter para retornar!");
            Console.ReadKey();
            return;
        }
        public static void AcessarRestritos()
        {
            int opcao = -1;
            bool validacao;

            do
            {
                Console.Clear();

                Console.WriteLine("ACESSAR RESTRITOS\n");

                Console.WriteLine("1 - Cadastrar CPF");
                Console.WriteLine("2 - Cadastrar CNPJ");
                Console.WriteLine("3 - Excluir CPF");
                Console.WriteLine("4 - Excluir CNPJ");
                Console.WriteLine("5 - Imprimir Restritos");
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

                if (opcao < 1 || opcao > 5 && opcao != 9)
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
                        CadastrarCPFRestrito();
                        Console.ReadKey();
                        break;

                    case 2:
                        CadastrarCNPJRestrito();
                        Console.ReadKey();
                        break;

                    case 3:
                        ExcluirCPFRestrito();
                        Console.ReadKey();
                        break;

                    case 4:
                        ExcluirCNPJRestrito();
                        Console.ReadKey();
                        break;

                    case 5:
                        ImprimirRestritos();                        
                        break;
                }

            } while (opcao != 9);
        }
    }
}
