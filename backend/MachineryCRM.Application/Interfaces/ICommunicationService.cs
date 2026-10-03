using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface ICommunicationService
{
    Task<CommunicationDto> CreateAsync(CreateCommunicationDto dto);
}