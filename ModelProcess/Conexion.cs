using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelProcess
{
    public class Conexion
    {
        protected string strOracle = string.Empty;
        public string Conexion1()
        {
            strOracle = ConfigurationManager.ConnectionStrings["OraConexAmazon"].ConnectionString;

            return strOracle;
        }

        //public string Conectar()
        //{
        //   string cnn = strOracle;

        //     return cnn;
        //}

    }
}
