using MySql.Data.MySqlClient;
using System.Data;
using trivia_gt.Models;

namespace trivia_gt.DAL
{
    public class PunteoDAL : IDALBase<PunteoBE>
    {

        MySqlConnection _conexionSQL;
        MySqlCommand _comandoSQL;

        MySqlParameter _correo;

        public PunteoDAL()
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

        public DataTable ListarDS(PunteoBE entidad)
        {
            try
            {
                string sql = "SELECT convert(SUM(p.punteo), char) punteo, convert(count(p.idPregunta), char) preguntas, u.correoElectronico correo " +
                             "FROM punteo p inner join usuarios u on p.idUsuario = u.idUsuario where p.idEstado = 1 group by u.correoElectronico";

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

        public List<PunteoBE> Listar(PunteoBE entidad)
        {
            try
            {
                List<PunteoBE> _lista = new List<PunteoBE>();
                PunteoBE _be;

                DataTable _dt = new DataTable();

                _dt = ListarDS(entidad);

                foreach (DataRow item in _dt.Rows)
                {
                    _be = new PunteoBE
                    {
                        Punteo = item["punteo"].ToString(),
                        TotalPreguntas = item["preguntas"].ToString(),
                        Correo = item["correo"].ToString(),
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

        public bool Actualizar(PunteoBE entidad)
        {
            throw new NotImplementedException();
        }

        public int Crear(PunteoBE entidad)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(PunteoBE entidad)
        {
            throw new NotImplementedException();
        }

    }
}
