using System;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using test.services.Service;

namespace test.services.Consumer
{
    public class TestActionConsumer : IConsumer<TestValueAddEvent>
    {
        private readonly ITestService TestService;

        public TestActionConsumer(ITestService testService)
        {
            TestService = testService;
        }

        public async Task Consume(ConsumeContext<TestValueAddEvent> context)
        {
            TestService.GetItemAsync(context.Message.Key);

            await Console.Out.WriteLineAsync($"{context.Message.Key} {context.Message.Value}");
        }
    }
}