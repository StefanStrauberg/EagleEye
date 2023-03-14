﻿using FGLogDog.Application.Models;
using System;
using System.Threading.Tasks;

namespace FGLogDog.Application.Contracts
{
    public interface IProducer<in T> : IDisposable where T : ProducerParameters
    {
        Task Run(T parameters);
    }
}