using MachineryCRM.Application.DTOs;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Domain.Entities;
using MachineryCRM.Domain.Interfaces;

namespace MachineryCRM.Application.Services;

public class TransferHistoryService : ITransferHistoryService
{
    private readonly IMachineRepository _machineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferHistoryService(IMachineRepository machineRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferHistoryDto> CreateAsync(CreateTransferHistoryDto dto)
    {
        var machine = await _machineRepository.GetByIdAsync(dto.MachineId);
        if (machine == null)
            throw new KeyNotFoundException("Machine not found.");

        var transfer = new TransferHistory(dto.MachineId, machine.SiteId, dto.DestinationSiteId, DateTime.UtcNow, dto.Reason, dto.LoggedBy);
        
        _machineRepository.AddTransferHistory(transfer);
        
        // Em um domínio avançado, a atualização do SiteId na própria máquina (machine.ChangeSite) ocorreria aqui simultaneamente.
        
        await _unitOfWork.CommitAsync();

        return new TransferHistoryDto
        {
            Id = transfer.Id,
            MachineId = transfer.MachineId,
            OriginSiteId = transfer.OriginSiteId,
            DestinationSiteId = transfer.DestinationSiteId,
            TransferDate = transfer.TransferDate,
            Reason = transfer.Reason,
            LoggedBy = transfer.LoggedBy
        };
    }
}