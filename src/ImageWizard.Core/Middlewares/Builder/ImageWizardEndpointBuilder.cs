// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

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
