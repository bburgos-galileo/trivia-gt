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
            throw new NotImplementedException();
        }
        public DataTable ListarDS(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }

        public List<ConfiguracionBE> Listar(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }

        public int Crear(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(ConfiguracionBE entidad)
        {
            throw new NotImplementedException();
        }

    }
}
