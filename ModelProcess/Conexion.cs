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
        public Conexion()
        {
            strOracle = ConfigurationManager.ConnectionStrings["OraConexAmazon"].ConnectionString;
        }
    }
}
