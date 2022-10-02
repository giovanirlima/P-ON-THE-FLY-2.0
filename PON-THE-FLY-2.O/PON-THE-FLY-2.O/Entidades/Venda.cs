using System;
using System.Data;
using System.Data.SqlClient;

namespace PON_THE_FLY_2.O.Entidades
{
    internal class Venda
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
        public static void CadastrarVenda()
        {
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            string sql, parametro, cpf, inscricao, idpassagem, idvoo;
            double valorPassagem, resultado, valortotalv;
            int quantidade = 0, acentosOcupados, capacidade, idvenda;
            DateTime idade;
            bool validacao;

            Console.Clear();

            Console.WriteLine("COMPRAR PASSAGEM\n");

            Console.Write("Digite o CPF do Passageiro: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            if (!ReadCPF(cpf))
            {
                Console.Write("\nCPF digitador é inválido!");
                return;
            }

            sql = $"SELECT * FROM Passageiro WHERE CPF = '{cpf}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCPF do Passageiro não cadastrado!");
                return;
            }

            sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nPassageiro com CPF restrito, não possivel realizar a venda!");
                return;
            }

            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Passageiro WHERE CPF = '{cpf}';";

            if (BancoAeroporto.RetornoDados(sql, conexao, parametro) == "INATIVO")
            {
                Console.Write("\nPassageiro com cadastro INATIVO, não é possivel efetuar venda!");
                return;
            }

            sql = $"SELECT Nascimento FROM Passageiro WHERE CPF = '{cpf}';";

            parametro = "Nascimento";

            idade = DateTime.Parse(BancoAeroporto.RetornoDados(sql, conexao, parametro));

            if ((DateTime.Now.Year - idade.Year) <= 18)
            {
                if ((DateTime.Now.Year - idade.Year) == 18)
                {
                    if (DateTime.Now.Month < idade.Month)
                    {
                        Console.Write("\nPassageiros menor de idade não poder comprar passagens!");
                        return;
                    }
                }
                else
                {
                    Console.Write("\nPassageiros menor de idade não poder comprar passagens!");
                    return;
                }
            }

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCPF do Passageiro não cadastrado!");
                return;
            }

            Console.Write("\nInforme o ID da passagem que deseja comprar: ");
            try
            {
                idpassagem = Console.ReadLine().ToUpper();
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.Write("\nPassagem não cadastrada para compra!");
                return;
            }

            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

            if (BancoAeroporto.RetornoDados(sql, conexao, parametro) == "INATIVA")
            {
                Console.Write("\nPassagem INATIVA, não é possivel efetuar venda!");
                return;
            }

            parametro = "ValorPassagem";

            sql = $"SELECT ValorPassagem FROM Passagem WHERE IDPASSAGEM = '{idpassagem}'";

            valorPassagem = Convert.ToDouble(BancoAeroporto.RetornoDados(sql, conexao, parametro));

            Console.WriteLine($"\n[Preço da passagem R$ {valorPassagem:F2}]\n");
            do
            {
                Console.Write("Limitado a 4 Passagens por venda, deseja comprar quantas: ");
                try
                {
                    quantidade = int.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Mensagem.ParametroMessage();
                    validacao = true;
                }
                if (quantidade < 1 || quantidade > 4)
                {
                    if (!validacao)
                    {
                        Mensagem.OpcaoMessage();
                        validacao = true;
                    }
                }
            } while (validacao);

            resultado = valorPassagem * quantidade;

            parametro = "AssentosOcupados";

            sql = "SELECT AssentosOcupados FROM AeronavePossueVoo " +
                  "JOIN Passagem ON passagem.IDVOO = aeronavepossuevoo.IDVOO  " +
                 $"WHERE passagem.IDPASSAGEM = '{idpassagem}';";

            acentosOcupados = Convert.ToInt32(BancoAeroporto.RetornoDados(sql, conexao, parametro));

            parametro = "Capacidade";

            sql = "SELECT Capacidade FROM AeronavePossueVoo " +
                  "JOIN Passagem ON passagem.IDVOO = aeronavepossuevoo.IDVOO  " +
                 $"WHERE passagem.IDPASSAGEM = '{idpassagem}';";


            capacidade = Convert.ToInt32(BancoAeroporto.RetornoDados(sql, conexao, parametro));

            parametro = "INSCRICAO";

            sql = "SELECT INSCRICAO FROM AeronavePossueVoo " +
                  "JOIN Passagem ON passagem.IDVOO = aeronavepossuevoo.IDVOO  " +
                 $"WHERE passagem.IDPASSAGEM = '{idpassagem}';";

            inscricao = BancoAeroporto.RetornoDados(sql, conexao, parametro);

            if ((acentosOcupados + quantidade) > capacidade)
            {
                Console.Write($"\nFalha na transação, possuimos um total de {capacidade - acentosOcupados} disponiveis");
                return;
            }

            sql = $"UPDATE Passageiro SET UltimaCompra = '{DateTime.Now.ToShortDateString()}' WHERE CPF = '{cpf}';";

            BancoAeroporto.UpdateDados(sql, conexao);

            parametro = "Identificador";

            sql = $"SELECT Identificador FROM Passagem WHERE IDPASSAGEM = '{idpassagem}'";

            idvenda = Convert.ToInt32(BancoAeroporto.RetornoDados(sql, conexao, parametro));

            parametro = "ValorTotalVendas";

            sql = $"SELECT ValorTotalVendas FROM Venda WHERE IDVENDA = '{idvenda}';";

            valortotalv = Convert.ToDouble(BancoAeroporto.RetornoDados(sql, conexao, parametro));

            sql = $"UPDATE Venda SET DataVenda = '{DateTime.Now.ToShortDateString()}', ValorTotalVendas = '{valortotalv + resultado}' WHERE IDVENDA = '{idvenda}'";

            BancoAeroporto.UpdateDados(sql, conexao);

            for (int i = 0; i < quantidade; i++)
            {
                sql = $"INSERT VendaPassagem(IDVENDA, IDPASSAGEM, ValorUnitarioAtual) VALUES('{idvenda}', '{idpassagem}', {valorPassagem});";

                validacao = BancoAeroporto.InsertDados(sql, conexao);
            }

            if (validacao)
            {
                parametro = "IDVOO";

                sql = $"SELECT IDVOO FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

                idvoo = BancoAeroporto.RetornoDados(sql, conexao, parametro);

                sql = $"UPDATE AeronavePossueVoo SET AssentosOcupados = '{quantidade + acentosOcupados}' WHERE INSCRICAO = '{inscricao}' AND IDVOO = '{idvoo}';";

                if (BancoAeroporto.UpdateDados(sql, conexao))
                {
                    Mensagem.TrueCompraMessage();
                    return;
                }
            }

            Mensagem.FalseCompraMessage();
        }
        public static void ImprimirPassagem()
        {
            BancoAeroporto caminho = new();
            SqlConnection conexao = new(BancoAeroporto.CaminhoDeConexao());
            SqlCommand cmd;
            int opcao = 0, iditem, idvenda;
            bool validacao;
            string sql, idpassagem;

            Console.Clear();

            Console.WriteLine("PAINEL DE IMPRESSÃO\n");
            Console.WriteLine("Escolha a opção desejada:\n");
            Console.WriteLine("1 - Ver Passagens Vendidas");
            Console.WriteLine("2 - Ver uma especifica");
            Console.WriteLine("3 - Ver venda Total");
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

                if (opcao < 1 || opcao > 3 && opcao != 9)
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

                cmd = new("SELECT aeronavepossuevoo.INSCRICAO, passagem.IDVOO, passagem.IDPASSAGEM, vendapassagem.IDITEM, " +
                          "passagem.ValorPassagem,  passagem.Situacao FROM VendaPassagem " +
                          "JOIN Passagem ON passagem.IDPASSAGEM = vendapassagem.IDPASSAGEM " +
                          "JOIN AeronavePossueVoo ON aeronavepossuevoo.IDVOO = passagem.IDVOO WHERE passagem.Situacao = 'ATIVA';", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("PASSAGEM VENDIDAS\n");
                        Console.WriteLine($"INSCRICAO: {reader.GetString(0)}");
                        Console.WriteLine($"ID PASSAGEM: {reader.GetString(1)}");
                        Console.WriteLine($"ID VOO: {reader.GetString(2)}");
                        Console.WriteLine($"ID ITEM: {reader.GetInt32(3)}");
                        Console.WriteLine($"Valor Pago: R$ {reader.GetDecimal(4)}");
                        Console.WriteLine($"Situação: {reader.GetString(5)}\n");
                    }
                }

                Console.Write("\nPressione enter para continuar!");
                conexao.Close();
                return;
            }

            if (opcao == 9)
            {
                return;
            }

            if (opcao == 3)
            {
                Console.Clear();

                Console.WriteLine("PAINEL DE IMPRESSÃO");

                Console.Write("\nInforme qual ID da Venda que deseja localizar: ");
                try
                {
                    idvenda = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("\nParametro de dado inválido!");
                    return;
                }

                sql = $"SELECT * FROM Venda WHERE IDVENDA = '{idvenda}';";

                if (!BancoAeroporto.LocalizarDados(sql, conexao))
                {
                    Console.WriteLine("\nVenda não cadastrado!");
                    return;
                }

                conexao.Open();

                cmd = new($"SELECT * FROM Venda WHERE IDVENDA = '{idvenda}'", conexao);


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("VALOR TOTAL\n");
                        Console.WriteLine($"ID VENDA: {reader.GetInt32(0)}");
                        Console.WriteLine($"Data Venda: {reader.GetDateTime(1).ToShortDateString()}");
                        Console.WriteLine($"Valor Total: R$ {reader.GetDecimal(2):F2}");
                    }
                }

                Console.Write("\nPressione enter para continuar!");
                conexao.Close();
                return;
            }
            Console.Clear();

            Console.WriteLine("PAINEL DE IMPRESSÃO");

            Console.Write("\nInforme qual ID da Passagem que deseja localizar: ");
            try
            {
                idpassagem = Console.ReadLine();
            }
            catch (Exception)
            {
                Mensagem.ParametroMessage();
                return;
            }

            sql = $"SELECT * FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nVoo não cadastrado!");
                return;
            }

            Console.Write("\nInforme qual ID do item que deseja localizar: ");
            try
            {
                iditem = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("\nParametro de dado inválido!");
                return;
            }

            sql = $"SELECT * FROM VendaPassagem WHERE IDPASSAGEM = '{idpassagem}' AND IDITEM = '{iditem}';";

            if (!BancoAeroporto.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nPassagem não cadastrada ou ID inválido!");
                return;
            }


            conexao.Open();

            cmd = new("SELECT aeronavepossuevoo.INSCRICAO, passagem.IDVOO, passagem.IDPASSAGEM, vendapassagem.IDITEM, " +
                         "passagem.ValorPassagem,  passagem. Situacao FROM VendaPassagem " +
                         "JOIN Passagem ON passagem.IDPASSAGEM = vendapassagem.IDPASSAGEM " +
                         "JOIN AeronavePossueVoo ON aeronavepossuevoo.IDVOO = passagem.IDVOO " +
                        $"WHERE passagem.IDPASSAGEM = '{idpassagem}' AND vendapassagem.IDITEM = '{iditem}' ", conexao);


            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("PASSAGEM VENDIDAS\n");
                    Console.WriteLine($"INSCRICAO: {reader.GetString(0)}");
                    Console.WriteLine($"ID PASSAGEM: {reader.GetString(1)}");
                    Console.WriteLine($"ID VOO: {reader.GetString(2)}");
                    Console.WriteLine($"ID ITEM: {reader.GetInt32(3)}");
                    Console.WriteLine($"Valor Pago: R$ {reader.GetDecimal(4)}");
                    Console.WriteLine($"Situação: {reader.GetString(5)}\n");
                }
            }

            Console.Write("\nPressione enter para continuar!");
            conexao.Close();
            return;
        }
        public static void AcessarVenda()
        {
            int opcao = 0;
            bool condicaoDeParada;

            do
            {
                Console.Clear();

                Console.WriteLine("PAINEL ACESSAR VENDAS\n");

                Console.WriteLine("1 - Cadastrar Venda");
                Console.WriteLine("2 - Imprimir Venda");
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
                        CadastrarVenda();
                        Console.ReadKey();
                        break;

                    case 2:
                        ImprimirPassagem();
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