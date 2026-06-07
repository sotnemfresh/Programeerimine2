﻿namespace KooliProjekt.Application.Infrastructure.Results
{
    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }

        public OperationResult() { }

        public OperationResult(T value) 
        { 
            Value = value;
        }        

        public new OperationResult<T> AddError(string error)
        {
            base.AddError(error);

            return this;
        }

        public new OperationResult<T> AddPropertyError(string propertyName, string error)
        {
            base.AddPropertyError(propertyName, error);

            return this;
        }

        public static OperationResult<T> Success(T value)
        {
            return new OperationResult<T>(value); // Kasutab ülemist konstruktorit
        }

        public static OperationResult<T> Failure(string error)
        {
            var result = new OperationResult<T>();
            result.AddError(error); // Kasutab baasklassi meetodit
            return result;
        }
    }
}