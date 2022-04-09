// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard;

public class ImageWizardEndpointBuilder : IImageWizardEndpointBuilder
{
    public ImageWizardEndpointBuilder(IEndpointRouteBuilder innerBuilder)
    {
        InnerEndpoints = innerBuilder;
    }

    private IEndpointRouteBuilder InnerEndpoints { get; }

    IServiceProvider IEndpointRouteBuilder.ServiceProvider => InnerEndpoints.ServiceProvider;

    ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources => InnerEndpoints.DataSources;

    IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder() => InnerEndpoints.CreateApplicationBuilder();
}
