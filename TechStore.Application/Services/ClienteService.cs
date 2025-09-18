using AutoMapper;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Domain.Entities;
using TechStore.Domain.Interfaces;
using BCrypt.Net;

namespace TechStore.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> GetByIdAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByEmailAsync(string email)
        {
            var cliente = await _clienteRepository.GetByEmailAsync(email);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByDocumentoAsync(string documento)
        {
            var cliente = await _clienteRepository.GetByDocumentoAsync(documento);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto> CreateAsync(CreateClienteDto createClienteDto)
        {
            // Verificar si el email ya existe
            if (await _clienteRepository.EmailExistsAsync(createClienteDto.Email))
            {
                throw new ArgumentException("El email ya está registrado");
            }

            // Verificar si el documento ya existe
            if (await _clienteRepository.DocumentoExistsAsync(createClienteDto.Documento))
            {
                throw new ArgumentException("El documento ya está registrado");
            }

            var cliente = _mapper.Map<Cliente>(createClienteDto);

            // Hash de la contraseña (en un caso real usarías BCrypt o similar)
            cliente.PasswordHash = HashPassword(createClienteDto.Password);

            var createdCliente = await _clienteRepository.AddAsync(cliente);
            return _mapper.Map<ClienteDto>(createdCliente);
        }

        public async Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteDto updateClienteDto)
        {
            var existingCliente = await _clienteRepository.GetByIdAsync(id);
            if (existingCliente == null)
            {
                return null;
            }

            _mapper.Map(updateClienteDto, existingCliente);
            await _clienteRepository.UpdateAsync(existingCliente);

            return _mapper.Map<ClienteDto>(existingCliente);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await _clienteRepository.ExistsAsync(id))
            {
                return false;
            }

            await _clienteRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _clienteRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _clienteRepository.EmailExistsAsync(email);
        }

        public async Task<bool> DocumentoExistsAsync(string documento)
        {
            return await _clienteRepository.DocumentoExistsAsync(documento);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}