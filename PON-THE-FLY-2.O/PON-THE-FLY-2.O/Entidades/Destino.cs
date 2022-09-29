using System.Collections.Generic;

namespace PON_THE_FLY_2.O.Entidades
{
    public class Destino
    {       
        public Destino()
        {            
        }
        public bool ListaDestino(string destino)
        {
            List<string> IATA = new() {"ALC", "AMS", "AJU", "AQP", "AUA", "ASU", "ATH", "ATL", "BWI", "BKK", "BCN", "BRC"
                , "BHZ", "CNF", "PLU", "BER", "TXL", "BIO", "BHM", "BVB", "BOG", "BLK", "BYO", "BOS", "BSB", "BNE", "BRU"
                , "BUE", "CFB", "CLO", "CGR", "CUN", "CCS", "CTG", "CXJ", "XAP", "CLT", "CHI", "MEX", "PTY", "CVG", "CLE"
                , "CGN", "CGH", "CPH", "CPO", "COR", "CGB", "CUR" , "CWB", "CUZ", "BFW", "DNE", "DTW", "DOU", "DXB","DUP"
                , "DUB", "DUS", "EDI", "ARN", "FAO", "FEN" ,"PHL" , "FLR", "FLN", "FLL", "FOR", "IGU", "FRA", "GIG","GVA"
                , "GOA", "GRU", "GYN", "JPR", "JPA", "JOI" , "DEL"
            };

            foreach (var i in IATA)
            {
                if (destino == i)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
