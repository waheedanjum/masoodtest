using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using PsIntegrations.Interfaces;
using PsIntegrations.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockTesting
{
    public class AuthControllerTest
    {
        #region property
        public Mock<IJwtService> jwtService = new Mock<IJwtService>();
        public Mock<IMemoryCache> memoryCache = new Mock<IMemoryCache>();
        public Mock<IConfiguration> configuration = new Mock<IConfiguration>();
        #endregion

        [Fact]
        public async void AuthenticateParagon()
        {

        }

    }
}
