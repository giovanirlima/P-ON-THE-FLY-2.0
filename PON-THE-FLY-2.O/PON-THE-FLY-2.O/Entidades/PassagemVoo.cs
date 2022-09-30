using System;
using System.Data.SqlClient;

namespace PON_THE_FLY_2.O.Entidades
{
    internal class PassagemVoo
    {
        public static void CadastrarPassagem()
        {           
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string sql, inscricao, parametro;
            int retorno, idvoo;
            bool validacao;
            double valor = -1;

            Console.Clear();

            Console.WriteLine("PAINEL DE CADASTRO\n");

            Console.Write("Informe o ID do Voo: ");
            try
            {
                idvoo = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Voo WHERE IDVOO = '{idvoo}'";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nVoo não cadastrado!");
                return;
            }

            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Voo WHERE IDVOO = '{idvoo}'";

            if (BancoAeroporto.RetornoDados(sql, conexao, parametro) == "INATIVO")
            {
                Console.Write("\nNão é possivel cadastrar Passagem, Voo está INATIVO!");
                return;
            }

            Console.Write("informe a inscrição da Aeronave: ");
            try
            {
                inscricao = Console.ReadLine().ToUpper();
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nAeronave não cadastrada!");
                return;
            }

            sql = $"SELECT * FROM AeronavePossueVoo WHERE INSCRICAO = '{inscricao}' AND IDVOO = '{idvoo}'";

            if (BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nEste Voo está cadastrado!!");
                return;
            }

            do
            {
                Console.Write("Digite o valor das passagens deste voo: R$ ");
                try
                {
                    valor = double.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Mensagem.ParametroMessage();
                    validacao = true;
                }

                if (valor >= 10000 || valor < 0)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nValor de Passagem fora do limite! [R$ 9999,99]\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            sql = $"INSERT Passagem(DataUltimaOperacao, ValorPassagem, Situacao, IDVOO) VALUES(@DATAULTIMAOPERACAO, @VALORPASSAGEM, " +
                  $"@SITUACAO, @IDVOO);";

            conexao.Open();
            SqlCommand cmd = new(sql, conexao);

            cmd.Parameters.Add(new SqlParameter("@DATAULTIMAOPERACAO", DateTime.Now.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@VALORPASSAGEM", valor));
            cmd.Parameters.Add(new SqlParameter("@SITUACAO", "ATIVA"));
            cmd.Parameters.Add(new SqlParameter("@IDVOO", idvoo));

            retorno = cmd.ExecuteNonQuery();

            if (retorno > 0)
            {
                retorno = 0;

                sql = $"INSERT Venda VALUES('{DateTime.Now.ToShortDateString()}', '0')";

                if (BancoAeroporto.InsertDados(sql, conexao))
                {
                    Mensagem.TrueCadastradoMessage();
                    return;
                }
            }
            conexao.Close();
            Mensagem.FalseCadastradoMessage();
        }
        public static void EditarPassagem()
        {
            BancoAeroporto caminho = new();
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string sql, retorno, parametro;
            int idpassagem, opcao = 0, retorno1;
            bool validacao;
            double valor = -1;

            Console.Clear();

            Console.WriteLine("PAINEL EDIÇÃO PASSAGEM\n");

            Console.Write("Informe o ID da passagem: ");
            try
            {
                idpassagem = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nPassagem não cadastrada!");
                return;
            }

            Console.Clear();

            Console.WriteLine("PAINEL DE EDIÇÃO");

            Console.WriteLine("\nInforme qual dado deseja alterar: ");
            Console.WriteLine("\n1 - Valor da passagem");
            Console.WriteLine("2 - Situação da passagem");
            Console.WriteLine("\n9 - Voltar ao meu anterior");
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
                    }
                }

            } while (validacao);


            switch (opcao)
            {
                case 1:
                    do
                    {
                        Console.Write("\nInforme o valor da passagem: R$ ");
                        try
                        {
                            valor = double.Parse(Console.ReadLine());
                            validacao = false;
                        }
                        catch (Exception)
                        {
                            Mensagem.ParametroMessage();
                            validacao = true;
                        }

                        if (valor >= 10000 || valor < 0)
                        {
                            if (!validacao)
                            {
                                Console.Write("\nValor de Passagem fora do limite! [R$ 9999,99]\n");
                                validacao = true;
                            }
                        }
                    } while (validacao);

                    sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}', ValorPassagem = @VALORPASSAGEM " +
                        $"WHERE IDPASSAGEM = '{idpassagem}';";

                    conexao.Open();
                    SqlCommand cmd = new(sql, conexao);

                    cmd.Parameters.Add(new SqlParameter("@VALORPASSAGEM", valor));

                    retorno1 = cmd.ExecuteNonQuery();

                    if (retorno1 > 0)
                    {
                        sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}' WHERE IDPASSAGEM = '{idpassagem}'";
                                                
                        if (BancoAeroporto.UpdateDados(sql, conexao))
                        {
                            Mensagem.TrueCadastradoMessage();
                            return;
                        }

                        Mensagem.FalseCadastradoMessage();
                        break;

                    }                       

                    conexao.Close();
                    Mensagem.FalseCadastradoMessage();
                    break;

                case 2:
                    sql = $"SELECT Situacao FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

                    parametro = "Situacao";
                    retorno = BancoAeroporto.RetornoDados(sql, conexao, parametro);

                    opcao = 0;

