namespace Pay.Recorrencia.Gestao.Consumer.Models
{
    public class ConsumeResultMetadata
    {
        public ConsumeResultMetadata(int pARTITION, long oFFSET, string tOPIC, string? eNTITY)
        {
            PARTITION = pARTITION;
            OFFSET = oFFSET;
            TOPIC = tOPIC;
            ENTITY = eNTITY;
        }

        public int PARTITION { get; set; }
        public long OFFSET { get; set; }
        public string? TOPIC { get; set; }
        public string? ENTITY { get; set; }
    }
}