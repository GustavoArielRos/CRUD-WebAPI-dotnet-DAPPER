using AutoMapper;
using CrudDapperVideo.Dto;
using CrudDapperVideo.Models;
using Dapper;
using System.Data.SqlClient;

namespace CrudDapperVideo.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        //fazendo uma injeção de dependencia para poder usar o objeto IConfiguration
        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<UsuarioListarDto>> BuscarUsuarioPorId(int usuarioId)
        {
            ResponseModel<UsuarioListarDto> response = new ResponseModel<UsuarioListarDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //new {Id = usuarioId}, é para falar que esse id com @ é o do recebido no parâmetro
                //QueryFirstOrDefaultAsync caso não tenha o usuario com id ele retorna um null
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("select * from Usuarios where id = @Id", new {Id = usuarioId});

                if(usuarioBanco == null)
                {
                    response.Mensagem = "Nenhum usuário localizado!";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<UsuarioListarDto>(usuarioBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuário localizado com sucesso";

            }

            return response;
        }

        //método assícrono, ou seja, é para esperar a conexão vir para rodar as outras coisas dele
        public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()
        {
            //propriedade vazia
            //criando a lista vazia para receber as informações do banco de dados
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            //dentro dos parenteses inicia uma conexão que fica aberta até o final do colchetes, ou seja,
            //quando esse using finaliza
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //diretiva await , espera essa requisição e depois vai para o próximo
                //a conexão vai fazer uma query e vai buscar o objeto usuário
                //estou aqui entrando dentro do usuário e rodando essa query que pega a tabela Usuarios
                //e mapeio essas informações para o objeto "Usuario"
                var usuariosBanco = await connection.QueryAsync<Usuario>("select * from Usuarios ");

                //caso não tem informações na tabela
                if (usuariosBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum usuário localizado!";
                    response.Status = false;
                    return response;
                }

                //Tranformação Mapper(transformar de lista de usuário para lista de usuárioDto)
                //temos que instalar antes o pacote AutoMapper
                var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuariosBanco);

                response.Dados = usuarioMapeado;
                response.Mensagem = "Usuários Localizados com sucesso!";
                
            }

            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuarioCriarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //Execute Async, retorna apenas o numero de linhas que foram afetadas
                var usuariosBanco = await connection.ExecuteAsync("insert into Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao)" +
                                                                    "values (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", usuarioCriarDto);

                //não mudou nenhuma linha ou seja , deu erro
                if(usuariosBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }

                //esse é um método private que será usado somente aqui
                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários listados com sucesso!";
            }

            return response;

        }

        //método que só funciona aqui nesse arquivo, ele chama todos os usuários da tabela
        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
        {

            return await connection.QueryAsync<Usuario>("select * from Usuarios");


        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //exeute async , executa uma operação e devolve o numero de linhas alteradas
                var usuariosBanco = await connection.ExecuteAsync("update Usuarios set NomeCompleto = @NomeCompleto," +
                                                                                       "Email = @Email," +
                                                                                       "Cargo = @Cargo," +
                                                                                       "Salario = @Salario," +
                                                                                       "Situacao= @Situacao," +
                                                                                       "CPF = @CPF where Id = @Id", usuarioEditarDto);

                if(usuariosBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar a edição!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários listados com sucesso!";

            }

            return response;

        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuario(int usuarioId)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                //"new {Id = usuarioId}, para falar que o @id é o do parâmetro "
                var usuariosBanco = await connection.ExecuteAsync("delete from Usuarios where id = @Id", new {Id = usuarioId});

                if(usuariosBanco == 0 )
                {
                    response.Mensagem = "Ocorreu um erro ao realizar a edição!";
                    response.Status = false;
                    return response;
                }

                var usuarios = await ListarUsuarios(connection);

                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Dados = usuariosMapeados;
                response.Mensagem = "Usuários Listados com sucesso";

            }

            return response;

        }
    }
}
