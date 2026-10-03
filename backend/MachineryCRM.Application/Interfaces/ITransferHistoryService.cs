using MachineryCRM.Application.DTOs;

namespace MachineryCRM.Application.Interfaces;

public interface ITransferHistoryService
{
    Task<TransferHistoryDto> CreateAsync(CreateTransferHistoryDto dto);
}