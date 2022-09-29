using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using PON_THE_FLY_2.O.Entidades;

namespace PON_THE_FLY_2.O.Entidades
{
    internal class PassagemVoo
    {
        public string IdPassagem { get; set; }
        public string IdVoo { get; set; }
        public DateTime DataUltimaOperacao { get; set; }
        public string Valor { get; set; }
        public char Situacao { get; set; }
        public PassagemVoo()
        {
        }
        public PassagemVoo(string idPassagem, string idVoo, DateTime dataUltimaOperacao, string valor, char situacao)
        {
            IdPassagem = idPassagem;
            IdVoo = idVoo;
            DataUltimaOperacao = dataUltimaOperacao;
            Valor = valor;
            Situacao = situacao;
        }

        public void CadastrarPassagem(List<Aeronave> listaAeronaves, List<Voo> listaVoo, List<PassagemVoo> listaPassagens)
        {
            Aeronave aeronave = new();
            Voo voo = new();
            bool validacao = false;
            double valor;
            int idPassagem = 1;

            Console.Clear();

            Console.Write("Informe o id do voo: ");
            int idVoo = int.Parse(Console.ReadLine());

            Console.Write("informe a inscrição da Aeronave: ");
            string inscricao = Console.ReadLine().ToUpper();

            foreach (var v in listaVoo)
            {
                if (v.IDVoo == idVoo)
                {
                    validacao = true;
                    voo = v;
                    break;
                }
            }

            foreach (var aero in listaAeronaves)
            {
                if (aero.Inscricao == inscricao)
                {
                    aeronave = aero;
                    break;
                }
            }

            if (!validacao)
            {
                Console.WriteLine("\nID do voo ou inscrição da aeronave incorretos!");
                return;
            }

            do
            {
                Console.Write("Digite o valor das passagens deste voo: R$ ");
                valor = double.Parse(Console.ReadLine());
                validacao = false;

                if (valor > 10000 || valor < 0)
                {
                    Console.WriteLine("\nValor de Passagem fora do limite!\n");
                    validacao = true;
                }

            } while (validacao);

            string stringIdVoo = "" + idVoo;
            string stringIdPassagem = "" + idPassagem;
            string stringValor = "" + valor;

            for (int i = 0; i < int.Parse(aeronave.Capacidade); i++)
            {
                listaPassagens.Add(
                    new PassagemVoo(
                        stringIdPassagem,
                        stringIdVoo,
                        DateTime.Now,
                        stringValor,
                        'L'
                    )); ;
                aeronave.AcentosOcupado = "" + idPassagem;
                stringIdPassagem = "" + idPassagem++;
            }

            if (int.Parse(aeronave.Capacidade) == int.Parse(aeronave.AcentosOcupado))
            {
                Console.WriteLine("\nCadastro de passagens com sucesso!");
            }

            else
            {
                Console.WriteLine("Falha ao criar passagens!");
            }
        }/// funcionando
         /// 
         /// Rodando perfeitamente.
        public void EditarPassagem(List<Voo> listaVoo, List<PassagemVoo> listaPassagem)
        {
            PassagemVoo p = new();

            Console.Clear();

            p.LocalizarPassagem(listaVoo, listaPassagem);

            Console.WriteLine("Informe qual dado deseja alterar: ");
            Console.WriteLine("\n1 - Valor");
            Console.WriteLine("2 - Situação");
            Console.WriteLine("0 - Sair");
            Console.Write("\nOpção: ");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    Console.WriteLine("\nInforme o valor da passagem: R$ ");
                    double valor = double.Parse(Console.ReadLine());

                    if (valor > 9999.99 || valor < 0)
                    {
                        Console.WriteLine("\nValor de Passagem fora do limite!");
                        break;
                    }

                    else
                    {
                        p.Valor = "" + valor;
                        Console.WriteLine("\n >> Pasagem editada com sucesso <<");
                    }
                    break;

                case 2:
                    Console.WriteLine("\nInforme a Situação: ");
                    char situacao = char.Parse(Console.ReadLine());
                    p.Situacao = situacao;
                    Console.WriteLine("\n >> Pasagem editada com sucesso <<");
                    break;
            }
        }/// funcionando
        public PassagemVoo LocalizarPassagem(List<Voo> listaVoo, List<PassagemVoo> listaPassagem)
        {
            bool achei = false;

            PassagemVoo p = new();

            Console.Clear();

            Console.Write("Informe o id do voo: ");
            int idVoo = int.Parse(Console.ReadLine());

            Console.Write("Informe o id da passagem: ");
            string idPassagem = Console.ReadLine();

            foreach (Voo i in listaVoo)
            {
                if (i.IDVoo == idVoo)
                {
                    foreach (var pv in listaPassagem)
                    {
                        if (pv.IdPassagem == idPassagem)
                        {
                            achei = true;
                            p = pv;
                            Console.WriteLine("\n>>>>>>> PASSAGEM LOCALIZADA <<<<<<");
                            Console.WriteLine(p.ToString());
                            return p;
                        }
                    }
                }
            }

            if (achei)
            {
                Console.WriteLine("Passagem não encontrada!");
            }
            return p;
        }/// Rodando perfeitamente.
        public void ImprimirPassagem(List<PassagemVoo> listaPassagem)
        {
            Console.Clear();

            foreach (PassagemVoo p in listaPassagem)
            {

                Console.WriteLine("\n>>>>>>>  PASSAGEM <<<<<<");
                Console.WriteLine(p.ToString());

            }
        }/// funcionando
         /// 
         /// Rodando perfeitamente.
        public void imprimindo_Arquivo()
        {
            string line;
            try
            {
                StreamReader sr = new StreamReader("c:\\Users\\Filipe Anjos\\Documents\\ATIVIDADES_ESTAGIO\\PON_THE_FLY\\PassagemVoo.dat");//Instancia um Objeto StreamReader (Classe de Manipulação de Leitura de Arquivos)
                line = sr.ReadLine(); //Faz a Leitura de uma linha do arquivo e atribui a string line
                while (line != null)// Laço de Repetição para fazer a leitura de linhas do arquivo até o EOF (End Of File - Fim do Arquivo)
                {
                    Console.WriteLine(line);//Imprime o retorno do arquivo no Console
                    line = sr.ReadLine(); //Faz a Leitura de linha do arquivo e atribui a string line
                }
                sr.Close();//Fecha o Arquivo
                Console.WriteLine("Fim da Leitura do Arquivo");
                Console.ReadLine();
            }
            catch (Exception e) // Tratamento de erro na abertura do arquivo
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executando o Bloco de Comando - Sem Erros");
            }
            Console.WriteLine("FIM DA LEITURA");
            Console.ReadKey();
        }
        public bool Ler_Arquivo()
        {
            string line;
            try
            {
                StreamReader sr = new StreamReader("c:\\Users\\Filipe Anjos\\Documents\\ATIVIDADES_ESTAGIO\\PON_THE_FLY\\PassagemVoo.dat");//Instancia um Objeto StreamReader (Classe de Manipulação de Leitura de Arquivos)
                line = sr.ReadLine(); //Faz a Leitura de uma linha do arquivo e atribui a string line
                while (line != null)// Laço de Repetição para fazer a leitura de linhas do arquivo até o EOF (End Of File - Fim do Arquivo)
                {

                    line = sr.ReadLine(); //Faz a Leitura de linha do arquivo e atribui a string line
                    return true;

                }
                sr.Close();//Fecha o Arquivo
                Console.WriteLine("FIM DA LEITURA");
                Console.ReadKey();
            }
            catch // Tratamento de erro na abertura do arquivo
            {
                Console.WriteLine("Arquivo inexistente! - Gerar arquivo");
                return false;
            }
            if (line != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void GravarArquivoPassagem(List<PassagemVoo> listaPassagem)
        {
            bool validacao = true;

            try
            {
                StreamWriter sw = new StreamWriter(@"C:\Users\5BY5\source\repos\PROJETO-ON-THE-FLY\POnTheFly\PassagemVoo.dat");  //Instancia um Objeto StreamWriter (Classe de Manipulação de Arquivos)
                                                                                                                                 //sw.WriteLine("maria;araraquara;190;contato;"); //Exemplo de escrita - formato da escrita será de acordo com a necessidade do projeto
                foreach (var i in listaPassagem)
                {
                    sw.WriteLine(i.getData());
                }
                sw.Close();  // Comando para Fechar o Arquivo
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                validacao = false;
            }

            if (validacao)
            {
                Console.WriteLine("\nArquivo gravado com sucesso!");
            }
        }
        public void CarregarArquivoPassagem(List<PassagemVoo> listaPassagem)
        {
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\5BY5\source\repos\PROJETO-ON-THE-FLY\POnTheFly\PassagemVoo.dat"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //tempo = new DateTime(int.Parse(line.Substring(14, 4)), int.Parse(line.Substring(12, 2)), int.Parse(line.Substring(10, 2)), int.Parse(line.Substring(20, 2)), int.Parse(line.Substring(18, 2)), int.Parse(line.Substring(18, 2)));
                        listaPassagem.Add(new PassagemVoo
                            (
                            line.Substring(0, 6),
                            line.Substring(6, 5),
                            new DateTime(int.Parse(line.Substring(15, 4)), int.Parse(line.Substring(13, 2)), int.Parse(line.Substring(11, 2)), int.Parse(line.Substring(19, 2)), int.Parse(line.Substring(21, 2)), int.Parse(line.Substring(21, 2))),
                            //tempo1 = DateTime.Parse((tempo).ToString("yyyy-MM-dd HH:mm:ss")),
                            line.Substring(23, 4),
                            char.Parse(line.Substring(27, 1))
                            ));
                    }

                    Console.WriteLine("\nArquivo carregado com sucesso!");
                }

            }

            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
        public void AcessarPassagem(List<Aeronave> aeronave, List<Voo> listaVoo, List<PassagemVoo> listaPassagem)
        {
            int opcao = 0;
            bool condicaoDeParada = false;
            PassagemVoo passagem = new();

            do
            {
                Console.Clear();

                Console.WriteLine("OPÇÃO: ACESSAR PASSAGEM\n");

                Console.WriteLine("1 - Cadastrar Passagem");
                Console.WriteLine("2 - Editar Passagem");
                Console.WriteLine("3 - Localizar Passagem");
                Console.WriteLine("4 - Imprimir Passagens");
                Console.WriteLine("\n9 - Voltar ao menu anterior");
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    condicaoDeParada = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("Parametro de entrada inválido!");
                    Console.WriteLine("Pressione enter para escolher novamente!");
                    Console.ReadKey();
                    condicaoDeParada = true;
                }

                if (opcao < 1 || opcao > 4 && opcao != 9)
                {
                    if (!condicaoDeParada)
                    {
                        Console.WriteLine("Escolha uma das opções disponiveis!!");
                        Console.WriteLine("Pressione enter para escolher novamente!");
                        Console.ReadKey();
                        condicaoDeParada = true;
                    }
                }

                switch (opcao)
                {
                    case 1:
                        passagem.CadastrarPassagem(aeronave, listaVoo, listaPassagem);
                        Console.ReadKey();
                        break;

                    case 2:
                        passagem.EditarPassagem(listaVoo, listaPassagem);
                        Console.ReadKey();
                        break;

                    case 3:
                        passagem.LocalizarPassagem(listaVoo, listaPassagem);
                        break;

                    case 4:
                        passagem.ImprimirPassagem(listaPassagem);
                        Console.ReadKey();
                        break;
                }

            } while (opcao != 9);
        }
        public string getData()
        {
            return $"{"PA" + IdPassagem.PadRight(4)}{"V" + IdVoo.PadRight(4)}{DataUltimaOperacao.ToString("ddMMyyyyHHmm")}{Valor.PadRight(4)}{Situacao}";
        }
        public override string ToString()
        {
            return "\nID da passagem: " + this.IdPassagem + "\nID do Voo: " + this.IdVoo + "\nData da Ultima Operação: " + this.DataUltimaOperacao + "\nValor da Passagem: R$ " + int.Parse(Valor) + "\nSituação da passagem: " + this.Situacao;

        }
    }
}






