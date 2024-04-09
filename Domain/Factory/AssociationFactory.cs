using Domain.Model;

namespace Domain.Factory
{
    public class AssociationFactory : IAssociationFactory
    {
        public Association NewAssociation(long colaboratorId, long projectId, Period period){

            return new Association(colaboratorId, projectId, period);
        }
    }
}