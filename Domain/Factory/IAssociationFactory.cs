using Domain.Model;

namespace Domain.Factory
{
    public interface IAssociationFactory
    {
        Association NewAssociation(long colaboratorId, long projectId, DateOnly startDate, DateOnly endDate);
    }
}