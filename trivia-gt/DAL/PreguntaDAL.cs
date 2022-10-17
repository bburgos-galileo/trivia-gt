using MySql.Data.MySqlClient;
using System.Data;
using trivia_gt.Models;

namespace trivia_gt.DAL
{
    public class PreguntaDAL : IDALBase<PreguntaBE>
    {
        MySqlConnection _conexionSQL;
        MySqlCommand _comandoSQL;

        MySqlParameter _idPunteo;
        MySqlParameter _punteo;
        MySqlParameter _intentos;
        MySqlParameter _nivel;
        MySqlParameter _idUsuario;
        MySqlParameter _idPregunta;
        MySqlParameter _idEstado;

        public PreguntaDAL()
        {
            _conexionSQL = Conexion.ObtenerConexion();
        }

        public void CrearParametro(string nombre, object valor)
        {
            switch (nombre)
            {
                case "idPunteo":
                    _idPunteo = new MySqlParameter
                    {
                        ParameterName = "@IdPunteo",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "punteo":
                    _punteo = new MySqlParameter
                    {
                        ParameterName = "@punteo",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "intentos":
                    _intentos = new MySqlParameter
                    {
                        ParameterName = "@intentos",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "nivel":
                    _nivel = new MySqlParameter
                    {
                        ParameterName = "@nivel",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "idUsuario":
                    _idUsuario = new MySqlParameter
                    {
                        ParameterName = "@idUsuario",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "idPregunta":
                    _idPregunta = new MySqlParameter
                    {
                        ParameterName = "@idPregunta",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
                case "idEstado":
                    _idEstado = new MySqlParameter
                    {
                        ParameterName = "@idEstado",
                        MySqlDbType = MySqlDbType.Int16,
                        Direction = ParameterDirection.Input,
                        Value = valor
                    };
                    break;
            }
        }

        public void CrearComando(string cmdText, CommandType tipo, MySqlConnection conexion)
        {
            _comandoSQL = new MySqlCommand(cmdText, conexion)
            {
                CommandType = tipo
            };
        }

        public void AgregarParametro(MySqlParameter parametro)
        {
            _comandoSQL.Parameters.Add(parametro);
        }

        public DataTable ListarDS(PreguntaBE entidad)
        {
            try
            {
                string sql = "select p.idPunteo, p.punteo, p.intentos, p.nivel, p.idUsuario, " + 
                             "p.idPregunta, p.idEstado from punteo p where idUsuario = @idUsuario";

                using (MySqlConnection connection = _conexionSQL)
                {
                    CrearComando(sql, CommandType.Text, _conexionSQL);

                    CrearParametro("idUsuario", entidad.idUsuario);
                    AgregarParametro(_idUsuario);

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

        public IList<PreguntaBE> Listar(PreguntaBE entidad)
        {
            try
            {
                IList<PreguntaBE> _lista = new List<PreguntaBE>();
                PreguntaBE _be;

                DataTable _dt = new DataTable();

                _dt = ListarDS(entidad);

                foreach (DataRow item in _dt.Rows)
                {
                    _be = new PreguntaBE
                    {
                        idPunteo = (int)item["idPunteo"],
                        punteo = (int)item["punteo"],
                        intentos = (int)item["intentos"],
                        nivel = (int)item["nivel"],
                        idUsuario = (int)item["idUsuario"],
                        idPregunta = (int)item["idPregunta"],
                        idEstado = (int)item["idEstado"]

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

        public int Crear(PreguntaBE entidad)
        {
            try
            {
                string sql = "INSERT INTO punteo (punteo, intentos,nivel, idUsuario, idPregunta, idEstado) " + 
                             "VALUES (@punteo, @intentos, @nivel, @idUsuario, @idPregunta, @idEstado)";

                CrearComando(sql, CommandType.Text, _conexionSQL);

                CrearParametro("punteo", entidad.punteo);
                AgregarParametro(_punteo);

                CrearParametro("intentos", entidad.intentos);
                AgregarParametro(_intentos);

                CrearParametro("nivel", entidad.nivel);
                AgregarParametro(_nivel);

                CrearParametro("idUsuario", entidad.idUsuario);
                AgregarParametro(_idUsuario);

                CrearParametro("idPregunta", entidad.idPregunta);
                AgregarParametro(_idPregunta);

                CrearParametro("idEstado", entidad.idEstado);
                AgregarParametro(_idEstado);

                _conexionSQL.Open();
                _ = _comandoSQL.ExecuteScalar();
                //_comandoSQL.ExecuteNonQuery();
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

        public bool Actualizar(PreguntaBE entidad)
        {
            try
            {
                string sql = "update punteo set punteo = @punteo, intentos = @intentos, idEstado = @idEstado where idPunteo = @idPunteo";

                CrearComando(sql, CommandType.Text, _conexionSQL);

                CrearParametro("idPunteo", entidad.idPunteo);
                AgregarParametro(_idPunteo);

                CrearParametro("punteo", entidad.punteo);
                AgregarParametro(_punteo);

                CrearParametro("intentos", entidad.intentos);
                AgregarParametro(_intentos);

                CrearParametro("idEstado", entidad.idEstado);
                AgregarParametro(_idEstado);

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

        public bool Eliminar(PreguntaBE entidad)
        {
            throw new NotImplementedException();
        }


    }
}
