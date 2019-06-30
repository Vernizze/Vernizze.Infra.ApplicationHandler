using Vernizze.Infra.ApplicationHandler.Interfaces.Service;
using Vernizze.Infra.CrossCutting.DataObjects.Results;
using Vernizze.Infra.CrossCutting.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ModelCross = Vernizze.Infra.CrossCutting.Validations.Base;

namespace Vernizze.Infra.ApplicationHandler.Service
{
    public abstract class BaseAppService
        : IBaseAppService
    {
        protected OperationResult<List<ValidationResult>> _models_validation_errors = new OperationResult<List<ValidationResult>>();
        protected OperationResultWithError<List<ValidationResult>> _business_validation_errors = new OperationResultWithError<List<ValidationResult>>();

        private bool _model_valid = true;
        private bool _business_valid = false;

        public bool ModelValid { get { return this._model_valid; } }
        public bool BusinessValid { get { return this._business_valid; } }

        public abstract TDerived DerivedType<TDerived>(TDerived value)
            where TDerived : IBaseAppService;

        public IBaseAppService ModelValidation<TRequest>(TRequest request)
        {
            var model_validations = new List<ValidationResult>();

            this._model_valid = ModelCross.ModelValidation.TryValidateObjectRecursive(request, model_validations);

            if (!this._model_valid)
            {
                this._models_validation_errors.result_object = model_validations;

                this._models_validation_errors.status_code = System.Net.HttpStatusCode.BadRequest;
            }

            return this;
        }

        public IBaseAppService BusinessValidation<TRequest>(TRequest request, Func<TRequest, OperationResultWithError<List<ValidationResult>>> validation_func)
        {
            this._business_validation_errors = validation_func(request);

            this._model_valid = (this._models_validation_errors.IsNotNull() && this._models_validation_errors.result_object.HaveAny());

            if (!this._model_valid)
                this._models_validation_errors.status_code = System.Net.HttpStatusCode.BadRequest;

            return this;
        }
    }
}
