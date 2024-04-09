using Domain.Model;

namespace Domain.Factory
{
    public class AssociationFactory : IAssociationFactory
    {
        public Association NewAssociation(long colaboratorId, long projectId, DateOnly startDate, DateOnly endDate){

            return new Association(colaboratorId, projectId, startDate, endDate);
        }
    }
}