using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using ModelProcess;

namespace WebServicePro
{
    public class Log : Conexion
    {

        public string Login(string usu, string pass)  //( USUARIO dto)
        {

            // var strOracle = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=prtfl.chcqocx7scvr.sa-east-1.rds.amazonaws.com)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=prtfl)));User Id=admin;Password=portafolio123;"

            // Boolean result = false;
            int Perfil = 0;
            string user = "";
            string psw = "";
            string msg = "";

            List<string> UserPasword = new List<string>();
            try
            {


                using (OracleConnection cn = new OracleConnection(strOracle))
                {
                    cn.Open();

                    OracleCommand cmd = new OracleCommand("SELECT p.id_perfil FROM PERFIL P " +
                        "INNER JOIN USUARIO US ON P.ID_PERFIL = US.ID_PERFIL where US.NOMBRE_USUARIO = :usu AND CONTRASENA = :pass", cn);
                    OracleCommand comando = new OracleCommand("SELECT * FROM USUARIO WHERE NOMBRE_USUARIO = :usu AND CONTRASENA = :pass", cn);

                    comando.Parameters.AddWithValue(":usu", usu);
                    comando.Parameters.AddWithValue(":pass", pass);

                    cmd.Parameters.AddWithValue(":usu", usu);
                    cmd.Parameters.AddWithValue(":pass", pass);

                    OracleDataReader lector = comando.ExecuteReader();
                    OracleDataReader _reader = cmd.ExecuteReader();

                    if (lector.Read() == true)
                    {

                        user = lector.GetString(9);
                        psw = lector.GetString(10);

                        if (_reader.Read())
                        {
                            Perfil = _reader.GetInt32(0);

                            UserPasword.Add(Perfil.ToString());
                        }

                        UserPasword.Add(user);

                        UserPasword.Add(psw);
                    }
                    else
                    {

                        msg = ("No hay coincidencias");
                    }

                    cn.Close();
                }

            }
            catch (Exception ex)
            {

                new Exception("Usuario o Contraseña incorrecta " + ex.Message);

            }
            return msg;

        }
    }
}