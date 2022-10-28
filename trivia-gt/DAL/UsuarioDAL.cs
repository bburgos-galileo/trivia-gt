using MySql.Data.MySqlClient;
using System.Data;
using trivia_gt.Models;

namespace trivia_gt.DAL
{
    public class UsuarioDAL : IDALBase<UsuarioBE>
    {
        MySqlConnection _conexionSQL;
        MySqlCommand _comandoSQL;

        MySqlParameter _correo;

        public UsuarioDAL()
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
                case "Correo":
                    _correo = new MySqlParameter
                    {
                        ParameterName = "@Correo",
                        MySqlDbType = MySqlDbType.VarChar,
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

        public DataTable ListarDS(UsuarioBE entidad)
        {
            try
            {
                string sql = "select u.idUsuario, u.nombres, u.apellidos, u.fechaNacimiento, u.correoElectronico, " +
                             "u.contraseña, a.url, DATE_FORMAT(ifnull(u.ultimaConexion, now()), '%d/%m/%Y') ultimaConexion, " +
                             "convert(datediff(now(),ifnull(u.ultimaConexion, now())), char) diasUltimaConn, u.idRol " +
                             "from usuarios u inner join avatar a on u.idAvatar = a.idAvatar " +
                             "where u.correoElectronico = @Correo";

                using (MySqlConnection connection = _conexionSQL)
                {
                    CrearComando(sql, CommandType.Text, _conexionSQL);

                    CrearParametro("Correo", entidad.Correo);
                    AgregarParametro(_correo);

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

        public List<UsuarioBE> Listar(UsuarioBE entidad)
        {
            try
            {
                List<UsuarioBE> _lista = new List<UsuarioBE>();
                UsuarioBE _be;

                DataTable _dt = new DataTable();

                _dt = ListarDS(entidad);

                foreach (DataRow item in _dt.Rows)
                {
                    _be = new UsuarioBE
                    {
                        IdUsuario = (int)item["idUsuario"],
                        IdRol = (int)item["idRol"],
                        Nombres = item["nombres"].ToString(),
                        Apellidos = item["apellidos"].ToString(),
                        Correo = item["correoElectronico"].ToString(),
                        Clave = item["contraseña"].ToString(),
                        ConfirmacionClave = item["contraseña"].ToString(),
                        url = item["url"].ToString(),
                        diasUltimaConexion = item["diasUltimaConn"].ToString(),
                        fechaUltimaConexion = item["ultimaConexion"].ToString()
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

        public bool Actualizar(UsuarioBE entidad)
        {
            try
            {
                string sql = "update usuarios set ultimaConexion = now() where correoElectronico = @Correo";

                CrearComando(sql, CommandType.Text, _conexionSQL);

                CrearParametro("Correo", entidad.Correo);
                AgregarParametro(_correo);

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

        public int Crear(UsuarioBE entidad)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(UsuarioBE entidad)
        {
            throw new NotImplementedException();
        }


    }
}
