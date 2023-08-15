using AccountAuthMicroservice.Models;
using AccountAuthMicroservice.Exceptions;
using AccountAuthMicroservice.Repositories;
using AccountAuthMicroservice.Repositories.Interface;
using AccountAuthMicroservice.ViewModels.Request;
using AccountAuthMicroservice.ViewModels.Response;
using AutoMapper;

namespace AccountAuthMicroservice.Services.Impl;

public class StoreService : IStoreService
{
    private IRepository<Store> _storeRepository;
    private IPersistence _persistence;
    private IMapper _mapper;

    public StoreService(IRepository<Store> storeRepository, IPersistence persistence, IMapper mapper)
    {
        _storeRepository = storeRepository;
        _persistence = persistence;
        _mapper = mapper;
    }

    public async Task CreateStoreAsync(StoreRequestDto storeRequestDto)
    {
        if (await _storeRepository.FindById(storeRequestDto.Id) != null)
            throw new BadRequestException("Gagal membuat toko, nomor identitas toko sudah terdaftar");

        Store store = _mapper.Map<Store>(storeRequestDto);
        await _storeRepository.Save(store);
        await _persistence.SaveChangesAsync();
    }

    public async Task UpdateStoreAsync(StoreRequestDto storeRequestDto, string roleId)
    {
        if (roleId.Equals("3")) throw new UnauthorizedException("Akses ditolak");
        
        var findById = await _storeRepository.FindById(storeRequestDto.Id);
        if (findById == null) throw new NotFoundException("Toko tidak ditemukan");
        try
        {
            await _persistence.BeginTransactionAsync();
            
            var store = _mapper.Map<Store>(storeRequestDto);
            _storeRepository.Update(store);
            
            await _persistence.CommitTransactionAsync();
            await _persistence.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _persistence.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<StoreResponseDto>> ListStoreAsync(string roleId)
    {
        if (!roleId.Equals("1")) throw new UnauthorizedException("Akses ditolak");
        
        var listStores = await _storeRepository.FindAll();
        var result = _mapper.Map<IEnumerable<StoreResponseDto>>(listStores);
        return result;
    }

    public async Task<StoreResponseDto> FindStoreById(string storeId)
    {
        var store = await _storeRepository.FindById(storeId);
        if (store == null) throw new NotFoundException("Data toko tidak ditemukan");
        return _mapper.Map<StoreResponseDto>(store);
    }
}