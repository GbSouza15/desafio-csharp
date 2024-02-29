using desafio.Models;

namespace desafio.Repositories
{
    public interface IProducerRepository
    {
        List<ProducerModel> GetProducers(int userId);
        ProducerModel GetProducersById(int userId, int id);
        ProducerModel UpdateProducer(int userId, ProducerModel producerModel);
        ProducerModel Add(int userId, ProducerModel producer);
    }
}
