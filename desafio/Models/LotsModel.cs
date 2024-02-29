namespace desafio.Models
{
    public class LotsModel
    {
        public int Id { get; set; }
        public string CodeLot { get; set; }
        public int IdPerimeter { get; set; }
        public int IdProducer { get; set; }
        public int IdUser { get; set; }
        public string? PerimeterName { get; set; }
        public string? ProducerName { get; set; }
    }
}