                    if (retorno == "ATIVA")
                    {
                        Console.WriteLine("\nSituação desta Passagem está atualmente ATIVA!\nDeseja alterar a situação desta Companhia para INATIVA?");
                        Console.Write("\n1 - Sim\n2 - Não\n\n");
                        Console.Write("Opção: ");
                        opcao = int.Parse(Console.ReadLine());


                        if (opcao == 1)
                        {
                            sql = $"UPDATE Passagem SET Situacao = 'INATIVA' WHERE IDPASSAGEM = '{idpassagem}'";

                            if (BancoAeroporto.UpdateDados(sql, conexao))
                            {
                                sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}' WHERE IDPASSAGEM = '{idpassagem}'";

                                if (BancoAeroporto.UpdateDados(sql, conexao))
                                {
                                    Mensagem.TrueCadastradoMessage();
                                    return;
                                }

                                Mensagem.FalseCadastradoMessage();
                                break;
                            }

                            Mensagem.FalseAlteradoMessage();
                            return;
                        }

                        Console.Write("\nAté logo!");
                        return;
                    }

                    Console.WriteLine("\nSituação desta Passagem está atualmente INATIVA!\nDeseja alterar a situação deste Passagem para ATIVA?");
                    Console.Write("\n1 - Sim\n2 - Não\n\n");

                    do
                    {
                        Console.Write("Opção: ");
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
                        if (opcao < 1 || opcao > 2)
                        {
                            if (!validacao)
                            {
                                Mensagem.OpcaoMessage();
                            }
                        }
                    } while (validacao);

                    if (opcao == 1)
                    {
                        sql = $"UPDATE Passagem SET Situacao = 'ATIVA' WHERE IDPASSAGEM = '{idpassagem}'";

                        if (BancoAeroporto.UpdateDados(sql, conexao))
                        {
                            sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}' WHERE IDPASSAGEM = '{idpassagem}'";

                            if (BancoAeroporto.UpdateDados(sql, conexao))
                            {
                                Mensagem.TrueCadastradoMessage();
                                return;
                            }

                            Mensagem.FalseCadastradoMessage();
                            break;
                        }

                        Mensagem.FalseAlteradoMessage();
                        return;
                    }

                    Console.Write("\nAté logo!");
                    break;
            }
        }
        public static void ImprimirPassagem()
        {
            BancoAeroporto caminho = new();
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            SqlCommand cmd;
            int opcao = 0, idpassagem;
            bool validacao;
            string sql;

            Console.Clear();

            Console.WriteLine("Ola,");
            Console.WriteLine("\nEscolha a opção desejada:\n");
            Console.WriteLine("1 - Ver Passagens cadastradas");
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

                cmd = new("SELECT passagem.IDVOO, passagem.IDPASSAGEM, passagem.ValorPassagem, passagem.DataUltimaOperacao, " +
                    "passagem.Situacao FROM Passagem WHERE passagem.Situacao = 'ATIVA';", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("Passagem\n");
                        Console.WriteLine($"IDVOO: {reader.GetInt32(0)}");
                        Console.WriteLine($"IDPASSAGEM: {reader.GetInt32(1)}");
                        Console.WriteLine($"ValorPassagem: {reader.GetDecimal(2).ToString("F2")}");
                        Console.WriteLine($"Data Ultime Operação: {reader.GetDateTime(3).ToShortDateString()}");
                        Console.WriteLine($"Situacao: {reader.GetString(4)}\n");
                    }
                }

                Console.Write("Pressione enter para continuar!");
                conexao.Close();
                return;
            }

            if (opcao == 9)
            {
                Console.Write("\nAté logo!");
                return;
            }

            Console.Clear();


            Console.Write("Informe qual ID da Passagem que deseja localizar: ");
            try
            {
                idpassagem = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("\nParametro de dado inválido!");
                return;
            }

            sql = $"SELECT * FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";
              
            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nVoo não cadastrado!");
                return;
            }

            conexao.Open();

            cmd = new($"SELECT passagem.IDVOO, passagem.IDPASSAGEM, passagem.ValorPassagem, passagem.DataUltimaOperacao, passagem.Situacao FROM Passagem WHERE passagem.IDPASSAGEM = '{idpassagem}';", conexao);


            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("Passagem\n");
                    Console.WriteLine($"IDVOO: {reader.GetInt32(0)}");
                    Console.WriteLine($"IDPASSAGEM: {reader.GetInt32(1)}");
                    Console.WriteLine($"ValorPassagem: {reader.GetDecimal(2).ToString("F2")}");
                    Console.WriteLine($"Data Ultime Operação: {reader.GetDateTime(3).ToShortDateString()}");
                    Console.WriteLine($"Situacao: {reader.GetString(4)}\n");
                }
            }

            Console.Write("Pressione enter para continuar!");
            conexao.Close();
            return;
        }
        public static void AcessarPassagem()
        {
            int opcao = 0;
            bool condicaoDeParada;
            Mensagem Mensagem = new();

            do
            {
                Console.Clear();

                Console.WriteLine("PAINEL ACESSAR PASSAGEM\n");

                Console.WriteLine("1 - Cadastrar Passagem");
                Console.WriteLine("2 - Editar Passagem");
                Console.WriteLine("3 - Imprimir Passagens");
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
                        condicaoDeParada = true;
                    }
                }

            } while (condicaoDeParada);

            switch (opcao)
            {
                case 1:
                    CadastrarPassagem();
                    Console.ReadKey();
                    break;

                case 2:
                    EditarPassagem();
                    Console.ReadKey();
                    break;

                case 3:
                    ImprimirPassagem();
                    Console.ReadKey();
                    break;
            }
        }
    }
}






