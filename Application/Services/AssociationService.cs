namespace Application.Services;

using Domain.Model;
using Application.DTO;

using Domain.IRepository;
using Gateway;

public class AssociationService
{
    private AssociationAmqpGateway _associationAmqpGateway;
    private readonly IAssociationRepository _associationRepository;

    public AssociationService(IAssociationRepository associationRepository, AssociationAmqpGateway associationAmqpGateway)
    {
        _associationRepository = associationRepository;
        _associationAmqpGateway = associationAmqpGateway;
    }

    public async Task<IEnumerable<AssociationDTO>> GetAll()
    {
        IEnumerable<Association> association = await _associationRepository.GetAssociationsAsync();

        IEnumerable<AssociationDTO> associationsDTO = AssociationDTO.ToDTO(association);

        return associationsDTO;
    }

    public async Task<AssociationDTO> GetById(long id)
    {
        Association association = await _associationRepository.GetAssociationsByIdAsync(id);

        if (association != null)
        {
            AssociationDTO associationDTO = AssociationDTO.ToDTO(association);

            return associationDTO;
        }
        return null;
    }

    public async Task<AssociationDTO> Add(AssociationDTO associationDTO)
    {

        Association association = AssociationDTO.ToDomain(associationDTO);

        Association associationSaved = await _associationRepository.Add(association);

        AssociationDTO assoDTO = AssociationDTO.ToDTO(associationSaved);

        string associationAmqpDTO = AssociationAmqpDTO.Serialize(assoDTO);
        _associationAmqpGateway.Publish(associationAmqpDTO);

        return assoDTO;
    }

    public async Task HandleMessage(AssociationDTO associationDTO)
    {
        Association existingAssociation = await _associationRepository.GetAssociationsByIdAsync(associationDTO.Id);

        if (existingAssociation == null)
        {
            Association association = AssociationDTO.ToDomain(associationDTO);
            Association associationSaved = await _associationRepository.Add(association);

            // await _messagePublisher.PublishNewAssociationMessage(associationDTO);
        }
        else
        {
            Console.WriteLine($"Association with identifier {associationDTO.Id} already exists in the database.");
        }
    }


    // public async Task<bool> Update(long id, AssociationDTO AssociacaoDTO, List<string> errorMessages)
    // {
    //     Holiday holiday = await _associacaoRepository.GetHolidayByIdAsync(id);

    //     if(holiday!=null)
    //     {
    //         AssociationDTO.UpdateToDomain(holiday, AssociacaoDTO);

    //         await _associacaoRepository.Update(holiday, errorMessages);

    //         return true;
    //     }
    //     else
    //     {
    //         errorMessages.Add("Not found");

    //         return false;
    //     }
    // }
}
