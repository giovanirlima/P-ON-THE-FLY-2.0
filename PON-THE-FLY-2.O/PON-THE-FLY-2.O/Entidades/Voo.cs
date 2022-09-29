using System;
using System.Data;
using System.Data.SqlClient;

namespace PON_THE_FLY_2.O.Entidades
{
    public class Voo
    {
        public int IDVoo { get; set; }
        public string Destino { get; set; }
        public string InscricaoAeronave { get; set; }
        public DateTime DataVoo { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }

        public Voo()
        {
        }

        public static void CadastrarVoo()
        {            
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string sql, inscricao;
            int idvoo = 0, capacidade = 0;
            Destino dest = new();
            DateTime dataVoo = DateTime.Now;
            bool condicaoDeSaida;

            Console.Clear();

            Console.Write("Informe a inscrição da Aeronave que será responsavél pelo voo: ");
            inscricao = Console.ReadLine().ToUpper();

            sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{inscricao}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nAeronave não localizada!");
                return;
            }

            conexao.Open();

            sql = $"SELECT IDVOO FROM AeronavePossueVoo WHERE INSCRICAO = '{inscricao}'";

            SqlCommand cmd = new(sql, conexao)
            {
                CommandType = CommandType.Text
            };

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idvoo = Convert.ToInt32(reader["IDVOO"].ToString());
                    }
                }
            }

            conexao.Close();

            if (idvoo > 9999)
            {
                Console.WriteLine("\nNúmero máximo de voo cadastrados");
                return;
            }

            Console.Write("Informe a IATA do destino de voo: ");
            string destino = Console.ReadLine().ToUpper();

            if (!dest.ListaDestino(destino))
            {
                Console.WriteLine("IATA informado não cadastrada!");
                return;
            }

            do
            {
                Console.Write("Infome a data do voo: ");

                try
                {
                    dataVoo = DateTime.Parse(Console.ReadLine());
                    condicaoDeSaida = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("\nData informada deve seguir o formato apresentado: [dd/mm/aaaa] [hh:mm]\n");
                    condicaoDeSaida = true;
                }

                if (dataVoo <= DateTime.Now)
                {
                    if (!condicaoDeSaida)
                    {
                        Console.WriteLine("\nData do voo não pode ser para o dia nem posterior!\n");
                        condicaoDeSaida = true;
                    }
                }

            } while (condicaoDeSaida);

            sql = $"INSERT Voo VALUES('{destino}', '{dataVoo}', '{DateTime.Now.ToShortDateString()}', 'ATIVO');";
                     
            if (BancoAeroporto.InsertDados(sql, conexao))
            {               
                conexao.Open();

                sql = $"SELECT Capacidade FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

                cmd = new(sql, conexao)
                {
                    CommandType = CommandType.Text
                };

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            capacidade = Convert.ToInt32(reader["Capacidade"].ToString());
                        }
                    }
                }

                conexao.Close();

                sql = $"INSERT AeronavePossueVoo(INSCRICAO, IDVOO, Capacidade, AcentosOcupados) VALUES('{inscricao}', '{idvoo + 1}', '{capacidade}', 0);";

                if (BancoAeroporto.InsertDados(sql, conexao))
                {
                    Mensagem.TrueCadastradoMessage();
                    return;
                }
            }

            Console.Write("\nFalha ao cadastrar Voo!");
        }
        public static void EditarVoo()
        {            
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            Destino dest = new();
            string sql, inscricao, parametro, retorno;
            DateTime data = new();
            bool validacao;
            int idvoo, op = 0, capacidade, ocupados;

            Console.Clear();

            Console.Write("Informe o ID do voo que deseja editar: ");
            try
            {
                idvoo = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.Write("\nParametro de entrada inválido!");
                return;
            }

            sql = $"SELECT * FROM Voo WHERE IDVOO = '{idvoo}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nVoo não cadastrado!");
                return;
            }

            Console.Clear();

            Console.WriteLine("Escolha a opção desejada");
            Console.WriteLine("\n1 - Editar IATA");
            Console.WriteLine("2 - Editar inscrição");
            Console.WriteLine("3 - Editar data do Voo");
            Console.WriteLine("4 - Editar Situação");
            Console.WriteLine("9 - Voltar ao menu anterior");
            do
            {
                Console.Write("\nOpção: ");
                try
                {
                    op = int.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nParametro de entrada inválido!");
                    validacao = true;
                }

                if (op < 1 || op > 4 && op != 9)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nOpção escolhida é inválido!");
                        validacao = true;
                    }
                }
            } while (validacao);

            if (op == 1)
            {
                Console.Write("\nInforme a IATA destino do novo destino: ");
                string destino = Console.ReadLine().ToUpper();

                validacao = dest.ListaDestino(destino);

                if (!validacao)
                {
                    Console.Write("\nNovo destino não cadastrado!");
                    return;
                }

                sql = $"UPDATE Voo SET Destino = '{destino}' WHERE IDVOO = '{idvoo}';";

                BancoAeroporto.UpdateDados(sql, conexao);



                return;
            }

            if (op == 2)
            {
                Console.Write("\nInforme a inscrição da nova Aeronave: ");
                inscricao = Console.ReadLine().ToUpper();

                sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

                if (!BancoAeroporto.LocalizarDados(sql, conexao))
                {
                    Console.Write("\nAeronave informada não possue cadastro!");
                    return;
                }

                sql = $"SELECT * FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}'";

                parametro = "INSCRICAO";

                retorno = BancoAeroporto.RetornoDados(sql, conexao, parametro);

                if (retorno == inscricao)
                {
                    Console.Write("\nIATA informada é a mesma do Voo!");
                    return;
                }

                sql = $"SELECT * FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}' AND INSCRICAO = '{retorno}';";

                parametro = "Capacidade";

                capacidade = Convert.ToInt32(BancoAeroporto.RetornoDados(sql, conexao, parametro));

                sql = $"SELECT * FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}' AND INSCRICAO = '{retorno}';";

                parametro = "AcentosOcupados";

                ocupados = Convert.ToInt32(BancoAeroporto.RetornoDados(sql, conexao, parametro));

                sql = $"DELETE FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}' AND INSCRICAO = '{retorno}';";

                BancoAeroporto.DeleteDados(sql, conexao);

                sql = $"INSERT AeronavePossueVoo(INSCRICAO, IDVOO, Capacidade, AcentosOcupados) VALUES('{inscricao}', '{idvoo}', '{capacidade}', '{ocupados}');";

                if (BancoAeroporto.InsertDados(sql, conexao))
                {
                    Mensagem.TrueAlteradoMessage();
                    return;
                }

                Mensagem.FalseAlteradoMessage();
                return;
            }

            if (op == 3)
            {
                do
                {
                    Console.Write("\nInforme a nova data do voo: ");
                    try
                    {
                        data = DateTime.Parse(Console.ReadLine());
                        validacao = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nFormato inválido! [dd/mm/yyyy]\n");
                        validacao = true;
                    }

                    if (data < DateTime.Now)
                    {
                        if (!validacao)
                        {
                            Console.WriteLine("\nData deve ser futura!\n");
                        }
                    }
                } while (validacao);

                sql = $"UPDATE Voo SET DataVoo = '{data}' WHERE IDVOO = '{idvoo}';";

                ;

                if (BancoAeroporto.UpdateDados(sql, conexao))
                {
                    Mensagem.TrueAlteradoMessage();
                    return;
                }

                Mensagem.FalseAlteradoMessage();
                return;
            }

            if (op == 4)
            {
                sql = $"SELECT Situacao FROM Voo WHERE IDVOO = '{idvoo}'";

                parametro = "Situacao";
                retorno = BancoAeroporto.RetornoDados(sql, conexao, parametro);

                if (retorno == "ATIVO")
                {
                    Console.WriteLine("\nSituação deste Voo está atualmente ATIVO!\nDeseja alterar a situação desta Companhia para INATIVO?");
                    Console.Write("\n1 - Sim\n2 - Não\n\n");
                    do
                    {
                        Console.Write("Opção: ");
                        try
                        {
                            op = int.Parse(Console.ReadLine());
                            validacao = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\nParametro de entrada inválido!\n");
                            validacao = true;
                        }
                        if (op < 1 || op > 2)
                        {
                            if (!validacao)
                            {
                                Console.WriteLine("\nOpção inválida!\n");
                            }
                        }
                    } while (validacao);


                    if (op == 1)
                    {
                        sql = $"UPDATE Voo SET Situacao = 'INATIVO' WHERE IDVOO = '{idvoo}'";

                        if (BancoAeroporto.UpdateDados(sql, conexao))
                        {
                            Mensagem.TrueAlteradoMessage();
                            return;
                        }

                        Mensagem.FalseAlteradoMessage();
                        return;
                    }

                    Console.Write("\nAté logo!");
                    return;
                }

                Console.WriteLine("\nSituação deste Voo está atualmente INATIVO!\nDeseja alterar a situação deste Voo para ATIVO?");
                Console.Write("\n1 - Sim\n2 - Não\n\nOpção: ");
                op = int.Parse(Console.ReadLine());

                if (op == 1)
                {
                    sql = $"UPDATE Voo SET Situacao = 'ATIVO' WHERE IDVOO = '{idvoo}'";

                    if (BancoAeroporto.UpdateDados(sql, conexao))
                    {
                        Mensagem.TrueAlteradoMessage();
                        return;
                    }

                    Mensagem.FalseAlteradoMessage();
                    return;
                }

                Console.Write("\nAté logo!");
                return;
            }
            Console.Write("\nAté logo!");
        }
        public static void ImprimirVoo()
        {            
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            SqlCommand cmd;            
            int opcao = 0, idvoo;
            bool validacao;
            string sql;

            Console.Clear();

            Console.WriteLine("Ola,");
            Console.WriteLine("\nEscolha a opção desejada:\n");
            Console.WriteLine("1 - Ver todas Passagens cadastradas");
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

                cmd = new("SELECT aeronavepossuevoo.INSCRICAO, aeronavepossuevoo.IDVOO, voo.Destino, voo.DataVoo, " +
                          "voo.DataCadastro, aeronavepossuevoo.AcentosOcupados, voo.Situacao FROM AeronavePossueVoo " +
                          "JOIN Voo ON " +
                          "voo.IDVOO = aeronavepossuevoo.IDVOO", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("Voo\n");
                        Console.WriteLine($"INSCRIÇÃO Aeronave: {reader.GetString(0)}");
                        Console.WriteLine($"IDVOO: {reader.GetInt32(1)}");
                        Console.WriteLine($"Destino: {reader.GetString(2)}");
                        Console.WriteLine($"Data do Voo: {reader.GetDateTime(3).ToShortDateString()}");
                        Console.WriteLine($"Data do Cadastro: {reader.GetDateTime(4).ToShortDateString()}");
                        Console.WriteLine($"Acentos Ocupados: {reader.GetInt32(5)}");
                        Console.WriteLine($"Situacao: {reader.GetString(6)}\n");
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


            Console.Write("Informe qual ID do Voo que deseja localizar: ");
            try
            {
                idvoo = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Voo WHERE IDVOO = '{idvoo}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nVoo não cadastrado!");
                return;
            }

            conexao.Open();

            cmd = new("SELECT aeronavepossuevoo.INSCRICAO, aeronavepossuevoo.IDVOO, voo.Destino, voo.DataVoo, " +
                      "voo.DataCadastro, aeronavepossuevoo.AcentosOcupados, voo.Situacao FROM AeronavePossueVoo " +
                      "JOIN Voo ON " +
                      $"voo.IDVOO = aeronavepossuevoo.IDVOO WHERE voo.IDVOO = '{idvoo}'", conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("Voo\n");
                    Console.WriteLine($"INSCRIÇÃO Aeronave: {reader.GetString(0)}");
                    Console.WriteLine($"IDVOO: {reader.GetInt32(1)}");
                    Console.WriteLine($"Destino: {reader.GetString(2)}");
                    Console.WriteLine($"Data do Voo: {reader.GetDateTime(3).ToShortDateString()}");
                    Console.WriteLine($"Data do Cadastro: {reader.GetDateTime(4).ToShortDateString()}");
                    Console.WriteLine($"Acentos Ocupados: {reader.GetInt32(5)}");
                    Console.WriteLine($"Situacao: {reader.GetString(6)}\n");
                }
            }

            Console.Write("Pressione enter para continuar!");
            conexao.Close();
            return;
        }
        public static void AcessarVoo()
        {
            int opcao = 0;
            bool condicaoDeParada;            

            do
            {
                Console.Clear();

                Console.WriteLine("OPÇÃO: ACESSAR VOO\n");

                Console.WriteLine("1 - Cadastrar Voo");
                Console.WriteLine("2 - Editar Voo");
                Console.WriteLine("3 - Imprimir Voo");
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
                        CadastrarVoo();
                        Console.ReadKey();
                        break;


                    case 2:
                        EditarVoo();
                        Console.ReadKey();
                        break;

                    case 3:
                        ImprimirVoo();
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
