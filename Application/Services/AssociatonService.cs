namespace Application.Services;

using Domain.Model;
using Application.DTO;

using Microsoft.EntityFrameworkCore;
using Domain.IRepository;

public class AssociatonService
{
    private readonly IAssociationRepository _associationRepository;

    public AssociatonService(IAssociationRepository associationRepository)
    {
        _associationRepository = associationRepository;
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

        return assoDTO;
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
