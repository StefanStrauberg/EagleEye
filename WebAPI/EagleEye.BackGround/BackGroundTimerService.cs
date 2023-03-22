using EagleEye.Application.Contracts.Logger;
using EagleEye.Application.Contracts.TemporaryBuffer;
using EagleEye.BackGround.Config;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EagleEye.BackGround
{
    internal class BackGroundTimerService : BackgroundService
    {
        Timer _timer;
        readonly IAppLogger<BackGroundTimerService> _logger;
        readonly IBufferRepository _bufferRepository;
        readonly IBackGroundTimer _backGroundTimer;

        public BackGroundTimerService(IAppLogger<BackGroundTimerService> logger,
                               IBufferRepository bufferRepository,
                               IBackGroundTimer backGroundTimer)
        {
            _logger = logger;
            _bufferRepository = bufferRepository;
            _backGroundTimer = backGroundTimer;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background timer service started");

            _timer = new Timer(ResetBuffer,
                               null,
                               TimeSpan.Zero,
                               TimeSpan.FromSeconds(_backGroundTimer.DelayTimer));

            return Task.CompletedTask;
        }

        void ResetBuffer(Object obj)
        {
            if (_bufferRepository.CountItems() is not 0)
                _bufferRepository.ClearBuffer();
        }
    }
}