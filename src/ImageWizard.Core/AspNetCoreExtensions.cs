using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ImageWizard
{
    static class AspNetCoreExtensions
    {
        /// <summary>
        /// Fixed version
        /// see https://github.com/aspnet/Extensions/issues/2077
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClient2<TClient>(this IServiceCollection services) where TClient : class
        {
            try
            {
                services.AddHttpClient<TClient>();
            }
            catch
            {
                //ignored if called multiple times (asp.net core 3.0)
                //see https://github.com/aspnet/Extensions/issues/2077
            }

            return services;
        }

        public static string GetTagUnquoted(this EntityTagHeaderValue value)
        {
            return value.Tag.Substring(1, value.Tag.Length - 2);
        }
    }
}
