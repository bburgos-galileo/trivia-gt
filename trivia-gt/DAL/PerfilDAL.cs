using MySql.Data.MySqlClient;
using System.Data;
using trivia_gt.Models;

namespace trivia_gt.DAL
{
    public class PerfilDAL :IDALBase<UsuarioBE>
    {

        MySqlConnection _conexionSQL;
        MySqlCommand _comandoSQL;

        MySqlParameter _idUsuario;
        MySqlParameter _nombres;
        MySqlParameter _apellidos;
        MySqlParameter _fechaNacimiento;
        MySqlParameter _correo;
        MySqlParameter _idAvatar;
        MySqlParameter _idRol;
        MySqlParameter _clave;

        public PerfilDAL()
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
                case "IdUsuario":
                    _idUsuario = new MySqlParameter
                    {
                        ParameterName = "@IdUsuario",
                        MySqlDbType = MySqlDbType.Int32,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "Nombres":
                    _nombres = new MySqlParameter
                    {
                        ParameterName = "@Nombres",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "Apellidos":
                    _apellidos = new MySqlParameter
                    {
                        ParameterName = "@Apellidos",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "FechaNacimiento":
                    _fechaNacimiento = new MySqlParameter
                    {
                        ParameterName = "@FechaNacimiento",
                        MySqlDbType = MySqlDbType.Date,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "Correo":
                    _correo = new MySqlParameter
                    {
                        ParameterName = "@CorreoElectronico",
                        MySqlDbType = MySqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "IdAvatar":
                    _idAvatar = new MySqlParameter
                    {
                        ParameterName = "@IdAvatar",
                        MySqlDbType = MySqlDbType.Int32,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "IdRol":
                    _idRol = new MySqlParameter
                    {
                        ParameterName = "@IdRol",
                        MySqlDbType = MySqlDbType.Int32,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "Clave":
                    _clave = new MySqlParameter
                    {
                        ParameterName = "@Clave",
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
                string sql = "select idUsuario, nombres, apellidos, fechaNacimiento, correoElectronico,contraseña,idAvatar,idRol FROM usuarios WHERE correoElectronico = @CorreoElectronico";

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
                        Nombres = item["nombres"].ToString(),
                        Apellidos = item["apellidos"].ToString(),
                        Correo = item["correoElectronico"].ToString(),
                        Clave = item["contraseña"].ToString(),
                        ConfirmacionClave = item["contraseña"].ToString(),
                        IdAvatar = (int)item["idAvatar"],
                        IdRol = (int)item["idRol"],
                        FechaNacimiento = String.Format("{0:yyyy-MM-dd}", item["fechaNacimiento"])
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
                string sql = "UPDATE usuarios SET nombres = @Nombres, apellidos = @Apellidos, " +
                             "fechaNacimiento = @FechaNacimiento, correoElectronico = @CorreoElectronico, " +
                             "contraseña = ifnull(@Clave, contraseña), idAvatar = @IdAvatar, idRol = @IdRol WHERE idUsuario= @IdUsuario";

                CrearComando(sql, CommandType.Text, _conexionSQL);

                CrearParametro("IdUsuario", entidad.IdUsuario);
                AgregarParametro(_idUsuario);

                CrearParametro("Nombres", entidad.Nombres);
                AgregarParametro(_nombres);

                CrearParametro("Apellidos", entidad.Apellidos);
                AgregarParametro(_apellidos);

                CrearParametro("FechaNacimiento", entidad.FechaNacimiento);
                AgregarParametro(_fechaNacimiento);

                
                CrearParametro("Correo", entidad.Correo);
                AgregarParametro(_correo);

                if (entidad.Clave != null)
                {
                    CrearParametro("Clave", entidad.Clave);
                    AgregarParametro(_clave);
                }

                CrearParametro("IdAvatar", entidad.IdAvatar);
                AgregarParametro(_idAvatar);

                CrearParametro("IdRol", entidad.IdRol);
                AgregarParametro(_idRol);

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
            try
            {
                string sql = "";

                CrearComando(sql, CommandType.Text, _conexionSQL);

                _conexionSQL.Open();
                _comandoSQL.ExecuteNonQuery();
                _conexionSQL.Close();

                return 1;

            }
            catch (Exception ex)
            {
                if (_conexionSQL.State == ConnectionState.Open)
                    _conexionSQL.Close();

                throw new Exception(ex.Message);
            }
        }

        public bool Eliminar(UsuarioBE entidad)
        {
            throw new NotImplementedException();
        }
    }
}
