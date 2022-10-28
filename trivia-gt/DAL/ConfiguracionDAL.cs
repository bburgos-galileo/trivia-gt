using MySql.Data.MySqlClient;
using System.Data;
using trivia_gt.Models;

namespace trivia_gt.DAL
{
    public class ConfiguracionDAL : IDALBase<ConfiguracionBE>
    {

        MySqlConnection _conexionSQL;
        MySqlCommand _comandoSQL;

        MySqlParameter _idConfiguracion;
        MySqlParameter _urlApi;
        MySqlParameter _noGrupo;

        public ConfiguracionDAL()
        {
            _conexionSQL = Conexion.ObtenerConexion();
        }

        public void CrearComando(string cmdText, CommandType tipo, MySqlConnection conexion)
        {
            _comandoSQL = new MySqlCommand(cmdText, conexion)
            {
                CommandType = tipo
            };
        }

        public void CrearParametro(string nombre, object valor)
        {
            switch (nombre)
            {
                case "idConfiguracion":
                    _idConfiguracion = new MySqlParameter
                    {
                        ParameterName = "@idConfiguracion",
                        MySqlDbType = MySqlDbType.Int32,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "urlApi":
                    _urlApi = new MySqlParameter
                    {
                        ParameterName = "@urlApi",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "noGrupo":
                    _noGrupo = new MySqlParameter
                    {
                        ParameterName = "@noGrupo",
                        MySqlDbType = MySqlDbType.Int32,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
            }
        }

        public void AgregarParametro(MySqlParameter parametro)
        {
            _comandoSQL.Parameters.Add(parametro);
        }
        public DataTable ListarDS(ConfiguracionBE entidad)
        {
            try
            {
                string sql = "select idconfiguracion, urlApi, noGrupo from configuracion";

                using (MySqlConnection connection = _conexionSQL)
                {
                    CrearComando(sql, CommandType.Text, _conexionSQL);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(_comandoSQL))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            DataSet _ds = new DataSet();
                            connection.Open();
                            da.Fill(dt);

                            return dt;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (_conexionSQL.State == ConnectionState.Open)
                    _conexionSQL.Close();
                throw new Exception(ex.Message);
            }
        }

        public List<ConfiguracionBE> Listar(ConfiguracionBE entidad)
        {
            try
            {
                List<ConfiguracionBE> _lista = new List<ConfiguracionBE>();
                ConfiguracionBE _be;

                DataTable _dt = new DataTable();

                _dt = ListarDS(entidad);

                foreach (DataRow item in _dt.Rows)
                {
                    _be = new ConfiguracionBE
                    {
                        idConfiguracion = (int)item["idConfiguracion"],
                        urlApi = item["urlApi"].ToString(),
                        noGrupo = (int)item["noGrupo"]
                    };

                    _lista.Add(_be);
                }

                return _lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Actualizar(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }
        public int Crear(ConfiguracionBE entidad)
        {
            try
            {
                string sql = "INSERT INTO configuracion SET idConfiguracion = @idConfiguracion, urlApi = @urlApi, " +
                             "noGrupo = @noGrupo, ";

                CrearComando(sql, CommandType.Text, _conexionSQL);

                CrearParametro("idConfiguracion", entidad.idConfiguracion);
                AgregarParametro(_idConfiguracion);

                CrearParametro("urlApi", entidad.urlApi);
                AgregarParametro(_urlApi);

                CrearParametro("noGrupo", entidad.noGrupo);
                AgregarParametro(_noGrupo);

                _conexionSQL.Open();
                _comandoSQL.ExecuteNonQuery();
                _conexionSQL.Close();

                return true;

            }
            catch (Exception ex)
            {
                if (_conexionSQL.State == ConnectionState.Open)
                    _conexionSQL.Close();

                throw new Exception(ex.Message);
            }
        }

        public bool Eliminar(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }

    }
}
