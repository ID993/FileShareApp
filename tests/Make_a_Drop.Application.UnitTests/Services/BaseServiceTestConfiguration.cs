﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Make_a_Drop.Application.MappingProfiles;
using Make_a_Drop.DataAccess.Repositories;
using Make_a_Drop.Shared.Services;
using NSubstitute;

namespace Make_a_Drop.Application.UnitTests.Services;

public class BaseServiceTestConfiguration
{
    protected readonly IClaimService ClaimService;
    protected readonly IConfiguration Configuration;
    protected readonly IMapper Mapper;

    protected BaseServiceTestConfiguration()
    {
        var configurationBody = new Dictionary<string, string>
        {
            { "JwtConfiguration:SecretKey", "Super secret token key" }
        };

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationBody)
            .Build();

        ClaimService = Substitute.For<IClaimService>();
        ClaimService.GetUserId().Returns(new Guid().ToString());
    }
}
