using System;
using System.Data;
using System.Data.SqlClient;

namespace PON_THE_FLY_2.O.Entidades
{
    public class BancoAeroporto
    {       
        public static string CaminhoDeConexao()
        {           
            //Console.Clear();

            //Console.Write("Informe a instancia do servidor para conexão: ");
            //string ip = Console.ReadLine();

            //Console.Write("Informe o nome da database que será utilizada: ");
            //string database = Console.ReadLine();

            //Console.Write("Informe o loguin de usuário: ");
            //string loguin = Console.ReadLine();

            //Console.Write("Informe a senha: ");
            //string senha = Console.ReadLine();

            //return Conexao = $"Data Source={ip}; Initial Catalog={database}; User Id={loguin}; Password ={senha}";
            return @"Data Source = localhost; Initial Catalog = AEROPORTO; User Id = sa; Password = 227126993";

        }
        public static bool LocalizarDados(string sql, SqlConnection conexao)
        {
            BancoAeroporto caminho = new();
            conexao = new(CaminhoDeConexao());
            conexao.Open();

            SqlCommand cmd = new(sql, conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }
            }

            conexao.Close();

            return false;

        }
        public static string RetornoDados(string sql, SqlConnection conexao, string parametro)
        {
            var situacao = "";
            BancoAeroporto caminho = new();
            conexao = new(CaminhoDeConexao());
            conexao.Open();

            SqlCommand cmd = new(sql, conexao);      
            cmd.CommandType = CommandType.Text;            

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        situacao = reader[$"{parametro}"].ToString();
                    }
                }
            }

            conexao.Close();

            return situacao;
        }
        public static bool InsertDados(string sql, SqlConnection conexao)
        {            
            BancoAeroporto caminho = new();
            conexao = new(CaminhoDeConexao());
            conexao.Open();

            SqlCommand cmd = new(sql, conexao);
            try
            {
                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }
            
            conexao.Close();

            return false;
        }
        public static bool UpdateDados(string sql, SqlConnection conexao)
        {
            int contador;
            BancoAeroporto caminho = new();
            conexao = new(CaminhoDeConexao());
            conexao.Open();

            SqlCommand cmd = new(sql, conexao);

            contador = cmd.ExecuteNonQuery();
            conexao.Close();

            if (contador > 0)
            {                
                return true;
            }

            return false;
        }
        public static bool DeleteDados(string sql, SqlConnection conexao)
        {
            int contador = 0;
            BancoAeroporto caminho = new();
            conexao = new(CaminhoDeConexao());
            conexao.Open();

            SqlCommand cmd = new(sql, conexao);
            try
            {
                contador = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
            }

            conexao.Close();

            if (contador > 0)
            {
                return true;
            }

            return false;
        }


    }
}