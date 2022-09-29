using System;
using POnTheFly;

namespace PON_THE_FLY_2.O.Entidades
{
    public class Program
    {
        static void Main(string[] args)
        {
            int opcao = 9;
            bool condicaoDeParada;

            do
            {
                Console.Clear();

                Console.WriteLine("Bem-Vindo ao Aeroporto ON THE FLY\n\n");

                Console.WriteLine("Selecione a opção desejada: ");
                Console.WriteLine("\n1 - Companhias");
                Console.WriteLine("2 - Aeronaves");
                Console.WriteLine("3 - Voo");
                Console.WriteLine("4 - Passagens");
                Console.WriteLine("5 - Vendas");
                Console.WriteLine("6 - Passageiros");
                Console.WriteLine("7 - Restritos");
                Console.WriteLine("\n0 - Sair");
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

                if (opcao < 0 || opcao > 7)
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
                        CompanhiaAerea.AcessarCompanhia();
                        break;

                    case 2:
                        Aeronave.AcessarAeronave();
                        break;

                    case 3:
                        Voo.AcessarVoo();
                        break;

                    case 4:
                        PassagemVoo.AcessarPassagem();
                        break;

                    case 5:
                        Venda.AcessarVenda();
                        break;
                        
                    case 6:
                        Passageiro.AcessarPassageiro();
                        break;
                       
                    case 7:
                        Restricao.AcessarRestritos();                        
                        break; 
                }

            } while (opcao != 0);
        }
    }
}
