﻿using FluentValidation.Results;

namespace Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public CustomValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public CustomValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
