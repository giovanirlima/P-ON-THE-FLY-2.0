using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PON_THE_FLY_2.O.Entidades
{
    internal class Mensagem
    {              
        public static void ParametroMessage()
        {
            Console.Write("\nParametro de dados inválidos!\n");
        }
        public static void TrueCadastradoMessage()
        {
            Console.Write("\nCadastrado com sucesso!");
        }
        public static void FalseCadastradoMessage()
        {
            Console.Write("\nError ao realizar cadastro!");
        }
        public static void TrueAlteradoMessage()
        {
            Console.Write("\nAlterado com sucesso!");
        }
        public static void FalseAlteradoMessage()
        {
            Console.Write("\nError na alteração!");
        }
        public static void TrueCompraMessage()
        {
            Console.Write("\nCompra efetuada com sucesso!");
        }
        public static void FalseCompraMessage()
        {
            Console.Write("\nError na compra!");
        }
        public static void OpcaoMessage()
        {
            Console.Write("\nOpção digitada é inválida!\n");
        }
    }
}
