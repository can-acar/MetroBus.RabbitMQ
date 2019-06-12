using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Model;
using Models.Requests;
using Models.Responses;

namespace test.services.Service
{
    public class TestService : ITestService
    {
        private readonly ILogger<TestService> Logger;
        private readonly IBusControl Control;

        public TestService(ILogger<TestService> logger, IBusControl control)
        {
            Logger = logger;
            Control = control;
        }

        public async Task<BaseResponse<Test>> CreateItemAsync(CreateTestRequest request)
        {
            BaseResponse<Test> CreateItemResponse = new BaseResponse<Test>();
            
            try
            {
                var Dictionary = new Dictionary<string, string>();


                //some user create business logics

                CreateItemResponse.Data = new Test {Key = "1", Value = "Test Value"}; // User id

                await Control.Publish(new TestValueAddEvent
                {
                    Key = "1",
                    Value = "Test Value"
                });
            }
            catch (Exception ex)
            {
                CreateItemResponse.Errors.Add(ex.Message);
                Logger.LogError(ex, ex.Message);
            }

            return CreateItemResponse;
        }

        public Task GetItemAsync(string key)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ITestService
    {
        Task<BaseResponse<Test>> CreateItemAsync(CreateTestRequest request);
        Task GetItemAsync(string key);
    }
}