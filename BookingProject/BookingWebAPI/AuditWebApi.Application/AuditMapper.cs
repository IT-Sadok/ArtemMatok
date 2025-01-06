using AuditWebApi.Application.UserAudit.DTOs;
using AuditWebApi.Domain.Entities;
using AutoMapper;
using Contracts.DTOs.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditWebApi.Application
{
    public class AuditMapper : Profile
    {
        public AuditMapper()
        {
            CreateMap<AuditRecord, AuditChangeDto>();
            CreateMap<AuditBookingCreateDto, AuditBooking>();
        }
    }
}
