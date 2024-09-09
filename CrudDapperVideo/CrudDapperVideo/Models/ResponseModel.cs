namespace CrudDapperVideo.Models
{
    public class ResponseModel<T>
    {   
        //a propriedade de Dados pode ser de qualquer tipo(é melhor do que ficar criando vários tipos de responsemodel)
        //Dados pode ser nulo caso não venha informação
        public T? Dados { get; set; }
        public string Mensagem { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
    }
}
