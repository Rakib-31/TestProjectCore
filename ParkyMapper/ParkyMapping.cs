using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjectCore.Model;
using TestProjectCore.Model.DTOs;

namespace TestProjectCore.ParkyMapper
{
    public class ParkyMapping: Profile
    {
        public ParkyMapping()
        {
            CreateMap<NationalPark,NationalParkDto>().ReverseMap();
        }
    }
}
