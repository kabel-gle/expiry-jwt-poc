using AutoMapper;
using Expiry.Dtos;
using Expiry.Models;
using System;

namespace Expiry.Mappings
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Reminder, ReminderDto>().ReverseMap();
        }
    }
}
