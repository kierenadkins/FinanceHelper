using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Domain.Base
{
    public class BaseResult
    {
        public BaseResult(string error)
        {
            Errors.Add(error);
        }

        public BaseResult()
        {
        }

        public bool Success => Errors.Count == 0;
        public List<string> Errors { get; } = new();

        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors.Add(error);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors.Where(e => !string.IsNullOrWhiteSpace(e)))
                Errors.Add(error);
        }
    }

    public class BaseResult<T> : BaseResult
    {
        public BaseResult() { }

        public BaseResult(T value)
        {
            Value = value;
        }

        public BaseResult(string error) : base(error) { }

        public T? Value { get; set; }

        public static BaseResult<T> SuccessResult(T value)
            => new BaseResult<T>(value);

        public static BaseResult<T> ErrorResult(string error)
            => new BaseResult<T>(error);
    }
}