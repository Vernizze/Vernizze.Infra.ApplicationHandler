using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vernizze.Infra.CrossCutting.DataObjects.Results;

namespace Vernizze.Infra.ApplicationHandler.Interfaces.Service
{
    public interface IBaseAppService
    {
        TDerived DerivedType<TDerived>(TDerived value)
            where TDerived : IBaseAppService;
        IBaseAppService ModelValidation<TRequest>(TRequest request);
        IBaseAppService BusinessValidation<TRequest>(TRequest request, Func<TRequest, OperationResultWithError<List<ValidationResult>>> validation_func);
    }
}
