using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using ModelProcess;

namespace WebServicePro
{
    /// <summary>
    /// Descripción breve de ServiceProcess
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceProcess : System.Web.Services.WebService
    {
        ModelProcess.Conexion con = new Conexion();

        //string str = con.Conexion1();

        [WebMethod]
            public string HelloWorld()
            {
                return "Hola a todos";
            }


            


            [WebMethod]
            public string Login(string usu, string pass) //( USUARIO dto)
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
                
               

                using (OracleConnection cn = new OracleConnection(con.Conexion1()))
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

                            msg = "ok";
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

            [WebMethod]

        public List<DashboardGen> DashBoard(int rut, string rut_empresa)
        {
            List<DashboardGen> list = new List<DashboardGen>();
            DashboardGen uni;
            try
            {
                using (OracleConnection cn = new OracleConnection(con.Conexion1()))
                {
                    cn.Open();
                    using (OracleCommand cmd = new OracleCommand("SP_DASHBOARD_GENERICO", cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new OracleParameter("P_RUT", OracleType.Number)).Value = rut;
                        cmd.Parameters.Add(new OracleParameter("P_EMPRESA", OracleType.Number)).Value = rut_empresa;
                        //cmd.Parameters.Add(new OracleParameter("P_RUT", OracleType.Cursor)).Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new OracleParameter("P_CURSOR", OracleType.Cursor)).Direction = System.Data.ParameterDirection.Output;
                        using (OracleDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                uni = new DashboardGen();
                                uni.NOMBRE_UNIDAD = Convert.ToString(dr["NOMBRE_UNIDAD"]);
                                uni.FECHACREACION = Convert.ToDateTime(dr["FECHACREACION"]);
                                uni.FECHA_ESTIMADA = Convert.ToString(dr["FECHA_ESTIMADA"]);
                                uni.FECHA_TERMINO = Convert.ToString(dr["FECHA_TERMINO"]);
                                uni.Tareas_ter = Convert.ToInt32(dr["Cant_tareas_Ter"]);
                                uni.Cant_tareas_tot = Convert.ToInt32(dr["Cant_tareas_tot"]);
                                uni.procentaje = Convert.ToInt32(dr["Porcentaje"]);
                                uni.ESTADO = Convert.ToString(dr["ESTADO"]);
                                uni.ATRASO = Convert.ToInt32(dr["Atraso"]);
                                uni.nombre_usurio = Convert.ToString(dr["nombre_usuario"]);

                                list.Add(uni);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                new Exception("Error en metodo dashboard " + ex.Message);
            }
            return list;
        }


    }
}
