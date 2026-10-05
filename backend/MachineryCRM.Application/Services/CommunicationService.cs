using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IMachineRepository _machineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommunicationService(IMachineRepository machineRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CommunicationDto> CreateAsync(CreateCommunicationDto dto)
    {
        var machine = await _machineRepository.GetByIdAsync(dto.MachineId);
        if (machine == null)
            throw new KeyNotFoundException("Machine not found.");

        var communication = new Communication(dto.MachineId, dto.ContactId, dto.AppUserId, dto.InteractionDate, dto.Channel, dto.Summary);
        
        _machineRepository.AddCommunication(communication);
        await _unitOfWork.CommitAsync();

        return new CommunicationDto
        {
            Id = communication.Id,
            MachineId = communication.MachineId,
            ContactId = communication.ContactId,
            AppUserId = communication.AppUserId,
            InteractionDate = communication.InteractionDate,
            Channel = communication.Channel,
            Summary = communication.Summary
        };
    }
}